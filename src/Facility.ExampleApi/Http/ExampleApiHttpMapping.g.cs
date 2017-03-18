// DO NOT EDIT: generated by fsdgencsharp
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using Facility.Core;
using Facility.Core.Http;

#pragma warning disable 612, 618 // member is obsolete

namespace Facility.ExampleApi.Http
{
	/// <summary>
	/// Example service for widgets.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("fsdgencsharp", "")]
	public static partial class ExampleApiHttpMapping
	{
		/// <summary>
		/// Gets widgets.
		/// </summary>
		public static readonly HttpMethodMapping<GetWidgetsRequestDto, GetWidgetsResponseDto> GetWidgetsMapping =
			new HttpMethodMapping<GetWidgetsRequestDto, GetWidgetsResponseDto>.Builder
			{
				HttpMethod = HttpMethod.Get,
				Path = "/widgets",
				GetUriParameters = request =>
					new Dictionary<string, string>
					{
						{ "q", request.Query },
						{ "limit", request.Limit == null ? null : request.Limit.Value.ToString(CultureInfo.InvariantCulture) },
						{ "sort", request.Sort == null ? null : request.Sort.Value.ToString() },
						{ "desc", request.Desc == null ? null : request.Desc.Value.ToString() },
						{ "maxWeight", request.MaxWeight == null ? null : request.MaxWeight.Value.ToString(CultureInfo.InvariantCulture) },
						{ "minPrice", request.MinPrice == null ? null : request.MinPrice.Value.ToString(CultureInfo.InvariantCulture) },
					},
				SetUriParameters = (request, parameters) =>
				{
					string queryParameterQuery;
					parameters.TryGetValue("q", out queryParameterQuery);
					request.Query = queryParameterQuery;
					string queryParameterLimit;
					parameters.TryGetValue("limit", out queryParameterLimit);
					request.Limit = ServiceDataUtility.TryParseInt32(queryParameterLimit);
					string queryParameterSort;
					parameters.TryGetValue("sort", out queryParameterSort);
					request.Sort = queryParameterSort == null ? default(WidgetField?) : new WidgetField(queryParameterSort);
					string queryParameterDesc;
					parameters.TryGetValue("desc", out queryParameterDesc);
					request.Desc = ServiceDataUtility.TryParseBoolean(queryParameterDesc);
					string queryParameterMaxWeight;
					parameters.TryGetValue("maxWeight", out queryParameterMaxWeight);
					request.MaxWeight = ServiceDataUtility.TryParseDouble(queryParameterMaxWeight);
					string queryParameterMinPrice;
					parameters.TryGetValue("minPrice", out queryParameterMinPrice);
					request.MinPrice = ServiceDataUtility.TryParseDecimal(queryParameterMinPrice);
					return request;
				},
				ResponseMappings =
				{
					new HttpResponseMapping<GetWidgetsResponseDto>.Builder
					{
						StatusCode = (HttpStatusCode) 200,
						ResponseBodyType = typeof(GetWidgetsResponseDto),
						GetResponseBody = response =>
							new GetWidgetsResponseDto
							{
								Widgets = response.Widgets,
								Total = response.Total,
								TotalWeight = response.TotalWeight,
							},
						CreateResponse = body =>
							new GetWidgetsResponseDto
							{
								Widgets = ((GetWidgetsResponseDto) body).Widgets,
								Total = ((GetWidgetsResponseDto) body).Total,
								TotalWeight = ((GetWidgetsResponseDto) body).TotalWeight,
							},
					}.Build(),
					new HttpResponseMapping<GetWidgetsResponseDto>.Builder
					{
						StatusCode = (HttpStatusCode) 202,
						ResponseBodyType = typeof(WidgetJobDto),
						MatchesResponse = response => response.Job != null,
						GetResponseBody = response => response.Job,
						CreateResponse = body => new GetWidgetsResponseDto { Job = (WidgetJobDto) body },
					}.Build(),
				},
			}.Build();

		/// <summary>
		/// Creates a new widget.
		/// </summary>
		public static readonly HttpMethodMapping<CreateWidgetRequestDto, CreateWidgetResponseDto> CreateWidgetMapping =
			new HttpMethodMapping<CreateWidgetRequestDto, CreateWidgetResponseDto>.Builder
			{
				HttpMethod = HttpMethod.Post,
				Path = "/widgets",
				ValidateRequest = request =>
				{
					if (request.Widget == null)
						return ServiceResult.Failure(ServiceErrors.CreateRequestFieldRequired("widget"));
					return ServiceResult.Success();
				},
				RequestBodyType = typeof(WidgetDto),
				GetRequestBody = request => request.Widget,
				CreateRequest = body => new CreateWidgetRequestDto{ Widget = (WidgetDto) body },
				ResponseMappings =
				{
					new HttpResponseMapping<CreateWidgetResponseDto>.Builder
					{
						StatusCode = (HttpStatusCode) 201,
						ResponseBodyType = typeof(WidgetDto),
						MatchesResponse = response => response.Widget != null,
						GetResponseBody = response => response.Widget,
						CreateResponse = body => new CreateWidgetResponseDto { Widget = (WidgetDto) body },
					}.Build(),
				},
			}.Build();

