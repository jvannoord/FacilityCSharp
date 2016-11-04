﻿using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Facility.Core.Http
{
	/// <summary>
	/// Sets the Authorization header of the request.
	/// </summary>
	public sealed class AuthorizationClientAspect : HttpClientServiceAspect
	{
		/// <summary>
		/// Creates an aspect that sets the Authorization header to the specified string.
		/// </summary>
		public static HttpClientServiceAspect Create(string authorizationHeader)
		{
			return new AuthorizationClientAspect(authorizationHeader);
		}

		/// <summary>
		/// Called right before the request is sent.
		/// </summary>
		protected override Task RequestReadyAsyncCore(HttpRequestMessage request, ServiceDto requestDto, CancellationToken cancellationToken)
		{
			if (m_authorizationHeader != null)
				request.Headers.Authorization = m_authorizationHeader;
			return Task.FromResult<object>(null);
		}

		private AuthorizationClientAspect(string authorizationHeader)
		{
			if (!string.IsNullOrWhiteSpace(authorizationHeader))
				m_authorizationHeader = AuthenticationHeaderValue.Parse(authorizationHeader);
		}

		readonly AuthenticationHeaderValue m_authorizationHeader;
	}
}
