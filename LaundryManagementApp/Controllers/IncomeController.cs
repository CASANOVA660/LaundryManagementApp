using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LaundryManagement.API.Interfaces;

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

        // GET: api/Income/Daily
        [HttpGet("Daily")]
        public async Task<ActionResult<decimal>> GetDailyIncome()
        {
            return await _incomeService.GetDailyIncome();
        }

        // GET: api/Income/Monthly
        [HttpGet("Monthly")]
        public async Task<ActionResult<decimal>> GetMonthlyIncome()
        {
            return await _incomeService.GetMonthlyIncome();
        }

        // GET: api/Income/Yearly
        [HttpGet("Yearly")]
        public async Task<ActionResult<decimal>> GetYearlyIncome()
        {
            return await _incomeService.GetYearlyIncome();
        }
    }
}