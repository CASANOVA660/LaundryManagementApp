using LaundryManagement.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using LaundryManagement.Infrastructure.Data;
using LaundryManagement.API.Interfaces;

namespace LaundryManagement.API.Services
{
    public class CycleService : ICycleService
    {
        private readonly LaundryDbContext _context;

        public CycleService(LaundryDbContext context)
        {
            _context = context;
        }

        public async Task UpdateCyclePrice(int id, decimal price)
        {
            var cycle = await _context.Cycles.FindAsync(id);
            if (cycle != null)
            {
                cycle.Cost = price;
                await _context.SaveChangesAsync();
            }
        }
    }
}