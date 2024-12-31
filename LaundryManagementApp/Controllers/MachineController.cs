using Microsoft.AspNetCore.Mvc;
using LaundryManagement.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using LaundryManagement.API.Interfaces;

namespace LaundryManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineController : ControllerBase
    {
        private readonly IMachineService _machineService;

        public MachineController(IMachineService machineService)
        {
            _machineService = machineService;
        }

        // GET: api/Machine
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Machine>>> GetMachines()
        {
            var machines = await _machineService.GetMachines();
            return Ok(machines);
        }

        // GET: api/Machine/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Machine>> GetMachine(int id)
        {
            var machine = await _machineService.GetMachine(id);
            if (machine == null)
            {
                return NotFound();
            }
            return machine;
        }

        // POST: api/Machine
        [HttpPost]
        public async Task<ActionResult<Machine>> AddMachine([FromBody] Machine machine)
        {
            var addedMachine = await _machineService.AddMachine(machine);
            return CreatedAtAction(nameof(GetMachine), new { id = addedMachine.Id }, addedMachine);
        }

        // DELETE: api/Machine/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveMachine(int id)
        {
            await _machineService.RemoveMachine(id);
            return NoContent();
        }
    }
}