// DO NOT EDIT: generated by fsdgencsharp
using System;
using System.Collections.Generic;
using Facility.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#pragma warning disable 612, 618 // member is obsolete

namespace Facility.ExampleApi
{
	/// <summary>
	/// Request for GetWidgets.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("fsdgencsharp", "")]
	public sealed partial class GetWidgetsRequestDto : ServiceDto<GetWidgetsRequestDto>
	{
		/// <summary>
		/// Creates an instance.
		/// </summary>
		public GetWidgetsRequestDto()
		{
		}

		/// <summary>
		/// The query.
		/// </summary>
		public string Query { get; set; }

		/// <summary>
		/// The limit of returned results.
		/// </summary>
		public int? Limit { get; set; }

		/// <summary>
		/// The sort field.
		/// </summary>
		public WidgetField? Sort { get; set; }

		/// <summary>
		/// True to sort descending.
		/// </summary>
		public bool? Desc { get; set; }

		/// <summary>
		/// The maximum weight.
		/// </summary>
		[Obsolete]
		public double? MaxWeight { get; set; }

		/// <summary>
		/// The minimum price.
		/// </summary>
		public decimal? MinPrice { get; set; }

		/// <summary>
		/// Determines if two DTOs are equivalent.
		/// </summary>
		public override bool IsEquivalentTo(GetWidgetsRequestDto other)
		{
			return other != null &&
				Query == other.Query &&
				Limit == other.Limit &&
				Sort == other.Sort &&
				Desc == other.Desc &&
				MaxWeight == other.MaxWeight &&
				MinPrice == other.MinPrice;
		}
	}
}
