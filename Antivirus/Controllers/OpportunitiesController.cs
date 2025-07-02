using Antivirus.DTOs;
using Antivirus.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Antivirus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OpportunitiesController : ControllerBase
    {
        private readonly IOpportunityService _opportunityService;

        public OpportunitiesController(IOpportunityService opportunityService)
        {
            _opportunityService = opportunityService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var opportunities = await _opportunityService.GetAllAsync();
            return Ok(opportunities);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(long id)
        {
            var opportunity = await _opportunityService.GetByIdAsync(id);
            if (opportunity == null)
                return NotFound(new { message = "Oportunidad no encontrada." });

            return Ok(opportunity);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OpportunitiesCreateDTO dto)
        {
            var opportunity = await _opportunityService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = opportunity.Id }, opportunity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] OpportunitiesCreateDTO dto)
        {
            var opportunity = await _opportunityService.UpdateAsync(id, dto);
            if (opportunity == null)
                return NotFound(new { message = "Oportunidad no encontrada." });

            return Ok(opportunity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _opportunityService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = "Oportunidad no encontrada." });

            return NoContent();
        }
    }
}
