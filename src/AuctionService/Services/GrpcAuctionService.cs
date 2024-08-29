using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Data;
using Grpc.Core;

namespace AuctionService.Services
{
    public class GrpcAuctionService : GrpcAuction.GrpcAuctionBase
    {
        private readonly AuctionDBContext _dBContext;
        public GrpcAuctionService(AuctionDBContext dBContext)
        {
            _dBContext = dBContext;
            
        }
        
        public override async Task<GrpcAuctionResponse> GetAuction(GetAuctionRequest request, ServerCallContext context)
        {
            Console.WriteLine(" ==> Received GRPC request for auection");

            var auction = await _dBContext.Auctions.FindAsync(Guid.Parse(request.Id));

            if (auction == null) 
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Not Found"));
               
            }

            var response = new GrpcAuctionResponse
            {
                Auction   = new GrpcAuctionModel
                {
                    AuctionEnd = auction.AuctionEnd.ToString(),
                    Id = auction.Id.ToString(),
                    ReservePrice = auction.ReservePrice,
                    Seller = auction.Seller
                }
            };
            return response;
        }
        
    }
}