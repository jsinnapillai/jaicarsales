using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers
{
    [ApiController]
    [Route("api/auctions")]
    public class AuctionsController : ControllerBase
    {
  
        private readonly AuctionDBContext _context;
        private readonly IMapper _mapper;

        public AuctionsController(AuctionDBContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
     
        }

        [HttpGet]
        public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
        {
            var auctions = await _context.Auctions
                .Include(x => x.Item)
                .OrderBy(x => x.Item.Make)
                .ToListAsync();

            return _mapper.Map<List<AuctionDto>>(auctions);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
        {
            var auction = await _context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);

            if(auction == null)
            return NotFound();

            return _mapper.Map<AuctionDto>(auction);

        }

        [HttpPost]
        public async Task<ActionResult<AuctionDto>> CreateAction(CreateAuctionDto auctionDto )
        {
            var action = _mapper.Map<Auction>(auctionDto);
            // TODO : add current user as seller

            action.Seller = "test";

            _context.Auctions.Add(action);

            var result = await _context.SaveChangesAsync() >0;

            if(!result) return BadRequest("Could not save changes to the DB");

            return CreatedAtAction(nameof(GetAuctionById), new { action.Id },_mapper.Map<AuctionDto>(action) );


        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
        {
            var auction = await _context.Auctions
                            .Include(x => x.Item)
                            .FirstOrDefaultAsync(x => x.Id == id);

            if (auction == null) return NotFound();

            // Todo : check seller

            auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
            auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
            auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
            auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
            auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

            var result = await _context.SaveChangesAsync() >0;

            if (result) return Ok();

            return BadRequest("Problem saving Changes");

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuction(Guid id)
        {
             var auction = await _context.Auctions.FindAsync(id);
             if(auction == null) return NotFound();

             // TODO : check seller matches 

             var auctionDto =   _context.Auctions.Remove(auction);
            
            var result = await _context.SaveChangesAsync() >0;

            if (!result) return BadRequest("Problem in deleting the auctions");

            return Ok();

        }
        
    }
}