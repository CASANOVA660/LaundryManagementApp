using Microsoft.AspNetCore.Mvc;
using LaundryManagement.Infrastructure.Models;
using LaundryManagement.API.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LaundryManagement.API.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class CycleController : ControllerBase
    {
        private readonly ICycleService _cycleService;

        public CycleController(ICycleService cycleService)
        {
            _cycleService = cycleService;
        }

        public class CreateCycleRequest
        {
            public string Type { get; set; }
            public decimal Cost { get; set; }
            public int Duration { get; set; }
            public int IdMachine { get; set; }
        }

        // GET: api/cycles/machine/{machineId}
        [HttpGet("machine/{machineId}")]
        public async Task<ActionResult<IEnumerable<Cycle>>> GetCyclesByMachineId(int machineId)
        {
            try
            {
                var cycles = await _cycleService.GetCyclesByMachineId(machineId);
                if (cycles == null)
                {
                    return NotFound($"No cycles found for machine {machineId}");
                }
                return Ok(cycles);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/cycles
        [HttpPost]
        public async Task<ActionResult<Cycle>> AddCycle([FromBody] CreateCycleRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Cycle data is null");
                }

                if (string.IsNullOrEmpty(request.Type))
                {
                    return BadRequest("Cycle type is required");
                }

                if (request.IdMachine <= 0)
                {
                    return BadRequest("Valid machine ID is required");
                }

                var cycle = new Cycle
                {
                    Type = request.Type,
                    Cost = request.Cost,
                    Duration = request.Duration,
                    IdMachine = request.IdMachine
                };

                var addedCycle = await _cycleService.AddCycle(cycle);
                return CreatedAtAction(nameof(GetCyclesByMachineId), new { machineId = cycle.IdMachine }, addedCycle);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/cycles/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveCycle(int id)
        {
            try
            {
                await _cycleService.RemoveCycle(id);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}