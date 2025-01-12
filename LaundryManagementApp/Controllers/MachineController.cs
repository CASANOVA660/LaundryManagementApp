using LaundryManagement.API.Interfaces;
using LaundryManagement.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaundryManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class MachineController : ControllerBase
    {
        private readonly IMachineService _machineService;

        public MachineController(IMachineService machineService)
        {
            _machineService = machineService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Machine>>> GetMachines()
        {
            var machines = await _machineService.GetMachines();
            return Ok(machines);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Machine>> GetMachine(int id)
        {
            var machine = await _machineService.GetMachine(id);
            if (machine == null)
            {
                return NotFound();
            }
            return Ok(machine);
        }

        public class CreateMachineRequest
        {
            public string Type { get; set; }
            public string Status { get; set; }
            public int IdLaundry { get; set; }
        }

        // POST: api/machines
        [HttpPost]
        public async Task<ActionResult<Machine>> AddMachine([FromBody] CreateMachineRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Machine data is null");
                }

                if (string.IsNullOrEmpty(request.Type))
                {
                    return BadRequest("Machine type is required");
                }

                if (request.IdLaundry <= 0)
                {
                    return BadRequest("Valid laundry ID is required");
                }

                var machine = new Machine
                {
                    Type = request.Type,
                    Status = request.Status ?? "Available",
                    IdLaundry = request.IdLaundry
                };

                var addedMachine = await _machineService.AddMachine(machine);
                return CreatedAtAction(nameof(GetMachine), new { id = addedMachine.Id }, addedMachine);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{id}/start")]
        public async Task<ActionResult<Machine>> StartCycle(int id, [FromBody] int cycleId)
        {
            try
            {
                var machine = await _machineService.StartCycle(id, cycleId);
                return Ok(machine);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/stop")]
        public async Task<ActionResult<Machine>> StopCycle(int id)
        {
            try
            {
                var machine = await _machineService.StopCycle(id);
                return Ok(machine);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/status")]
        public async Task<ActionResult<bool>> IsCycleComplete(int id)
        {
            try
            {
                var isComplete = await _machineService.IsCycleComplete(id);
                return Ok(isComplete);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMachine(int id)
        {
            try
            {
                await _machineService.RemoveMachine(id);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}