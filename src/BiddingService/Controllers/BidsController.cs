using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BiddingService.DTOs;
using BiddingService.Models;
using BiddingService.Services;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace BiddingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly GrpcAuctionClient _grpcAuctionClient;

        public BidsController(IMapper mapper, IPublishEndpoint publishEndpoint, GrpcAuctionClient grpcAuctionClient)
        {
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _grpcAuctionClient = grpcAuctionClient;
        }

        [Authorize]
        [HttpPost]
         public async Task<ActionResult<BidDto>> PlaceBid(string auctionId, int amount)
        {
            var auction = await DB.Find<Auction>().OneAsync(auctionId);
            
            if(auction == null)
            {
                auction  = _grpcAuctionClient.GetAuction(auctionId);
                if(auction == null)
                {
                    return BadRequest("Cannot accept bids for this auction at all");
                }
            }
            if(auction.Seller == User.Identity.Name)
            {
                return BadRequest("You cannt bid on your own auction");
            }

            var bid = new Bid{
                Amount = amount,
                AuctionId = auctionId,
                Bidder = User.Identity.Name
            };

            if (auction.AuctionEnd < DateTime.UtcNow )
            {
                bid.BidStatus = BidStatus.Finished;
            }
            else
            {
                    var highbid = await DB.Find<Bid>()   
                    .Match(a => a.AuctionId == auctionId)
                    .Sort(b => b.Descending(x => x.Amount))
                    .ExecuteFirstAsync();

                if(highbid !=null && amount > highbid.Amount || highbid == null) 
                {
                    bid.BidStatus = amount > auction.ReservePrice 
                        ? BidStatus.Accepted : BidStatus.AcceptedBelowReserve;
                }

                if (highbid != null && bid.Amount <=highbid.Amount )
                {
                    bid.BidStatus = BidStatus.TooLow;
                }
           }

            await DB.SaveAsync(bid);
            
            var bidplaced = _mapper.Map<BitPlaced>(bid);
            Console.WriteLine("Bid placed - bitplaced.BidStatus" + bidplaced.BidStatus);

            await _publishEndpoint.Publish(bidplaced);

            // await _publishEndPoint.Publish(_mapper.Map<BidPlaced>(bid));

            return Ok(_mapper.Map<BidDto>(bid));

        }
  

    [HttpGet("{auctionId}")]
    public async Task<ActionResult<List<BidDto>>> GetBidAuction(string auctionId)
    {
        var bids = await DB.Find<Bid>()
        .Match(a => a.AuctionId == auctionId)
        .Sort( b => b.Descending(a => a.BidTime))
        .ExecuteAsync();

        return bids.Select(_mapper.Map<BidDto>).ToList();

    }
}
}