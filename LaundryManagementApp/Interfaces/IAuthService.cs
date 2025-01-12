using LaundryManagement.Infrastructure.Models;
using System.Threading.Tasks;

namespace LaundryManagement.API.Interfaces
{
    public interface IAuthService
    {
        Task<Owner> GetOwner(int id);
        Task<Client> RegisterClient(Client client);
        Task<Owner> RegisterOwner(Owner owner);
        Task<Client> LoginClient(string email, string password);
        Task<Owner> LoginOwner(string email, string password);
    }
}
