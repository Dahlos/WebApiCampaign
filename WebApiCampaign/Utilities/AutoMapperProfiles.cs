using AutoMapper;
using WebApiCampaign.DTOs;
using WebApiCampaign.Entities;

namespace WebApiCampaign.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CampaignCreateDTO, Campaign>();
            CreateMap<Campaign, CampaignDTO>();
        }
    }
}
