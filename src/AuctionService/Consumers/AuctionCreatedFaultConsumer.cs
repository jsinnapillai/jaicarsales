using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using MassTransit;

namespace AuctionService
{
    public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
    {
        public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
        {
             Console.WriteLine("--> Coonsuming Faulty Creation ");

             var exception = context.Message.Exceptions.First();

             if (exception.ExceptionType == "System.ArgumentException")
             {
                context.Message.Message.Model = "FooBar1";
                await context.Publish(context.Message.Message);
             }
             else {
                Console.WriteLine("Not an argumet exception - update error dashboard somewhere");
             }
             
        }

    }
}