		/// <summary>
		/// Gets the specified widget.
		/// </summary>
		public static readonly HttpMethodMapping<GetWidgetRequestDto, GetWidgetResponseDto> GetWidgetMapping =
			new HttpMethodMapping<GetWidgetRequestDto, GetWidgetResponseDto>.Builder
			{
				HttpMethod = HttpMethod.Get,
				Path = "/widgets/{id}",
				ValidateRequest = request =>
				{
					if (string.IsNullOrEmpty(request.Id))
						return ServiceResult.Failure(ServiceErrors.CreateRequestFieldRequired("id"));
					return ServiceResult.Success();
				},
				GetUriParameters = request =>
					new Dictionary<string, string>
					{
						{ "id", request.Id },
					},
				SetUriParameters = (request, parameters) =>
				{
					string queryParameterId;
					parameters.TryGetValue("id", out queryParameterId);
					request.Id = queryParameterId;
					return request;
				},
				GetRequestHeaders = request =>
					new Dictionary<string, string>
					{
						{ "If-None-Match", request.IfNoneMatch },
					},
				SetRequestHeaders = (request, headers) =>
				{
					string headerIfNoneMatch;
					headers.TryGetValue("If-None-Match", out headerIfNoneMatch);
					request.IfNoneMatch = headerIfNoneMatch;
					return request;
				},
				ResponseMappings =
				{
					new HttpResponseMapping<GetWidgetResponseDto>.Builder
					{
						StatusCode = (HttpStatusCode) 200,
						ResponseBodyType = typeof(WidgetDto),
						MatchesResponse = response => response.Widget != null,
						GetResponseBody = response => response.Widget,
						CreateResponse = body => new GetWidgetResponseDto { Widget = (WidgetDto) body },
					}.Build(),
					new HttpResponseMapping<GetWidgetResponseDto>.Builder
					{
						StatusCode = (HttpStatusCode) 304,
						MatchesResponse = response => response.NotModified.GetValueOrDefault(),
						CreateResponse = body => new GetWidgetResponseDto { NotModified = true },
					}.Build(),
				},
				GetResponseHeaders = response =>
					new Dictionary<string, string>
					{
						{ "eTag", response.ETag },
					},
				SetResponseHeaders = (response, headers) =>
				{
					string headerETag;
					headers.TryGetValue("eTag", out headerETag);
					response.ETag = headerETag;
					return response;
				},
			}.Build();

		/// <summary>
		/// Deletes the specified widget.
		/// </summary>
		public static readonly HttpMethodMapping<DeleteWidgetRequestDto, DeleteWidgetResponseDto> DeleteWidgetMapping =
			new HttpMethodMapping<DeleteWidgetRequestDto, DeleteWidgetResponseDto>.Builder
			{
				HttpMethod = HttpMethod.Delete,
				Path = "/widgets/{id}",
				ValidateRequest = request =>
				{
					if (string.IsNullOrEmpty(request.Id))
						return ServiceResult.Failure(ServiceErrors.CreateRequestFieldRequired("id"));
					return ServiceResult.Success();
				},
				GetUriParameters = request =>
					new Dictionary<string, string>
					{
						{ "id", request.Id },
					},
				SetUriParameters = (request, parameters) =>
				{
					string queryParameterId;
					parameters.TryGetValue("id", out queryParameterId);
					request.Id = queryParameterId;
					return request;
				},
				ResponseMappings =
				{
					new HttpResponseMapping<DeleteWidgetResponseDto>.Builder
					{
						StatusCode = (HttpStatusCode) 204,
					}.Build(),
				},
			}.Build();

