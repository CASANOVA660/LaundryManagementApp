using System.Threading.Tasks;

namespace LaundryManagement.API.Interfaces
{
    public interface IIncomeService
    {
        Task<decimal> GetDailyIncome();
        Task<decimal> GetMonthlyIncome();
        Task<decimal> GetYearlyIncome();
    }
}