using System;
using System.Threading;
using System.Threading.Tasks;
using LaundryManagement.Infrastructure.Data;
using LaundryManagement.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace LaundryManagement.SimulationApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Create a DbContext instance
            var options = new DbContextOptionsBuilder<LaundryDbContext>()
                .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=LaundryManagement;Trusted_Connection=True;")
                .Options;

            using (var context = new LaundryDbContext(options))
            {
                Console.WriteLine("Welcome to Laundry Management System");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.Write("Choose an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await Register(context);
                        break;
                    case "2":
                        await Login(context);
                        break;
                    default:
                        Console.WriteLine("Invalid option. Exiting...");
                        break;
                }
            }
        }

        static async Task Register(LaundryDbContext context)
        {
            try
            {
                Console.WriteLine("Are you a client or an owner?");
                Console.WriteLine("1. Client");
                Console.WriteLine("2. Owner");
                Console.Write("Choose an option: ");
                var userType = Console.ReadLine();

                Console.Write("Enter your last name (Nom): ");
                var nom = Console.ReadLine();

                Console.Write("Enter your first name (Prenom): ");
                var prenom = Console.ReadLine();

                Console.Write("Enter your email: ");
                var email = Console.ReadLine();

                Console.Write("Enter your password: ");
                var password = Console.ReadLine();

                if (userType == "1")
                {
                    // Prompt for phone number for clients
                    Console.Write("Enter your phone number: ");
                    var phone = Console.ReadLine();

                    var client = new Client
                    {
                        Nom = nom,
                        Prenom = prenom,
                        Email = email,
                        Password = password,
                        Phone = phone // Add the phone number
                    };
                    context.Clients.Add(client);
                }
                else if (userType == "2")
                {
                    Console.Write("Enter your phone number: ");
                    var phone = Console.ReadLine();

                    var owner = new Owner
                    {
                        Nom = nom,
                        Prenom = prenom,
                        Email = email,
                        Password = password, 
                        Phone = phone // Add the phone number
                    };
                    context.Owners.Add(owner);
                }
                else
                {
                    Console.WriteLine("Invalid option.");
                    return;
                }

                await context.SaveChangesAsync();
                Console.WriteLine("Registration successful!");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"An error occurred while saving changes: {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        static async Task Login(LaundryDbContext context)
        {
            Console.WriteLine("Are you a client or an owner?");
            Console.WriteLine("1. Client");
            Console.WriteLine("2. Owner");
            Console.Write("Choose an option: ");
            var userType = Console.ReadLine();

            Console.Write("Enter your email: ");
            var email = Console.ReadLine();

            Console.Write("Enter your password: ");
            var password = Console.ReadLine();

            if (userType == "1")
            {
                var client = await context.Clients.FirstOrDefaultAsync(c => c.Email == email && c.Password == password);
                if (client == null)
                {
                    Console.WriteLine("Invalid email or password.");
                    return;
                }
                await ClientMenu(context, client.Id);
            }
            else if (userType == "2")
            {
                var owner = await context.Owners.FirstOrDefaultAsync(o => o.Email == email && o.Password == password);
                if (owner == null)
                {
                    Console.WriteLine("Invalid email or password.");
                    return;
                }
                await OwnerMenu(context, owner.Id);
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }
        }

        static async Task ClientMenu(LaundryDbContext context, int clientId)
        {
            while (true)
            {
                Console.WriteLine("\nClient Menu");
                Console.WriteLine("1. View Machines");
                Console.WriteLine("2. Start Machine");
                Console.WriteLine("3. Stop Machine");
                Console.WriteLine("4. Exit");
                Console.Write("Choose an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ViewMachines(context);
                        break;
                    case "2":
                        await StartMachine(context, clientId);
                        break;
                    case "3":
                        await StopMachine(context, clientId);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static async Task OwnerMenu(LaundryDbContext context, int ownerId)
        {
            while (true)
            {
                Console.WriteLine("\nOwner Menu");
                Console.WriteLine("1. View Machines");
                Console.WriteLine("2. Add Machine");
                Console.WriteLine("3. Remove Machine");
                Console.WriteLine("4. View Income");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ViewMachines(context);
                        break;
                    case "2":
                        await AddMachine(context, ownerId);
                        break;
                    case "3":
                        await RemoveMachine(context);
                        break;
                    case "4":
                        await ViewIncome(context);
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static async Task ViewMachines(LaundryDbContext context)
        {
            Console.WriteLine("\nAvailable Machines:");
            var machines = await context.Machines.Include(m => m.Cycles).ToListAsync();
            foreach (var machine in machines)
            {
                Console.WriteLine($"ID: {machine.Id}, Type: {machine.Type}, Status: {machine.Status}");
                if (machine.Cycles != null)
                {
                    Console.WriteLine("Available Cycles:");
                    foreach (var cycle in machine.Cycles)
                    {
                        Console.WriteLine($"  Cycle ID: {cycle.Id}, Type: {cycle.Type}, Duration: {cycle.Duration} mins, Cost: {cycle.Cost:C}");
                    }
                }
            }
            Console.WriteLine();
        }

        static async Task StartMachine(LaundryDbContext context, int clientId)
        {
            Console.Write("Enter Machine ID: ");
            if (!int.TryParse(Console.ReadLine(), out int machineId))
            {
                Console.WriteLine("Invalid Machine ID.");
                return;
            }

            var machine = await context.Machines.Include(m => m.Cycles).FirstOrDefaultAsync(m => m.Id == machineId);
            if (machine == null)
            {
                Console.WriteLine("Machine not found.");
                return;
            }

            if (machine.Status == "Working")
            {
                Console.WriteLine("Machine is already in use.");
                return;
            }

            Console.WriteLine("Available Cycles:");
            foreach (var ccycle in machine.Cycles)
            {
                Console.WriteLine($"  Cycle ID: {ccycle.Id}, Type: {ccycle.Type}, Duration: {ccycle.Duration} mins, Cost: {ccycle.Cost:C}");
            }

            Console.Write("Enter Cycle ID: ");
            if (!int.TryParse(Console.ReadLine(), out int cycleId))
            {
                Console.WriteLine("Invalid Cycle ID.");
                return;
            }

            var cycle = machine.Cycles.FirstOrDefault(c => c.Id == cycleId);
            if (cycle == null)
            {
                Console.WriteLine("Cycle not found.");
                return;
            }

            // Update machine status
            machine.Status = "Working";
            await context.SaveChangesAsync();

            // Record the transaction
            var transaction = new Transaction
            {
                MachineId = machineId,
                CycleId = cycleId,
                Amount = cycle.Cost,
                TransactionDate = DateTime.UtcNow
                
            };
            context.Transactions.Add(transaction);
            await context.SaveChangesAsync();

            Console.WriteLine($"Machine {machine.Id} started with Cycle {cycle.Type}. Duration: {cycle.Duration} mins, Cost: {cycle.Cost:C}");

            // Start a timer to automatically stop the machine
            var timer = new Timer(async _ =>
            {
                machine.Status = "Available";
                await context.SaveChangesAsync();
                Console.WriteLine($"Machine {machine.Id} stopped automatically after {cycle.Duration} mins.");
            }, null, cycle.Duration * 60 * 1000, Timeout.Infinite);
        }

        static async Task StopMachine(LaundryDbContext context, int clientId)
        {
            Console.Write("Enter Machine ID: ");
            if (!int.TryParse(Console.ReadLine(), out int machineId))
            {
                Console.WriteLine("Invalid Machine ID.");
                return;
            }

            var machine = await context.Machines.FirstOrDefaultAsync(m => m.Id == machineId);
            if (machine == null)
            {
                Console.WriteLine("Machine not found.");
                return;
            }

            if (machine.Status != "Working")
            {
                Console.WriteLine("Machine is not in use.");
                return;
            }

            // Update machine status
            machine.Status = "Available";
            await context.SaveChangesAsync();

            Console.WriteLine($"Machine {machine.Id} stopped.");
        }

        static async Task AddMachine(LaundryDbContext context, int ownerId)
        {
            try
            {
                // Check if the owner has a corresponding Laundry record
                var laundry = await context.Laundries.FirstOrDefaultAsync(l => l.IdProprietaire == ownerId);

                if (laundry == null)
                {
                    // If no Laundry record exists, create one
                    laundry = new Laundry
                    {
                        IdProprietaire = ownerId,
                        Nom = "Default Laundry Name" // Set a default name or prompt the user for input
                    };
                    context.Laundries.Add(laundry);
                    await context.SaveChangesAsync(); // Save the new Laundry record
                }

                // Prompt for machine type
                Console.Write("Enter Machine Type (e.g., Washer, Dryer): ");
                var machineType = Console.ReadLine();

                // Create the machine
                var machine = new Machine
                {
                    Type = machineType,
                    Status = "Available",
                    IdLaundry = laundry.Id // Use the Id of the Laundry record
                };

                // Add cycles to the machine
                var cycles = new List<Cycle>();
                while (true)
                {
                    Console.WriteLine("\nAdd a cycle for the machine (or type 'done' to finish):");
                    Console.Write("Enter Cycle Type (e.g., Normal, Delicate): ");
                    var cycleType = Console.ReadLine();

                    if (cycleType.ToLower() == "done")
                        break;

                    Console.Write("Enter Cycle Cost: ");
                    if (!decimal.TryParse(Console.ReadLine(), out decimal cycleCost))
                    {
                        Console.WriteLine("Invalid cost. Please enter a valid number.");
                        continue;
                    }

                    Console.Write("Enter Cycle Duration (in minutes): ");
                    if (!int.TryParse(Console.ReadLine(), out int cycleDuration))
                    {
                        Console.WriteLine("Invalid duration. Please enter a valid number.");
                        continue;
                    }

                    // Create the cycle
                    var cycle = new Cycle
                    {
                        Type = cycleType,
                        Cost = cycleCost,
                        Duration = cycleDuration,
                        IdMachine = machine.Id // This will be set after the machine is saved
                    };

                    cycles.Add(cycle);
                }

                // Add the machine to the context
                context.Machines.Add(machine);
                await context.SaveChangesAsync(); // Save the machine to generate its Id

                // Set the IdMachine for each cycle and add them to the context
                foreach (var cycle in cycles)
                {
                    cycle.IdMachine = machine.Id;
                    context.Cycles.Add(cycle);
                }

                await context.SaveChangesAsync(); // Save the cycles

                Console.WriteLine($"Machine {machine.Id} added with {cycles.Count} cycle(s).");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"An error occurred while adding the machine: {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        static async Task RemoveMachine(LaundryDbContext context)
        {
            Console.Write("Enter Machine ID: ");
            if (!int.TryParse(Console.ReadLine(), out int machineId))
            {
                Console.WriteLine("Invalid Machine ID.");
                return;
            }

            var machine = await context.Machines.FirstOrDefaultAsync(m => m.Id == machineId);
            if (machine == null)
            {
                Console.WriteLine("Machine not found.");
                return;
            }

            context.Machines.Remove(machine);
            await context.SaveChangesAsync();

            Console.WriteLine($"Machine {machine.Id} removed.");
        }

        static async Task ViewIncome(LaundryDbContext context)
        {
            var today = DateTime.UtcNow.Date;
            var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var startOfYear = new DateTime(DateTime.UtcNow.Year, 1, 1);

            var dailyIncome = await context.Transactions
                .Where(t => t.TransactionDate >= today)
                .SumAsync(t => t.Amount);

            var monthlyIncome = await context.Transactions
                .Where(t => t.TransactionDate >= startOfMonth)
                .SumAsync(t => t.Amount);

            var yearlyIncome = await context.Transactions
                .Where(t => t.TransactionDate >= startOfYear)
                .SumAsync(t => t.Amount);

            Console.WriteLine("\nIncome Report:");
            Console.WriteLine($"Daily Income: {dailyIncome:C}");
            Console.WriteLine($"Monthly Income: {monthlyIncome:C}");
            Console.WriteLine($"Yearly Income: {yearlyIncome:C}");
        }
    }
}