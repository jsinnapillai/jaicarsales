using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.model;
using SearchService.services;

namespace SearchService.Data
{
    public class DbInitializer
    {
        public static async Task InitDb(WebApplication app)
        {
            await DB.InitAsync("SearchDb",MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("MongoDBConnect")));

            await DB.Index<Item>()
                .Key(x => x.Make, KeyType.Text)
                .Key(x => x.Model, KeyType.Text)
                .Key(x => x.Color, KeyType.Text)
                .CreateAsync();

            var count = await DB.CountAsync<Item>();
            // if(count == 0 )
            // {
            //     Console.WriteLine("No Data - will attempt to seed");
            //     var itemData = await File.ReadAllTextAsync("Data/auctions.json");

            //     var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

            //     var items = JsonSerializer.Deserialize<List<Item>>(itemData,options);

            //     await DB.SaveAsync(items);
            // }


            using var scope = app.Services.CreateScope();
            var httpClient = scope.ServiceProvider.GetRequiredService<AuctionServiceHttpClient>();

            var items = await httpClient.GetItemForSearchDB();

            Console.WriteLine(items.Count + "  returned from the auction service");

            if(items.Count >0 )
            {
                await DB.SaveAsync(items);
            }
            
        }
    }
}