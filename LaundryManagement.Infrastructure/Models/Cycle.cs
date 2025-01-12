using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LaundryManagement.Infrastructure.Models
{
      public class Cycle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
