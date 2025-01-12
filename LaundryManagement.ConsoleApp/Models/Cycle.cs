namespace LaundryManagement.ConsoleApp.Models
{
    public class Cycle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; } // Duration in minutes
        public decimal Price { get; set; }
    }
}
