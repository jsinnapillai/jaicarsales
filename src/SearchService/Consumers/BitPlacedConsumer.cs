using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.model;

namespace SearchService
{
    public class BitPlacedConsumer : IConsumer<BitPlaced>
    {
        public async Task Consume(ConsumeContext<BitPlaced> context)
        {
           Console.WriteLine(" --- > Conuming Bit placed from Search Service");

           

           var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId);

            Console.WriteLine(" --- > Conuming Bid placed from Search Service auction.CurrentHighBid"+ auction.CurrentHighBid);
            Console.WriteLine(" --- > Conuming Bid placed from Search Service context.Message.Amount"+ context.Message.Amount);
            Console.WriteLine(" --- > Conuming Bid placed from Search Service  context.Message.BidStatus"+  context.Message.BidStatus);



            if (auction.CurrentHighBid == null 
            || context.Message.BidStatus.Contains("Accepted")
           && context.Message.Amount > auction.CurrentHighBid
           )
           {
            auction.CurrentHighBid = context.Message.Amount;
            await auction.SaveAsync();
           }
        }
    }
}