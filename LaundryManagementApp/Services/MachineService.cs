using LaundryManagement.Infrastructure.Models;
using LaundryManagement.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using LaundryManagement.Infrastructure.Data;

namespace LaundryManagement.API.Services
{
    public class MachineService : IMachineService
    {
        private readonly LaundryDbContext _context;

        public MachineService(LaundryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Machine>> GetMachines()
        {
            return await _context.Machines.Include(m => m.Cycles).ToListAsync();
        }

        public async Task<Machine> GetMachine(int id)
        {
            return await _context.Machines.Include(m => m.Cycles).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Machine> AddMachine(Machine machine)
        {
            _context.Machines.Add(machine);
            await _context.SaveChangesAsync();
            return machine;
        }

        public async Task RemoveMachine(int id)
        {
            var machine = await _context.Machines.FindAsync(id);
            if (machine != null)
            {
                _context.Machines.Remove(machine);
                await _context.SaveChangesAsync();
            }
        }
    }
}