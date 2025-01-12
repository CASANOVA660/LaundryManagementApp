using System.Collections.Generic;
using System.Threading.Tasks;
using LaundryManagement.Infrastructure.Models;

namespace LaundryManagement.API.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> AddTransaction(Transaction transaction);
        Task<Transaction> GetTransaction(int id);
        Task<IEnumerable<Transaction>> GetTransactionsByClientId(int clientId);
        Task<bool> UpdateTransactionStatus(int id, string status);
    }
}
