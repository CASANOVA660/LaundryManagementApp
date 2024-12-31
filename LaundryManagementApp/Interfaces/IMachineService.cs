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
    }
}