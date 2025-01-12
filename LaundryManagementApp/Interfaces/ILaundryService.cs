using LaundryManagement.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaundryManagement.API.Interfaces
{
    public interface ILaundryService
    {
        Task<Laundry> GetLaundryByOwnerId(int ownerId);
        Task<Laundry> AddLaundry(Laundry laundry);
    }
}
