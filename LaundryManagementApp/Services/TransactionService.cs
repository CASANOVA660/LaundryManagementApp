using LaundryManagement.Infrastructure.Data;using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using LaundryManagement.API.Interfaces;
using LaundryManagement.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace LaundryManagement.API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly LaundryDbContext _context;

        public TransactionService(LaundryDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> AddTransaction(Transaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            // Validate cycle exists
            var cycle = await _context.Cycles.FindAsync(transaction.IdCycle);
            if (cycle == null)
                throw new InvalidOperationException($"Cycle with ID {transaction.IdCycle} not found");

            // Validate client exists
            var client = await _context.Clients.FindAsync(transaction.IdClient);
            if (client == null)
                throw new InvalidOperationException($"Client with ID {transaction.IdClient} not found");

            transaction.TransactionDate = DateTime.UtcNow;
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<Transaction> GetTransaction(int id)
        {
            return await _context.Transactions
                .Include(t => t.Client)
                .Include(t => t.Cycle)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByClientId(int clientId)
        {
            return await _context.Transactions
                .Include(t => t.Cycle)
                .Where(t => t.IdClient == clientId)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<bool> UpdateTransactionStatus(int id, string status)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
                return false;

            transaction.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
