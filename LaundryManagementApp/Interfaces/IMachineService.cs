using LaundryManagement.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaundryManagement.API.Interfaces
{
    public interface IMachineService
    {
        Task<IEnumerable<Machine>> GetMachines();
        Task<Machine> GetMachine(int id);
        Task<Machine> AddMachine(Machine machine);
        Task RemoveMachine(int id);
        Task<IEnumerable<Machine>> GetMachinesByLaundryId(int laundryId);
        Task UpdateMachine(Machine machine);
        Task<Machine> StartCycle(int machineId, int cycleId);
        Task<Machine> StopCycle(int machineId);
        Task<bool> IsCycleComplete(int machineId);
        Task<Cycle> GetCycle(int cycleId);
    }
}