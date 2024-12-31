using Microsoft.AspNetCore.Mvc;
using LaundryManagement.API.Interfaces;
using System.Threading.Tasks;

namespace LaundryManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CycleController : ControllerBase
    {
        private readonly ICycleService _cycleService;

        public CycleController(ICycleService cycleService)
        {
            _cycleService = cycleService;
        }

        // PUT: api/Cycle/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCyclePrice(int id, [FromBody] decimal price)
        {
            await _cycleService.UpdateCyclePrice(id, price);
            return NoContent();
        }
    }
}