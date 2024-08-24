using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.model;

namespace SearchService 
{
    public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
    { 

        public async Task Consume(ConsumeContext<AuctionDeleted> context)
        {
           Console.WriteLine("---> Consuming Auction Deleted" + context.Message.Id);

           var result = await DB.DeleteAsync<Item>(context.Message.Id);

           if(!result.IsAcknowledged)
            {
                throw new MessageException(typeof(AuctionDeleted), "Problem in deleting mongoDb");
            }
        }
    }
}