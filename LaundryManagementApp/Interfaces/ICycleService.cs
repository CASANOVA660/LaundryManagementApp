using System.Collections.Generic;
using System.Threading.Tasks;
using LaundryManagement.Infrastructure.Models;

namespace LaundryManagement.API.Interfaces
{
    public interface ICycleService
    {
        Task<IEnumerable<Cycle>> GetCyclesByMachineId(int machineId);
        Task<Cycle> GetCycle(int id);
        Task<Cycle> AddCycle(Cycle cycle);
        Task RemoveCycle(int id);
    }
}