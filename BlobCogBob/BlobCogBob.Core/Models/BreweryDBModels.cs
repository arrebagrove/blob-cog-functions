// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using BlobCogBob.Core;
//
//    var data = Welcome.FromJson(jsonString);

namespace BlobCogBob.Core
{
    using System;
    using System.Net;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public partial class BreweryDbSearchResult
    {
        [JsonProperty("currentPage")]
        public long CurrentPage { get; set; }

        [JsonProperty("numberOfPages")]
        public long NumberOfPages { get; set; }

        [JsonProperty("totalResults")]
        public long TotalResults { get; set; }

        [JsonProperty("data")]
        public List<BreweryDbBeerInfo> Data { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public partial class BreweryDbBeerInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nameDisplay")]
        public string NameDisplay { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("abv")]
        public string Abv { get; set; }

        [JsonProperty("glasswareId")]
        public long? GlasswareId { get; set; }

        [JsonProperty("availableId")]
        public long? AvailableId { get; set; }

        [JsonProperty("styleId")]
        public long StyleId { get; set; }

        [JsonProperty("isOrganic")]
        public string IsOrganic { get; set; }

        [JsonProperty("labels")]
        public Labels Labels { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("statusDisplay")]
        public string StatusDisplay { get; set; }

        [JsonProperty("createDate")]
        public System.DateTime CreateDate { get; set; }

        [JsonProperty("updateDate")]
        public System.DateTime UpdateDate { get; set; }

        [JsonProperty("glass")]
        public Glass Glass { get; set; }

        [JsonProperty("available")]
        public Available Available { get; set; }

        [JsonProperty("style")]
        public Style Style { get; set; }

        [JsonProperty("type")]
        public string PurpleType { get; set; }

        [JsonProperty("ibu")]
        public string Ibu { get; set; }

        [JsonProperty("foodPairings")]
        public string FoodPairings { get; set; }

        [JsonProperty("originalGravity")]
        public string OriginalGravity { get; set; }
    }

    public partial class Available
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public partial class Glass
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("createDate")]
        public System.DateTime CreateDate { get; set; }
    }

    public partial class Labels
    {
        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("medium")]
        public string Medium { get; set; }

        [JsonProperty("large")]
        public string Large { get; set; }
    }

    public partial class Style
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("categoryId")]
        public long CategoryId { get; set; }

        [JsonProperty("category")]
        public Glass Category { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("shortName")]
        public string ShortName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("ibuMin")]
        public string IbuMin { get; set; }

        [JsonProperty("ibuMax")]
        public string IbuMax { get; set; }

        [JsonProperty("abvMin")]
        public string AbvMin { get; set; }

        [JsonProperty("abvMax")]
        public string AbvMax { get; set; }

        [JsonProperty("srmMin")]
        public string SrmMin { get; set; }

        [JsonProperty("srmMax")]
        public string SrmMax { get; set; }

        [JsonProperty("ogMin")]
        public string OgMin { get; set; }

        [JsonProperty("fgMin")]
        public string FgMin { get; set; }

        [JsonProperty("fgMax")]
        public string FgMax { get; set; }

        [JsonProperty("createDate")]
        public System.DateTime CreateDate { get; set; }

        [JsonProperty("updateDate")]
        public System.DateTime UpdateDate { get; set; }
    }

    public partial class BreweryDbSearchResult
    {
        public static BreweryDbSearchResult FromJson(string json) => JsonConvert.DeserializeObject<BreweryDbSearchResult>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this BreweryDbSearchResult self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
