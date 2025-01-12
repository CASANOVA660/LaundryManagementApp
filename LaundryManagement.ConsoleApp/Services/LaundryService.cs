using LaundryManagement.ConsoleApp.Data;
using LaundryManagement.ConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LaundryManagement.ConsoleApp.Services
{
    public class LaundryService
    {
        private readonly LaundryDbContext _context;

        public LaundryService(LaundryDbContext context)
        {
            _context = context;
        }

        public async Task StartMachine(int machineId, int cycleId)
        {
            var machine = await _context.Machines
                .Include(m => m.CurrentCycle)
                .FirstOrDefaultAsync(m => m.Id == machineId);

            if (machine == null)
                throw new Exception("Machine not found");

            if (machine.Status != "Available")
                throw new Exception("Machine is not available");

            var cycle = await _context.Cycles.FindAsync(cycleId);
            if (cycle == null)
                throw new Exception("Cycle not found");

            machine.Status = "Running";
            machine.CurrentCycleId = cycleId;
            machine.CycleStartTime = DateTime.Now;

            await _context.SaveChangesAsync();

            // Start a background task to monitor cycle completion
            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromMinutes(cycle.DurationMinutes));
                await StopMachine(machineId);
            });
        }

        public async Task StopMachine(int machineId)
        {
            var machine = await _context.Machines.FindAsync(machineId);
            if (machine == null)
                throw new Exception("Machine not found");

            machine.Status = "Available";
            machine.CurrentCycleId = null;
            machine.CycleStartTime = null;

            await _context.SaveChangesAsync();
        }
    }
}