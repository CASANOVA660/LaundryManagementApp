using LaundryManagement.ConsoleApp.Data;
using LaundryManagement.ConsoleApp.Models;
using LaundryManagement.ConsoleApp.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryManagement.ConsoleApp
{
    class Program
    {
        private static LaundryDbContext _context;
        private static LaundryService _laundryService;

        static async Task Main(string[] args)
        {
            SetupDatabase();
            _laundryService = new LaundryService(_context);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to Laundry Management System");
                Console.WriteLine("1. Select Laundry");
                Console.WriteLine("2. Exit");
                
                var choice = Console.ReadLine();
                
                if (choice == "2") break;
                
                if (choice == "1")
                {
                    await SelectLaundry();
                }
            }
        }

        private static async Task SelectLaundry()
        {
            var laundries = await _context.Laundries.ToListAsync();
            Console.WriteLine("\nAvailable Laundries:");
            foreach (var laundry in laundries)
            {
                Console.WriteLine($"{laundry.Id}. {laundry.Name} - {laundry.Location}");
            }

            Console.Write("\nSelect laundry ID: ");
            if (int.TryParse(Console.ReadLine(), out int laundryId))
            {
                await ManageMachines(laundryId);
            }
        }

        private static async Task ManageMachines(int laundryId)
        {
            while (true)
            {
                Console.Clear();
                var machines = await _context.Machines
                    .Include(m => m.CurrentCycle)
                    .Where(m => m.LaundryId == laundryId)
                    .ToListAsync();

                Console.WriteLine("Machines Status:");
                foreach (var machine in machines)
                {
                    var status = machine.CurrentCycle != null 
                        ? $"Running {machine.CurrentCycle.Name} - Started at {machine.CycleStartTime}"
                        : machine.Status;
                    Console.WriteLine($"{machine.Id}. {machine.Name} - Status: {status}");
                }

                Console.WriteLine("\n1. Start Machine");
                Console.WriteLine("2. Stop Machine");
                Console.WriteLine("3. Back to Laundry Selection");

                var choice = Console.ReadLine();
                if (choice == "3") break;

                switch (choice)
                {
                    case "1":
                        await StartMachine(machines);
                        break;
                    case "2":
                        await StopMachine(machines);
                        break;
                }
            }
        }

        private static async Task StartMachine(IEnumerable<Machine> machines)
        {
            Console.Write("Enter machine ID: ");
            if (!int.TryParse(Console.ReadLine(), out int machineId)) return;

            var cycles = await _context.Cycles.ToListAsync();
            Console.WriteLine("\nAvailable Cycles:");
            foreach (var cycle in cycles)
            {
                Console.WriteLine($"{cycle.Id}. {cycle.Name} - {cycle.DurationMinutes} minutes - ${cycle.Price}");
            }

            Console.Write("Select cycle ID: ");
            if (int.TryParse(Console.ReadLine(), out int cycleId))
            {
                await _laundryService.StartMachine(machineId, cycleId);
            }
        }

        private static async Task StopMachine(IEnumerable<Machine> machines)
        {
            Console.Write("Enter machine ID: ");
            if (int.TryParse(Console.ReadLine(), out int machineId))
            {
                await _laundryService.StopMachine(machineId);
            }
        }

        private static void SetupDatabase()
        {
            _context = new LaundryDbContext();
            _context.Database.EnsureCreated();

            if (!_context.Laundries.Any())
            {
                SeedData();
            }
        }

        private static void SeedData()
        {
            // Add sample laundries
            var laundry1 = new Laundry { Name = "Downtown Laundry", Location = "123 Main St" };
            var laundry2 = new Laundry { Name = "Campus Laundry", Location = "456 College Ave" };
            _context.Laundries.AddRange(laundry1, laundry2);
            _context.SaveChanges();

            // Add sample cycles
            var cycles = new[]
            {
                new Cycle { Name = "Quick Wash", DurationMinutes = 30, Price = 5.00M, Description = "Fast cycle for lightly soiled clothes" },
                new Cycle { Name = "Normal Wash", DurationMinutes = 45, Price = 7.50M, Description = "Standard washing cycle" },
                new Cycle { Name = "Heavy Duty", DurationMinutes = 60, Price = 10.00M, Description = "For heavily soiled items" }
            };
            _context.Cycles.AddRange(cycles);
            _context.SaveChanges();

            // Add sample machines
            var machines = new[]
            {
                new Machine { Name = "Machine 1", LaundryId = laundry1.Id },
                new Machine { Name = "Machine 2", LaundryId = laundry1.Id },
                new Machine { Name = "Machine 3", LaundryId = laundry2.Id },
                new Machine { Name = "Machine 4", LaundryId = laundry2.Id }
            };
            _context.Machines.AddRange(machines);
            _context.SaveChanges();
        }
    }
}