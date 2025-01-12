using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaundryManagement.Infrastructure.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        public int IdClient { get; set; }

        [Required]
        public int IdCycle { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [ForeignKey("IdClient")]
        public virtual Client Client { get; set; }

        [ForeignKey("IdCycle")]
        public virtual Cycle Cycle { get; set; }
    }
}
