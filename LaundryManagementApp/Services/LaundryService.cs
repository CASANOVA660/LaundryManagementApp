using LaundryManagement.API.Interfaces;
using LaundryManagement.Infrastructure.Data;
using LaundryManagement.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LaundryManagement.API.Services
{
    public class LaundryService : ILaundryService
    {
        private readonly LaundryDbContext _context;

        public LaundryService(LaundryDbContext context)
        {
            _context = context;
        }

        public async Task<Laundry> GetLaundryByOwnerId(int ownerId)
        {
            return await _context.Laundries
                .Include(l => l.Machines)
                .FirstOrDefaultAsync(l => l.IdProprietaire == ownerId);
        }

        public async Task<Laundry> AddLaundry(Laundry laundry)
        {
            // Verify owner exists
            var owner = await _context.Owners.FindAsync(laundry.IdProprietaire);
            if (owner == null)
            {
                throw new System.Exception($"Owner with ID {laundry.IdProprietaire} not found");
            }

            // Check if owner already has a laundry
            var existingLaundry = await GetLaundryByOwnerId(laundry.IdProprietaire);
            if (existingLaundry != null)
            {
                throw new System.Exception($"Owner with ID {laundry.IdProprietaire} already has a laundry");
            }

            _context.Laundries.Add(laundry);
            await _context.SaveChangesAsync();
            return laundry;
        }
    }
}
