using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.model;

namespace SearchService.Consumers
{
    public class BitPlacedConsumer : IConsumer<BitPlaced>
    {
        public async Task Consume(ConsumeContext<BitPlaced> context)
        {
           Console.WriteLine(" --- > Conuming Bit placed from Search Service");

           var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId);

           if( context.Message.BitStatus.Contains("Accepted")
           && context.Message.Amount > auction.CurrentHighBid
           )
           {
            auction.CurrentHighBid = context.Message.Amount;
            await auction.SaveAsync();
           }
        }
    }
}