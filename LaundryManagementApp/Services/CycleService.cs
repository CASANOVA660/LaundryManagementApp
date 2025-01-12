using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LaundryManagement.Infrastructure.Data;
using LaundryManagement.Infrastructure.Models;
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

        public async Task<IEnumerable<Cycle>> GetCyclesByMachineId(int machineId)
        {
            return await _context.Cycles
                .Where(c => c.IdMachine == machineId)
                .ToListAsync();
        }

        public async Task<Cycle> AddCycle(Cycle cycle)
        {
            if (cycle == null)
                throw new ArgumentNullException(nameof(cycle));

            // Verify that the machine exists
            var machine = await _context.Machines.FindAsync(cycle.IdMachine);
            if (machine == null)
                throw new InvalidOperationException($"Machine with ID {cycle.IdMachine} not found");

            // Add the cycle
            _context.Cycles.Add(cycle);
            await _context.SaveChangesAsync();

            // Reload the cycle to get any database-generated values
            await _context.Entry(cycle).ReloadAsync();

            return cycle;
        }

        public async Task RemoveCycle(int id)
        {
            var cycle = await _context.Cycles.FindAsync(id);
            if (cycle == null)
                throw new InvalidOperationException($"Cycle with ID {id} not found");

            _context.Cycles.Remove(cycle);
            await _context.SaveChangesAsync();
        }

        public async Task<Cycle> GetCycle(int id)
        {
            return await _context.Cycles.FindAsync(id);
        }
    }
}