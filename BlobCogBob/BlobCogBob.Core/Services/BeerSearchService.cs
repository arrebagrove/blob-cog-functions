using System;
using BlobCogBob.Shared;
using BlobCogBob.Core.Services;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using BlobCogBob.Core.Shared;
using System.Threading.Tasks;

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


            var theBeers = await GetDataObjectFromAPI<List<BreweryDbBeerInfo>, string>(builtUrl.ToString()).ConfigureAwait(false);

            return null;
        }
    }
}
