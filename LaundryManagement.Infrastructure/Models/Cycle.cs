using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaundryManagement.Infrastructure.Models
{
    public class Cycle
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Cost { get; set; }
        public int Duration { get; set; }
        public int IdMachine { get; set; }
        public Machine Machine { get; set; }
    }
}
