using Microsoft.AspNetCore.Mvc;
using LaundryManagement.Infrastructure.Models;
using LaundryManagement.API.Interfaces;
using System.Threading.Tasks;

namespace LaundryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaundryController : ControllerBase
    {
        private readonly ILaundryService _laundryService;

        public LaundryController(ILaundryService laundryService)
        {
            _laundryService = laundryService;
        }

        // GET: api/laundry/owner/{ownerId}
        [HttpGet("owner/{ownerId}")]
        public async Task<ActionResult<Laundry>> GetLaundryByOwnerId(int ownerId)
        {
            try
            {
                var laundry = await _laundryService.GetLaundryByOwnerId(ownerId);
                if (laundry == null)
                {
                    return NotFound($"No laundry found for owner ID {ownerId}");
                }
                return Ok(laundry);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/laundry
        [HttpPost]
        public async Task<ActionResult<Laundry>> AddLaundry([FromBody] Laundry laundry)
        {
            try
            {
                var addedLaundry = await _laundryService.AddLaundry(laundry);
                return CreatedAtAction(nameof(GetLaundryByOwnerId), new { ownerId = addedLaundry.IdProprietaire }, addedLaundry);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
