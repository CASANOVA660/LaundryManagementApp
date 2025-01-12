using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LaundryManagement.API.Interfaces;
using LaundryManagement.Infrastructure.Models;

namespace LaundryManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService _incomeService;

        public IncomeController(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }

        // GET: api/Income
        [HttpGet]
        public async Task<ActionResult<Income>> GetIncome()
        {
            var income = new Income
            {
                Daily = await _incomeService.GetDailyIncome(),
                Monthly = await _incomeService.GetMonthlyIncome(),
                Yearly = await _incomeService.GetYearlyIncome()
            };
            return income;
        }
    }
}