		/// <summary>
		/// Edits widget.
		/// </summary>
		public static readonly HttpMethodMapping<EditWidgetRequestDto, EditWidgetResponseDto> EditWidgetMapping =
			new HttpMethodMapping<EditWidgetRequestDto, EditWidgetResponseDto>.Builder
			{
				HttpMethod = HttpMethod.Post,
				Path = "/widgets/{id}",
				ValidateRequest = request =>
				{
					if (string.IsNullOrEmpty(request.Id))
						return ServiceResult.Failure(ServiceErrors.CreateRequestFieldRequired("id"));
					return ServiceResult.Success();
				},
				GetUriParameters = request =>
					new Dictionary<string, string>
					{
						{ "id", request.Id },
					},
				SetUriParameters = (request, parameters) =>
				{
					string queryParameterId;
					parameters.TryGetValue("id", out queryParameterId);
					request.Id = queryParameterId;
					return request;
				},
				RequestBodyType = typeof(EditWidgetRequestDto),
				GetRequestBody = request =>
					new EditWidgetRequestDto
					{
						Ops = request.Ops,
						Weight = request.Weight,
					},
				CreateRequest = body =>
					new EditWidgetRequestDto
					{
						Ops = ((EditWidgetRequestDto) body).Ops,
						Weight = ((EditWidgetRequestDto) body).Weight,
					},
				ResponseMappings =
				{
					new HttpResponseMapping<EditWidgetResponseDto>.Builder
					{
						StatusCode = (HttpStatusCode) 200,
						ResponseBodyType = typeof(WidgetDto),
						MatchesResponse = response => response.Widget != null,
						GetResponseBody = response => response.Widget,
						CreateResponse = body => new EditWidgetResponseDto { Widget = (WidgetDto) body },
					}.Build(),
					new HttpResponseMapping<EditWidgetResponseDto>.Builder
					{
						StatusCode = (HttpStatusCode) 202,
						ResponseBodyType = typeof(WidgetJobDto),
						MatchesResponse = response => response.Job != null,
						GetResponseBody = response => response.Job,
						CreateResponse = body => new EditWidgetResponseDto { Job = (WidgetJobDto) body },
					}.Build(),
				},
			}.Build();

		/// <summary>
		/// Gets the specified widgets.
		/// </summary>
		public static readonly HttpMethodMapping<GetWidgetBatchRequestDto, GetWidgetBatchResponseDto> GetWidgetBatchMapping =
			new HttpMethodMapping<GetWidgetBatchRequestDto, GetWidgetBatchResponseDto>.Builder
			{
				HttpMethod = HttpMethod.Post,
				Path = "/widgets/get",
				ValidateRequest = request =>
				{
					if (request.Ids == null)
						return ServiceResult.Failure(ServiceErrors.CreateRequestFieldRequired("ids"));
					return ServiceResult.Success();
				},
				RequestBodyType = typeof(IReadOnlyList<string>),
				GetRequestBody = request => request.Ids,
				CreateRequest = body => new GetWidgetBatchRequestDto{ Ids = (IReadOnlyList<string>) body },
				ResponseMappings =
				{
					new HttpResponseMapping<GetWidgetBatchResponseDto>.Builder
					{
						StatusCode = (HttpStatusCode) 200,
						ResponseBodyType = typeof(IReadOnlyList<ServiceResult<WidgetDto>>),
						MatchesResponse = response => response.Results != null,
						GetResponseBody = response => response.Results,
						CreateResponse = body => new GetWidgetBatchResponseDto { Results = (IReadOnlyList<ServiceResult<WidgetDto>>) body },
					}.Build(),
				},
			}.Build();

		/// <summary>
		/// Gets the widget weight.
		/// </summary>
		[Obsolete]
		public static readonly HttpMethodMapping<GetWidgetWeightRequestDto, GetWidgetWeightResponseDto> GetWidgetWeightMapping =
			new HttpMethodMapping<GetWidgetWeightRequestDto, GetWidgetWeightResponseDto>.Builder
			{
				HttpMethod = HttpMethod.Get,
				Path = "/widgets/{id}/weight",
				ValidateRequest = request =>
				{
					if (string.IsNullOrEmpty(request.Id))
						return ServiceResult.Failure(ServiceErrors.CreateRequestFieldRequired("id"));
					return ServiceResult.Success();
				},
				GetUriParameters = request =>
					new Dictionary<string, string>
					{
						{ "id", request.Id },
					},
				SetUriParameters = (request, parameters) =>
				{
					string queryParameterId;
					parameters.TryGetValue("id", out queryParameterId);
					request.Id = queryParameterId;
					return request;
				},
				ResponseMappings =
				{
					new HttpResponseMapping<GetWidgetWeightResponseDto>.Builder
					{
						StatusCode = (HttpStatusCode) 200,
						ResponseBodyType = typeof(GetWidgetWeightResponseDto),
					}.Build(),
				},
			}.Build();

