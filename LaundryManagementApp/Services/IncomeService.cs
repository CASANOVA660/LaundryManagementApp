using LaundryManagement.API.Interfaces;
using LaundryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LaundryManagement.API.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly LaundryDbContext _context;

        public IncomeService(LaundryDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetDailyIncome()
        {
            var today = DateTime.UtcNow.Date;
            return await _context.Transactions
                .Where(t => t.TransactionDate >= today)
                .SumAsync(t => t.Amount);
        }

        public async Task<decimal> GetMonthlyIncome()
        {
            var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            return await _context.Transactions
                .Where(t => t.TransactionDate >= startOfMonth)
                .SumAsync(t => t.Amount);
        }

        public async Task<decimal> GetYearlyIncome()
        {
            var startOfYear = new DateTime(DateTime.UtcNow.Year, 1, 1);
            return await _context.Transactions
                .Where(t => t.TransactionDate >= startOfYear)
                .SumAsync(t => t.Amount);
        }
    }
}