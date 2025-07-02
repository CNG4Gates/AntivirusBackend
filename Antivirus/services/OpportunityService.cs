using Antivirus.DTOs;
using Antivirus.Models;
using Antivirus.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Antivirus.Services
{
    public class OpportunityService : IOpportunityService
    {
        private readonly AppDbContext _context;

        public OpportunityService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OpportunitiesReadDTO>> GetAllAsync()
        {
            return await _context.Opportunities
                .Select(o => new OpportunitiesReadDTO
                {
                    Id = o.Id,
                    Name = o.Name,
                    Description = o.Description,
                    ImageUrl = o.ImageUrl,
                    Status = o.Status
                })
                .ToListAsync();
        }

        public async Task<OpportunitiesReadDTO?> GetByIdAsync(long id)
        {
            var opportunity = await _context.Opportunities.FindAsync(id);
            if (opportunity == null) return null;

            return new OpportunitiesReadDTO
            {
                Id = opportunity.Id,
                Name = opportunity.Name,
                Description = opportunity.Description,
                ImageUrl = opportunity.ImageUrl,
                Status = opportunity.Status
            };
        }

        public async Task<OpportunitiesReadDTO> CreateAsync(OpportunitiesCreateDTO dto)
        {
            var opportunity = new Opportunity
            {
                Name = dto.Name,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                Status = true // O el valor por defecto que prefieras
            };

            _context.Opportunities.Add(opportunity);
            await _context.SaveChangesAsync();

            return new OpportunitiesReadDTO
            {
                Id = opportunity.Id,
                Name = opportunity.Name,
                Description = opportunity.Description,
                ImageUrl = opportunity.ImageUrl,
                Status = opportunity.Status
            };
        }

        public async Task<OpportunitiesReadDTO?> UpdateAsync(long id, OpportunitiesCreateDTO dto)
        {
            var opportunity = await _context.Opportunities.FindAsync(id);
            if (opportunity == null) return null;

            opportunity.Name = dto.Name;
            opportunity.Description = dto.Description;
            opportunity.ImageUrl = dto.ImageUrl;

            await _context.SaveChangesAsync();

            return new OpportunitiesReadDTO
            {
                Id = opportunity.Id,
                Name = opportunity.Name,
                Description = opportunity.Description,
                ImageUrl = opportunity.ImageUrl,
                Status = opportunity.Status
            };
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var opportunity = await _context.Opportunities.FindAsync(id);
            if (opportunity == null) return false;

            _context.Opportunities.Remove(opportunity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
