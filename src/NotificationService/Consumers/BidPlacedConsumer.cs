using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService
{
    public class BidPlacedConsumer : IConsumer<BitPlaced>
{
                private readonly IHubContext<NotificationHub> _hubContext;

        public BidPlacedConsumer(IHubContext<NotificationHub> hubContext)
         {
            _hubContext = hubContext;
          }
    public async Task Consume(ConsumeContext<BitPlaced> context)
    {
        Console.WriteLine("==> auction finished message received");

         await _hubContext.Clients.All.SendAsync("BidPlaced", context.Message);
    }
}
}