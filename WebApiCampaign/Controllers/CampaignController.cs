using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCampaign.DTOs;
using WebApiCampaign.Entities;

namespace WebApiCampaign.Controllers
{

    [ApiController]
    [Route("api/campaigns")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CampaignController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CampaignController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper  = mapper;
        }

        [HttpGet]
        //[ResponseCache(Duration = 2)]
        public async Task<ActionResult<List<CampaignDTO>>> Get()
        {
            var campaigns = await context.Campaigns.ToListAsync();
            return mapper.Map<List<CampaignDTO>>(campaigns);
        }
         

        [HttpGet("{id:int}", Name ="GetCampaign")]
        public async Task<ActionResult<CampaignDTO>> Get(int id)
        {
            var campaign = await context.Campaigns.FirstOrDefaultAsync(x => x.Id == id);

            if (campaign == null)
            {
                return NotFound();
            }

            return mapper.Map<CampaignDTO>(campaign);
        }


        [HttpPost]
        public async Task<ActionResult> Post(CampaignCreateDTO campaignCreateDTO)
        {

            var existNameCampaign = await context.Campaigns.AnyAsync(c => c.Name.ToUpper().Equals(campaignCreateDTO.Name.ToUpper()) );

            if (existNameCampaign)
            {
                return BadRequest($"Campaign with name {campaignCreateDTO.Name} already exists");
            }

            var existCodeCampaign = await context.Campaigns.AnyAsync(c => c.Code.ToUpper().Equals(campaignCreateDTO.Code.ToUpper()) );

            if (existCodeCampaign)
            {
                return BadRequest($"Campaign with code {campaignCreateDTO.Code} already exists");
            }


            var campaign = mapper.Map<Campaign>(campaignCreateDTO);
            context.Add(campaign);
            await context.SaveChangesAsync();
            
            var newCampaignDTO = mapper.Map<CampaignDTO>(campaign);

            return CreatedAtRoute("GetCampaign", new {id = campaign.Id}, newCampaignDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(CampaignCreateDTO campaignCreateDTO, int id)
        {

            bool existe = await context.Campaigns.AnyAsync(campaign => campaign.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            var existNameCampaign = await context.Campaigns.AnyAsync(c => c.Name.ToUpper().Equals(campaignCreateDTO.Name.ToUpper()) && c.Id != id);

            if (existNameCampaign)
            {
                return BadRequest($"Campaign with name {campaignCreateDTO.Name} already exists");
            }

            var existCodeCampaign = await context.Campaigns.AnyAsync(c => c.Code.ToUpper().Equals(campaignCreateDTO.Code.ToUpper()) && c.Id != id);

            if (existCodeCampaign)
            {
                return BadRequest($"Campaign with code {campaignCreateDTO.Code} already exists");
            }

            var campaign = mapper.Map<Campaign>(campaignCreateDTO);
            campaign.Id = id;

            context.Update(campaign);
            await context.SaveChangesAsync();


            var newCampaignDTO = mapper.Map<CampaignDTO>(campaign);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool existe = await context.Campaigns.AnyAsync(campaign => campaign.Id == id);
            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Campaign() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }

}