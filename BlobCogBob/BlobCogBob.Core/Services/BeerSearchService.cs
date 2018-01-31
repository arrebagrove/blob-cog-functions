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


            var theBeers = await GetDataObjectFromAPI<BreweryDbSearchResult, string>(builtUrl.ToString()).ConfigureAwait(false);

            var firstFound = theBeers.Data.FirstOrDefault(ff => Regex.Replace(ff.NameDisplay, @"\s+","") == Regex.Replace(beerName, @"\s+",""));
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
    }
}
