using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryManagement.Infrastructure.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int MachineId { get; set; }
        public int CycleId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    }
}
