using LaundryManagement.API.Interfaces;
using LaundryManagement.Infrastructure.Data;
using LaundryManagement.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaundryManagement.API.Services
{
    public class MachineService : IMachineService
    {
        private readonly LaundryDbContext _context;

        public MachineService(LaundryDbContext context)
        {
            _context = context;
        }

        public async Task<Machine> AddMachine(Machine machine)
        {
            try
            {
                // Verify the laundry exists
                var laundry = await _context.Laundries.FindAsync(machine.IdLaundry);
                if (laundry == null)
                {
                    throw new System.Exception($"Laundry with ID {machine.IdLaundry} not found");
                }

                // Set initial status if not provided
                if (string.IsNullOrEmpty(machine.Status))
                {
                    machine.Status = "Available";
                }

                _context.Machines.Add(machine);
                await _context.SaveChangesAsync();

                // Reload the machine to get any database-generated values
                await _context.Entry(machine).ReloadAsync();
                return machine;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception($"Error adding machine: {ex.Message}");
            }
        }

        public async Task<Machine> GetMachine(int id)
        {
            return await _context.Machines
                .Include(m => m.Cycles)
                .Include(m => m.CurrentCycle)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Machine>> GetMachines()
        {
            return await _context.Machines
                .Include(m => m.Cycles)
                .Include(m => m.CurrentCycle)
                .ToListAsync();
        }

        public async Task<IEnumerable<Machine>> GetMachinesByLaundryId(int laundryId)
        {
            return await _context.Machines
                .Include(m => m.Cycles)
                .Include(m => m.CurrentCycle)
                .Where(m => m.IdLaundry == laundryId)
                .ToListAsync();
        }

        public async Task UpdateMachine(Machine machine)
        {
            try
            {
                _context.Entry(machine).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                throw new System.Exception($"Error updating machine: {ex.Message}");
            }
        }

        public async Task RemoveMachine(int id)
        {
            var machine = await _context.Machines.FindAsync(id);
            if (machine == null)
            {
                throw new System.Exception($"Machine with ID {id} not found");
            }

            _context.Machines.Remove(machine);
            await _context.SaveChangesAsync();
        }

        public async Task<Cycle> GetCycle(int cycleId)
        {
            return await _context.Cycles.FindAsync(cycleId);
        }

        public async Task<Machine> StartCycle(int machineId, int cycleId)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                // First check if machine exists and is available without starting a transaction
                var machine = await _context.Machines
                    .FirstOrDefaultAsync(m => m.Id == machineId);

                if (machine == null)
                {
                    throw new Exception($"Machine with ID {machineId} not found");
                }

                if (machine.Status != "Available")
                {
                    throw new Exception($"Machine is not available. Current status: {machine.Status}");
                }

                var cycle = await _context.Cycles.FindAsync(cycleId);
                if (cycle == null)
                {
                    throw new Exception($"Cycle with ID {cycleId} not found");
                }

                // Now start a transaction for the update
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Double-check machine is still available
                        var isStillAvailable = await _context.Machines
                            .AnyAsync(m => m.Id == machineId && m.Status == "Available");

                        if (!isStillAvailable)
                        {
                            throw new Exception("Machine is no longer available");
                        }

                        // Update machine properties
                        machine.Status = "In Use";
                        machine.CurrentCycleId = cycleId;
                        machine.CurrentCycleStartTime = DateTime.Now;
                        machine.CurrentCycleEndTime = DateTime.Now.AddMinutes(cycle.Duration);

                        _context.Machines.Update(machine);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        // Return updated machine
                        return await _context.Machines
                            .Include(m => m.CurrentCycle)
                            .FirstOrDefaultAsync(m => m.Id == machineId);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception($"Failed to update machine status: {ex.Message}");
                    }
                }
            });
        }

        public async Task<Machine> StopCycle(int machineId)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                // First check if machine exists and is in use without starting a transaction
                var machine = await _context.Machines
                    .Include(m => m.CurrentCycle)
                    .FirstOrDefaultAsync(m => m.Id == machineId);

                if (machine == null)
                {
                    throw new Exception($"Machine with ID {machineId} not found");
                }

                if (machine.Status != "In Use")
                {
                    throw new Exception("Machine is not in use");
                }

                // Now start a transaction for the update
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Double-check machine is still in use
                        var isStillInUse = await _context.Machines
                            .AnyAsync(m => m.Id == machineId && m.Status == "In Use");

                        if (!isStillInUse)
                        {
                            throw new Exception("Machine is no longer in use");
                        }

                        // Update machine properties
                        machine.Status = "Available";
                        machine.CurrentCycleId = null;
                        machine.CurrentCycleStartTime = null;
                        machine.CurrentCycleEndTime = null;

                        _context.Machines.Update(machine);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        // Return updated machine
                        return await _context.Machines
                            .Include(m => m.CurrentCycle)
                            .FirstOrDefaultAsync(m => m.Id == machineId);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception($"Failed to update machine status: {ex.Message}");
                    }
                }
            });
        }

        public async Task<bool> IsCycleComplete(int machineId)
        {
            var machine = await _context.Machines
                .FirstOrDefaultAsync(m => m.Id == machineId);

            if (machine == null)
            {
                throw new Exception($"Machine with ID {machineId} not found");
            }

            if (machine.Status != "In Use" || !machine.CurrentCycleEndTime.HasValue)
            {
                return false;
            }

            return DateTime.Now >= machine.CurrentCycleEndTime.Value;
        }
    }
}
