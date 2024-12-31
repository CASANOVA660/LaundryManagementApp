using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryManagement.Infrastructure.Models
{
    public class Machine
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Status { get; set; } = "Available";
        public int IdLaundry { get; set; }
        public Laundry Laundry { get; set; }
        public ICollection<Cycle> Cycles { get; set; }
    }
}
