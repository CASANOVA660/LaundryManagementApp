using System.Threading.Tasks;

namespace LaundryManagement.API.Interfaces
{
    public interface ICycleService
    {
        Task UpdateCyclePrice(int id, decimal price);
    }
}