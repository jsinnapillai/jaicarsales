using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.model;

namespace SearchService
{
    public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
    {
        private readonly IMapper _mapper;
        public  AuctionCreatedConsumer(IMapper mapper)
        {
            _mapper = mapper;
            
        }
        public async Task Consume(ConsumeContext<AuctionCreated> context)
        {
           Console.WriteLine(" --> Consuming auction created:" + context.Message.Id);

           var item = _mapper.Map<Item>(context.Message);

            if (item.Model == "Foo")
            {
                throw new ArgumentException("Cannt sell cars with name of Foo");
            }

           await item.SaveAsync();
        }
    }
}