using System;
using BlobCogBob.Shared;
using BlobCogBob.Core.Services;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using BlobCogBob.Core.Shared;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

namespace BlobCogBob.Core
{
    public class BeerSearchService : HttpService
    {

        public async static Task<BeerInfo> FindBeer(string beerName)
        {
            // Build the url
            var builtUrl = new UriBuilder(BackendConstants.BreweryDbSearchUrl);
            var query = HttpUtility.ParseQueryString(builtUrl.Query);
            query["key"] = APIKeys.BreweryDbAPIKey;
            query["format"] = "json";
            query["t"] = "beer";
            query["q"] = beerName;

            builtUrl.Query = query.ToString();

            var beerNameNoWhiteSpace = beerName.StripWhitespaces().ToUpper();

            var theBeers = await GetDataObjectFromAPI<BreweryDbSearchResult, string>(builtUrl.ToString()).ConfigureAwait(false);

            var firstFound = theBeers.Data.FirstOrDefault(ff => ff.NameDisplay.StripWhitespaces().ToUpper() == beerNameNoWhiteSpace);

            if (firstFound != null)
            {
                var beerInfo = new BeerInfo
                {
                    ABV = firstFound.Abv,
                    Description = firstFound.Description,
                    Label = firstFound.Labels.Medium,
                    Style = firstFound.Style.Description,
                    Name = firstFound.NameDisplay
                };

                return beerInfo;
            }

            return null;
        }
    }

    public static class StringExtensions
    {
        public static string StripWhitespaces(this string input)
        {
            return Regex.Replace(input, @"\s+", "");
        }
    }
}
