using Microsoft.AspNetCore.Mvc;
using LaundryManagement.Infrastructure.Models;
using LaundryManagement.API.Interfaces;
using System.Threading.Tasks;

namespace LaundryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // GET: api/auth/owner/5
        [HttpGet("owner/{id}")]
        public async Task<ActionResult<Owner>> GetOwner(int id)
        {
            var owner = await _authService.GetOwner(id);
            if (owner == null)
            {
                return NotFound();
            }
            return owner;
        }

        // POST: api/auth/register/client
        [HttpPost("register/client")]
        public async Task<ActionResult<Client>> RegisterClient([FromBody] Client client)
        {
            var registeredClient = await _authService.RegisterClient(client);
            return CreatedAtAction(nameof(RegisterClient), new { id = registeredClient.Id }, registeredClient);
        }

        // POST: api/auth/register/owner
        [HttpPost("register/owner")]
        public async Task<ActionResult<Owner>> RegisterOwner([FromBody] Owner owner)
        {
            var registeredOwner = await _authService.RegisterOwner(owner);
            return CreatedAtAction(nameof(RegisterOwner), new { id = registeredOwner.Id }, registeredOwner);
        }

        // POST: api/auth/login/client
        [HttpPost("login/client")]
        public async Task<ActionResult<Client>> LoginClient([FromBody] LoginModel model)
        {
            var client = await _authService.LoginClient(model.Email, model.Password);
            if (client == null)
            {
                return NotFound("Invalid email or password");
            }
            return client;
        }

        // POST: api/auth/login/owner
        [HttpPost("login/owner")]
        public async Task<ActionResult<Owner>> LoginOwner([FromBody] LoginModel model)
        {
            var owner = await _authService.LoginOwner(model.Email, model.Password);
            if (owner == null)
            {
                return NotFound("Invalid email or password");
            }
            return owner;
        }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
