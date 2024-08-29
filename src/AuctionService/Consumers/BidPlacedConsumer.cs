using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Data;
using Contracts;
using MassTransit;

namespace AuctionService
{
    public class BidPlacedConsumer : IConsumer<BitPlaced>
    {
        private readonly AuctionDBContext _dBContext;
        public BidPlacedConsumer(AuctionDBContext dBContext)
        {
            _dBContext = dBContext;
            
        }
        public async Task Consume(ConsumeContext<BitPlaced> context)
        {
            Console.WriteLine("---> Consuming Bit placed");
            var auction = await _dBContext.Auctions.FindAsync(Guid.Parse(context.Message.AuctionId));

           Console.WriteLine("---> Consuming Bit placed rom AuctionService ontext.Message.BitStatus",context.Message.BidStatus);

            if(auction.CurrentHighBid == null || context.Message.BidStatus.Contains("Accepted")
            && context.Message.Amount > auction.CurrentHighBid
            )
            {
                auction.CurrentHighBid = context.Message.Amount;    
                await _dBContext.SaveChangesAsync();
            }
        }
    }
}