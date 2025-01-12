using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace LaundryManagement.Infrastructure.Models
{
    // LaundryManagement.ConsoleApp/Models/Machine.cs
namespace LaundryManagement.ConsoleApp.Models
{
    public class Machine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; } = "Available";
        public int LaundryId { get; set; }
        public virtual Laundry Laundry { get; set; }
        public DateTime? CycleStartTime { get; set; }
        public int? CurrentCycleId { get; set; }
        public virtual Cycle CurrentCycle { get; set; }
    }
}
}
