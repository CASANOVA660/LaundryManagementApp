using System.Collections.Generic;
using System.Threading.Tasks;
using LaundryManagement.Infrastructure.Models;

namespace LaundryManagement.API.Interfaces
{
    public interface IClientService
    {
        Task<Client> GetClient(int id);
        Task<IEnumerable<Client>> GetAllClients();
        Task<Client> AddClient(Client client);
        Task<bool> UpdateClient(Client client);
        Task<bool> DeleteClient(int id);
    }
}
