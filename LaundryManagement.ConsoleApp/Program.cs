using LaundryManagement.ConsoleApp.Models;
using LaundryManagement.ConsoleApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryManagement.ConsoleApp
{
    class Program
    {
        private static ILaundryService _laundryService;

        static async Task Main(string[] args)
        {
            SetupServices();
            await RunMainMenu();
        }

        private static void SetupServices()
        {
            var services = new ServiceCollection();
            services.AddDbContext<DbContext>(options => 
                options.UseSqlServer("your_connection_string_here"));
            services.AddScoped<ILaundryService, LaundryService>();

            var serviceProvider = services.BuildServiceProvider();
            _laundryService = serviceProvider.GetRequiredService<ILaundryService>();
        }

        private static async Task RunMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Laundry Management System ===");
                Console.WriteLine("1. Select Laundry");
                Console.WriteLine("0. Exit");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        await SelectLaundry();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static async Task SelectLaundry()
        {
            var laundries = await _laundryService.GetLaundries();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Available Laundries ===");
                foreach (var laundry in laundries)
                {
                    Console.WriteLine($"{laundry.Id}. {laundry.Name} - {laundry.Location}");
                }
                Console.WriteLine("0. Back to Main Menu");

                if (int.TryParse(Console.ReadLine(), out int laundryId))
                {
                    if (laundryId == 0) return;
                    if (laundries.Any(l => l.Id == laundryId))
                    {
                        await ManageMachines(laundryId);
                    }
                }
                
                Console.WriteLine("Invalid selection. Press any key to continue...");
                Console.ReadKey();
            }
        }

        private static async Task ManageMachines(int laundryId)
        {
            while (true)
            {
                Console.Clear();
                var machines = await _laundryService.GetMachinesByLaundryId(laundryId);
                Console.WriteLine("=== Machines ===");
                foreach (var machine in machines)
                {
                    Console.WriteLine($"{machine.Id}. {machine.Name} - Status: {machine.Status}");
                    if (machine.Status == "Running")
                    {
                        if (await _laundryService.IsCycleComplete(machine.Id))
                        {
                            Console.WriteLine("   Cycle Complete!");
                        }
                    }
                }
                
                Console.WriteLine("\nOptions:");
                Console.WriteLine("1. Start Machine");
                Console.WriteLine("2. Stop Machine");
                Console.WriteLine("0. Back to Laundry Selection");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        await StartMachine(machines);
                        break;
                    case "2":
                        await StopMachine(machines);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static async Task StartMachine(IEnumerable<Machine> machines)
        {
            Console.WriteLine("\nSelect Machine ID:");
            if (!int.TryParse(Console.ReadLine(), out int machineId) || 
                !machines.Any(m => m.Id == machineId))
            {
                Console.WriteLine("Invalid machine selection.");
                Console.ReadKey();
                return;
            }

            var cycles = await _laundryService.GetAvailableCycles();
            Console.WriteLine("\nAvailable Cycles:");
            foreach (var cycle in cycles)
            {
                Console.WriteLine($"{cycle.Id}. {cycle.Name} - Duration: {cycle.Duration} minutes, Price: ${cycle.Price}");
            }

            Console.WriteLine("\nSelect Cycle ID:");
            if (!int.TryParse(Console.ReadLine(), out int cycleId) ||
                !cycles.Any(c => c.Id == cycleId))
            {
                Console.WriteLine("Invalid cycle selection.");
                Console.ReadKey();
                return;
            }

            if (await _laundryService.StartMachine(machineId, cycleId))
            {
                Console.WriteLine("Machine started successfully!");
            }
            else
            {
                Console.WriteLine("Failed to start machine. Make sure it's available.");
            }
            Console.ReadKey();
        }

        private static async Task StopMachine(IEnumerable<Machine> machines)
        {
            Console.WriteLine("\nSelect Machine ID to stop:");
            if (!int.TryParse(Console.ReadLine(), out int machineId) ||
                !machines.Any(m => m.Id == machineId))
            {
                Console.WriteLine("Invalid machine selection.");
                Console.ReadKey();
                return;
            }

            if (await _laundryService.StopMachine(machineId))
            {
                Console.WriteLine("Machine stopped successfully!");
            }
            else
            {
                Console.WriteLine("Failed to stop machine. Make sure it's running.");
            }
            Console.ReadKey();
        }
    }
}
