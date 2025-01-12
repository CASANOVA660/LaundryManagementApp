using System;

namespace LaundryManagement.ConsoleApp.Models
{
    public class Machine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public int LaundryId { get; set; }
        public DateTime? CycleStartTime { get; set; }
        public int? CurrentCycleId { get; set; }
    }
}
