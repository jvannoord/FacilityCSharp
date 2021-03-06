using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Facility.Core.Http
{
	/// <summary>
	/// Used by HTTP clients.
	/// </summary>
	public abstract class HttpClientService
	{
		/// <summary>
		/// Creates an instance with the specified settings.
		/// </summary>
		protected HttpClientService(HttpClientServiceSettings settings, Uri defaultBaseUri)
		{
			settings = settings ?? new HttpClientServiceSettings();

			m_httpClient = settings.HttpClient ?? s_defaultHttpClient;
			m_aspects = settings.Aspects;
			m_synchronous = settings.Synchronous;

			var baseUri = settings.BaseUri ?? defaultBaseUri;
			m_baseUrl = baseUri == null ? "" : (baseUri.IsAbsoluteUri ? baseUri.AbsoluteUri : baseUri.OriginalString);

			ContentSerializer = settings.ContentSerializer ?? JsonHttpContentSerializer.Instance;
		}

		/// <summary>
		/// The HTTP content serializer.
		/// </summary>
		protected HttpContentSerializer ContentSerializer { get; }

		/// <summary>
		/// Sends an HTTP request and processes the response.
		/// </summary>
		protected async Task<ServiceResult<TResponse>> TrySendRequestAsync<TRequest, TResponse>(HttpMethodMapping<TRequest, TResponse> mapping, TRequest request, CancellationToken cancellationToken)
			where TRequest : ServiceDto, new()
			where TResponse : ServiceDto, new()
		{
			if (mapping == null)
				throw new ArgumentNullException(nameof(mapping));
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			try
			{
				// validate the request DTO
				var requestValidation = mapping.ValidateRequest(request);
				if (requestValidation.IsFailure)
					return requestValidation.AsFailure();

				// create the HTTP request with the right method, path, query string, and headers
				var httpRequestResult = TryCreateHttpRequest(mapping.HttpMethod, mapping.Path, mapping.GetUriParameters(request), mapping.GetRequestHeaders(request));
				if (httpRequestResult.IsFailure)
					return httpRequestResult.AsFailure();
				using (var httpRequest = httpRequestResult.Value)
				{
					// create the request body if necessary
					var requestBody = mapping.GetRequestBody(request);
					if (requestBody != null)
						httpRequest.Content = ContentSerializer.CreateHttpContent(requestBody);

					// send the HTTP request and get the HTTP response
					using (var httpResponse = await SendRequestAsync(httpRequest, request, cancellationToken).ConfigureAwait(false))
					{
						// find the response mapping for the status code
						var statusCode = httpResponse.StatusCode;
						var responseMapping = mapping.ResponseMappings.FirstOrDefault(x => x.StatusCode == statusCode);

						// fail if no response mapping can be found for the status code
						if (responseMapping == null)
							return ServiceResult.Failure(await CreateErrorFromHttpResponseAsync(httpResponse, cancellationToken).ConfigureAwait(false));

						// read the response body if necessary
						object responseBody = null;
						if (responseMapping.ResponseBodyType != null)
						{
							ServiceResult<object> responseResult = await ContentSerializer.ReadHttpContentAsync(
								responseMapping.ResponseBodyType, httpResponse.Content, cancellationToken).ConfigureAwait(false);
							if (responseResult.IsFailure)
							{
								var error = responseResult.Error;
								error.Code = ServiceErrors.InvalidResponse;
								return ServiceResult.Failure(error);
							}
							responseBody = responseResult.Value;
						}

						// create the response DTO
						var response = responseMapping.CreateResponse(responseBody);
						response = mapping.SetResponseHeaders(response, HttpServiceUtility.CreateDictionaryFromHeaders(httpResponse.Headers));
						return ServiceResult.Success(response);
					}
				}
			}
			catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
			{
				// HttpClient timeout
				return ServiceResult.Failure(ServiceErrors.CreateTimeout());
			}
			catch (Exception exception) when (ShouldCreateErrorFromException(exception))
			{
				// cancellation can cause the wrong exception
				cancellationToken.ThrowIfCancellationRequested();

				// error contacting service
				return ServiceResult.Failure(CreateErrorFromException(exception));
			}
		}

		/// <summary>
		/// Called to create an error object from an unhandled HTTP response.
		/// </summary>
		protected virtual async Task<ServiceErrorDto> CreateErrorFromHttpResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
		{
			var result = await ContentSerializer.ReadHttpContentAsync<ServiceErrorDto>(response.Content, cancellationToken).ConfigureAwait(false);

			if (result.IsFailure || string.IsNullOrWhiteSpace(result.Value?.Code))
				return HttpServiceErrors.CreateErrorForStatusCode(response.StatusCode, response.ReasonPhrase);

			return result.Value;
		}

		/// <summary>
		/// Called to determine if an error object should be created from an unexpected exception.
		/// </summary>
		protected virtual bool ShouldCreateErrorFromException(Exception exception)
		{
			if (exception is ArgumentException || exception is ObjectDisposedException || exception is AggregateException)
				return true;

			string exceptionTypeName = exception.GetType().FullName;
			return exceptionTypeName != null && (
				exceptionTypeName.StartsWith("System.Net.", StringComparison.Ordinal) ||
				exceptionTypeName.StartsWith("System.IO.", StringComparison.Ordinal) ||
				exceptionTypeName.StartsWith("System.Web.", StringComparison.Ordinal));
		}

		/// <summary>
		/// Called to create an error object from an unexpected exception.
		/// </summary>
		protected virtual ServiceErrorDto CreateErrorFromException(Exception exception) => ServiceErrorUtility.CreateInternalErrorForException(exception);

		private async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage httpRequest, ServiceDto requestDto, CancellationToken cancellationToken)
		{
			if (m_aspects != null)
			{
				foreach (var aspect in m_aspects)
					await AdaptTask(aspect.RequestReadyAsync(httpRequest, requestDto, cancellationToken)).ConfigureAwait(true);
			}

			var httpResponse = await AdaptTask(m_httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken)).ConfigureAwait(true);

			if (m_aspects != null)
			{
				foreach (var aspect in m_aspects)
					await AdaptTask(aspect.ResponseReceivedAsync(httpResponse, requestDto, cancellationToken)).ConfigureAwait(true);
			}

			return httpResponse;
		}

		private ServiceResult<HttpRequestMessage> TryCreateHttpRequest(HttpMethod httpMethod, string relativeUrlPattern, IEnumerable<KeyValuePair<string, string>> uriParameters, IEnumerable<KeyValuePair<string, string>> requestHeaders)
		{
			string url = m_baseUrl.TrimEnd('/') + "/" + relativeUrlPattern.TrimStart('/');
			if (uriParameters != null)
				url = GetUrlFromPattern(url, uriParameters);

			var requestMessage = new HttpRequestMessage(httpMethod, url);

			var headersResult = HttpServiceUtility.TryAddHeaders(requestMessage.Headers, requestHeaders);
			if (headersResult.IsFailure)
				return headersResult.AsFailure();

			return ServiceResult.Success(requestMessage);
		}

		private static string GetUrlFromPattern(string url, IEnumerable<KeyValuePair<string, string>> parameters)
		{
			bool hasQuery = url.IndexOf('?') != -1;

			foreach (KeyValuePair<string, string> parameter in parameters)
			{
				if (parameter.Key != null && parameter.Value != null)
				{
					string bracketedKey = "{" + parameter.Key + "}";
					int bracketedKeyIndex = url.IndexOf(bracketedKey, StringComparison.Ordinal);
					if (bracketedKeyIndex != -1)
					{
						url = url.Substring(0, bracketedKeyIndex) +
							Uri.EscapeDataString(parameter.Value) + url.Substring(bracketedKeyIndex + bracketedKey.Length);
					}
					else
					{
						url += (hasQuery ? "&" : "?") + Uri.EscapeDataString(parameter.Key) + "=" + Uri.EscapeDataString(parameter.Value);
						hasQuery = true;
					}
				}
			}

			return url;
		}

		private Task AdaptTask(Task task)
		{
			if (!m_synchronous)
				return task;

			task.GetAwaiter().GetResult();
			return HttpServiceUtility.CompletedTask;
		}

		private Task<T> AdaptTask<T>(Task<T> task)
		{
			if (!m_synchronous)
				return task;

			return Task.FromResult(task.GetAwaiter().GetResult());
		}

		static readonly HttpClient s_defaultHttpClient = HttpServiceUtility.CreateHttpClient();

		readonly HttpClient m_httpClient;
		readonly IReadOnlyList<HttpClientServiceAspect> m_aspects;
		readonly bool m_synchronous;
		readonly string m_baseUrl;
	}
}
