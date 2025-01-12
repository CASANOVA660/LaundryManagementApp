using Microsoft.AspNetCore.Mvc;
using LaundryManagement.Infrastructure.Models;
using LaundryManagement.API.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LaundryManagement.API.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMachineService _machineService;

        public TransactionController(ITransactionService transactionService, IMachineService machineService)
        {
            _transactionService = transactionService;
            _machineService = machineService;
        }

        public class CreateTransactionRequest
        {
            public int IdClient { get; set; }
            public int IdCycle { get; set; }
            public decimal Amount { get; set; }
            public string Status { get; set; }
        }

        // POST: api/transactions
        [HttpPost]
        public async Task<ActionResult<Transaction>> CreateTransaction([FromBody] CreateTransactionRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Transaction data is null");
                }

                // Get the cycle to find its machine ID
                var cycle = await _machineService.GetCycle(request.IdCycle);
                if (cycle == null)
                {
                    return BadRequest($"Cycle with ID {request.IdCycle} not found");
                }

                // First try to start the machine cycle
                try
                {
                    var machine = await _machineService.StartCycle(cycle.IdMachine, request.IdCycle);
                    if (machine == null)
                    {
                        return BadRequest("Failed to start machine cycle");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"Failed to start machine: {ex.Message}");
                }

                // If machine started successfully, create the transaction
                var transaction = new Transaction
                {
                    IdClient = request.IdClient,
                    IdCycle = request.IdCycle,
                    Amount = request.Amount,
                    Status = request.Status ?? "Pending",
                    TransactionDate = System.DateTime.UtcNow
                };

                var createdTransaction = await _transactionService.AddTransaction(transaction);
                return CreatedAtAction(nameof(GetTransaction), new { id = createdTransaction.Id }, createdTransaction);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/transactions/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            try
            {
                var transaction = await _transactionService.GetTransaction(id);
                if (transaction == null)
                {
                    return NotFound($"Transaction with ID {id} not found");
                }
                return Ok(transaction);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/transactions/client/{clientId}
        [HttpGet("client/{clientId}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetClientTransactions(int clientId)
        {
            try
            {
                var transactions = await _transactionService.GetTransactionsByClientId(clientId);
                return Ok(transactions);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
