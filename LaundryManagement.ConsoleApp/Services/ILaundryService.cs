using LaundryManagement.ConsoleApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaundryManagement.ConsoleApp.Services
{
    public interface ILaundryService
    {
        Task<IEnumerable<Laundry>> GetLaundries();
        Task<IEnumerable<Machine>> GetMachinesByLaundryId(int laundryId);
        Task<IEnumerable<Cycle>> GetAvailableCycles();
        Task<bool> StartMachine(int machineId, int cycleId);
        Task<bool> StopMachine(int machineId);
        Task<bool> IsCycleComplete(int machineId);
    }
}
