using AutoMapper;
using Contracts;
using SearchService.model;

namespace SearchService.RequestHelpers
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles()
        {
            CreateMap<AuctionCreated,Item>();
            CreateMap<AuctionUpdated,Item>();

        }
    }
}