		/// <summary>
		/// Gets a widget preference.
		/// </summary>
		public static readonly HttpMethodMapping<GetPreferenceRequestDto, GetPreferenceResponseDto> GetPreferenceMapping =
			new HttpMethodMapping<GetPreferenceRequestDto, GetPreferenceResponseDto>.Builder
			{
				HttpMethod = HttpMethod.Get,
				Path = "/prefs/{key}",
				ValidateRequest = request =>
				{
					if (string.IsNullOrEmpty(request.Key))
						return ServiceResult.Failure(ServiceErrors.CreateRequestFieldRequired("key"));
					return ServiceResult.Success();
				},
				GetUriParameters = request =>
					new Dictionary<string, string>
					{
						{ "key", request.Key },
					},
				SetUriParameters = (request, parameters) =>
				{
					string queryParameterKey;
					parameters.TryGetValue("key", out queryParameterKey);
					request.Key = queryParameterKey;
					return request;
				},
				ResponseMappings =
				{
					new HttpResponseMapping<GetPreferenceResponseDto>.Builder
					{
						StatusCode = (HttpStatusCode) 200,
						ResponseBodyType = typeof(PreferenceDto),
						MatchesResponse = response => response.Value != null,
						GetResponseBody = response => response.Value,
						CreateResponse = body => new GetPreferenceResponseDto { Value = (PreferenceDto) body },
					}.Build(),
				},
			}.Build();

		/// <summary>
		/// Sets a widget preference.
		/// </summary>
		public static readonly HttpMethodMapping<SetPreferenceRequestDto, SetPreferenceResponseDto> SetPreferenceMapping =
			new HttpMethodMapping<SetPreferenceRequestDto, SetPreferenceResponseDto>.Builder
			{
				HttpMethod = HttpMethod.Put,
				Path = "/prefs/{key}",
				ValidateRequest = request =>
				{
					if (string.IsNullOrEmpty(request.Key))
						return ServiceResult.Failure(ServiceErrors.CreateRequestFieldRequired("key"));
					if (request.Value == null)
						return ServiceResult.Failure(ServiceErrors.CreateRequestFieldRequired("value"));
					return ServiceResult.Success();
				},
				GetUriParameters = request =>
					new Dictionary<string, string>
					{
						{ "key", request.Key },
					},
				SetUriParameters = (request, parameters) =>
				{
					string queryParameterKey;
					parameters.TryGetValue("key", out queryParameterKey);
					request.Key = queryParameterKey;
					return request;
				},
				RequestBodyType = typeof(PreferenceDto),
				GetRequestBody = request => request.Value,
				CreateRequest = body => new SetPreferenceRequestDto{ Value = (PreferenceDto) body },
				ResponseMappings =
				{
					new HttpResponseMapping<SetPreferenceResponseDto>.Builder
					{
						StatusCode = (HttpStatusCode) 200,
						ResponseBodyType = typeof(PreferenceDto),
						MatchesResponse = response => response.Value != null,
						GetResponseBody = response => response.Value,
						CreateResponse = body => new SetPreferenceResponseDto { Value = (PreferenceDto) body },
					}.Build(),
				},
			}.Build();

		/// <summary>
		/// Gets service info.
		/// </summary>
		public static readonly HttpMethodMapping<GetInfoRequestDto, GetInfoResponseDto> GetInfoMapping =
			new HttpMethodMapping<GetInfoRequestDto, GetInfoResponseDto>.Builder
			{
				HttpMethod = HttpMethod.Get,
				Path = "/",
				ResponseMappings =
				{
					new HttpResponseMapping<GetInfoResponseDto>.Builder
					{
						StatusCode = (HttpStatusCode) 200,
						ResponseBodyType = typeof(GetInfoResponseDto),
					}.Build(),
				},
			}.Build();

		/// <summary>
		/// Demonstrates the default HTTP behavior.
		/// </summary>
		public static readonly HttpMethodMapping<NotRestfulRequestDto, NotRestfulResponseDto> NotRestfulMapping =
			new HttpMethodMapping<NotRestfulRequestDto, NotRestfulResponseDto>.Builder
			{
				HttpMethod = HttpMethod.Post,
				Path = "/notRestful",
				ResponseMappings =
				{
					new HttpResponseMapping<NotRestfulResponseDto>.Builder
					{
						StatusCode = (HttpStatusCode) 200,
					}.Build(),
				},
			}.Build();

		public static readonly HttpMethodMapping<KitchenRequestDto, KitchenResponseDto> KitchenMapping =
			new HttpMethodMapping<KitchenRequestDto, KitchenResponseDto>.Builder
			{
				HttpMethod = HttpMethod.Post,
				Path = "/kitchen",
				RequestBodyType = typeof(KitchenRequestDto),
				ResponseMappings =
				{
					new HttpResponseMapping<KitchenResponseDto>.Builder
					{
						StatusCode = (HttpStatusCode) 200,
					}.Build(),
				},
			}.Build();
	}
}
