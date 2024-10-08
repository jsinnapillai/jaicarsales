using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Entities;
using SearchService.model;

namespace SearchService.services
{
    public class AuctionServiceHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
 
        public AuctionServiceHttpClient(HttpClient httpClient,IConfiguration config)
        {
            _config = config;
   
            _httpClient = httpClient;
            
        }

        public async Task<List<Item>> GetItemForSearchDB()
        {
            var lastupdated = await DB.Find<Item,string>()
            .Sort(x => x.Ascending(x => x.UpdatedAt))
            .Project(x => x.UpdatedAt.ToString())
            .ExecuteFirstAsync();

            Console.WriteLine("Last Date is : " + lastupdated);

            // return await _httpClient.GetFromJsonAsync<List<Item>>(_config["AuctionServiceUrl"]+"/api/auctions?date=" + lastupdated);
            return await _httpClient.GetFromJsonAsync<List<Item>>(_config["AuctionServiceUrl"]+"/api/auctions" );
        }
    }
}