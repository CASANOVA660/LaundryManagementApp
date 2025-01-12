using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using LaundryManagement.Infrastructure.Models;

namespace LaundryManagement.SimulationApp
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5042/")
        };

        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Welcome to Laundry Management System");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await Register();
                        break;
                    case "2":
                        await Login();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static async Task Register()
        {
            try
            {
                Console.WriteLine("Are you a client or an owner?");
                Console.WriteLine("1. Client");
                Console.WriteLine("2. Owner");
                Console.Write("Choose an option: ");
                var userType = Console.ReadLine();

                switch (userType)
                {
                    case "1":
                        await RegisterClient();
                        break;
                    case "2":
                        await RegisterOwner();
                        break;
                    default:
                        Console.WriteLine("Invalid user type selected.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static async Task RegisterClient()
        {
            Console.Write("Enter your last name (Nom): ");
            var nom = Console.ReadLine();

            Console.Write("Enter your first name (Prenom): ");
            var prenom = Console.ReadLine();

            Console.Write("Enter your email: ");
            var email = Console.ReadLine();

            Console.Write("Enter your password: ");
            var password = Console.ReadLine();

            Console.Write("Enter your phone number: ");
            var phone = Console.ReadLine();

            var client = new
            {
                Nom = nom,
                Prenom = prenom,
                Email = email,
                Password = password,
                Phone = phone
            };

            try
            {
                var response = await Program.client.PostAsJsonAsync("api/auth/register/client", client);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Successfully registered client: {prenom} {nom}");
                    // Automatically log in with the new account
                    Console.WriteLine("Logging in with your new account...");
                    await LoginClient(email, password);
                }
                else
                {
                    Console.WriteLine($"Registration failed: {content}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task RegisterOwner()
        {
            Console.Write("Enter your last name (Nom): ");
            var nom = Console.ReadLine();

            Console.Write("Enter your first name (Prenom): ");
            var prenom = Console.ReadLine();

            Console.Write("Enter your email: ");
            var email = Console.ReadLine();

            Console.Write("Enter your password: ");
            var password = Console.ReadLine();

            Console.Write("Enter your phone number: ");
            var phone = Console.ReadLine();

            var owner = new
            {
                Nom = nom,
                Prenom = prenom,
                Email = email,
                Password = password,
                Phone = phone
            };

            try
            {
                var response = await Program.client.PostAsJsonAsync("api/auth/register/owner", owner);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Successfully registered owner: {prenom} {nom}");
                    // Automatically log in with the new account
                    Console.WriteLine("Logging in with your new account...");
                    await LoginOwner(email, password);
                }
                else
                {
                    Console.WriteLine($"Registration failed: {content}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task Login(string email = null, string password = null, bool? isClient = null)
        {
            try
            {
                if (isClient == null)
                {
                    Console.WriteLine("Are you a client or an owner?");
                    Console.WriteLine("1. Client");
                    Console.WriteLine("2. Owner");
                    Console.Write("Choose an option: ");
                    var userType = Console.ReadLine();
                    isClient = userType == "1";
                }

                if (email == null)
                {
                    Console.Write("Enter your email: ");
                    email = Console.ReadLine();
                }

                if (password == null)
                {
                    Console.Write("Enter your password: ");
                    password = Console.ReadLine();
                }

                var loginModel = new { Email = email, Password = password };

                if (isClient.Value)
                {
                    var clientResponse = await Program.client.PostAsJsonAsync("api/auth/login/client", loginModel);
                    if (clientResponse.IsSuccessStatusCode)
                    {
                        var loggedInClient = await clientResponse.Content.ReadFromJsonAsync<Client>();
                        Console.WriteLine($"Successfully logged in as client: {loggedInClient.Nom} {loggedInClient.Prenom}");
                        await ClientMenu(loggedInClient.Id);
                    }
                    else
                    {
                        var error = await clientResponse.Content.ReadAsStringAsync();
                        Console.WriteLine($"Login failed: {error}");
                    }
                }
                else
                {
                    var ownerResponse = await Program.client.PostAsJsonAsync("api/auth/login/owner", loginModel);
                    if (ownerResponse.IsSuccessStatusCode)
                    {
                        var loggedInOwner = await ownerResponse.Content.ReadFromJsonAsync<Owner>();
                        Console.WriteLine($"Successfully logged in as owner: {loggedInOwner.Nom} {loggedInOwner.Prenom}");
                        await OwnerMenu(loggedInOwner.Id);
                    }
                    else
                    {
                        var error = await ownerResponse.Content.ReadAsStringAsync();
                        Console.WriteLine($"Login failed: {error}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task LoginClient(string email, string password)
        {
            try
            {
                var loginModel = new { Email = email, Password = password };

                var clientResponse = await Program.client.PostAsJsonAsync("api/auth/login/client", loginModel);
                if (clientResponse.IsSuccessStatusCode)
                {
                    var loggedInClient = await clientResponse.Content.ReadFromJsonAsync<Client>();
                    Console.WriteLine($"Successfully logged in as client: {loggedInClient.Nom} {loggedInClient.Prenom}");
                    await ClientMenu(loggedInClient.Id);
                }
                else
                {
                    var error = await clientResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Login failed: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task LoginOwner(string email, string password)
        {
            try
            {
                var loginModel = new { Email = email, Password = password };

                var ownerResponse = await Program.client.PostAsJsonAsync("api/auth/login/owner", loginModel);
                if (ownerResponse.IsSuccessStatusCode)
                {
                    var loggedInOwner = await ownerResponse.Content.ReadFromJsonAsync<Owner>();
                    Console.WriteLine($"Successfully logged in as owner: {loggedInOwner.Nom} {loggedInOwner.Prenom}");
                    await OwnerMenu(loggedInOwner.Id);
                }
                else
                {
                    var error = await ownerResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Login failed: {error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task ClientMenu(int clientId)
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
                        await ViewMachines();
                        break;
                    case "2":
                        await StartMachine(clientId);
                        break;
                    case "3":
                        await StopMachine(clientId);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static async Task OwnerMenu(int ownerId)
        {
            // First check if owner has a laundry, if not create one
            var laundryResponse = await Program.client.GetAsync($"api/laundry/owner/{ownerId}");
            if (!laundryResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("Creating a new laundry for your account...");
                var ownerResponse = await Program.client.GetAsync($"api/auth/owner/{ownerId}");
                if (ownerResponse.IsSuccessStatusCode)
                {
                    var owner = await ownerResponse.Content.ReadFromJsonAsync<Owner>();
                    var laundry = new Laundry
                    {
                        Nom = $"{owner.Prenom}'s Laundry",
                        IdProprietaire = ownerId
                    };
                    
                    var createLaundryResponse = await Program.client.PostAsJsonAsync("api/laundry", laundry);
                    if (createLaundryResponse.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Laundry created successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to create laundry. Some features may not work correctly.");
                    }
                }
            }

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
                        await ViewMachines();
                        break;
                    case "2":
                        await AddMachine(ownerId);
                        break;
                    case "3":
                        await RemoveMachine();
                        break;
                    case "4":
                        await ViewIncome();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static async Task ViewMachines()
        {
            var response = await Program.client.GetAsync("api/machines");
            if (response.IsSuccessStatusCode)
            {
                var machines = await response.Content.ReadFromJsonAsync<List<Machine>>();
                Console.WriteLine("\nAvailable Machines:");
                foreach (var machine in machines)
                {
                    Console.WriteLine($"ID: {machine.Id}, Type: {machine.Type}, Status: {machine.Status}");
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Failed to retrieve machines.");
            }
        }

        static async Task StartMachine(int clientId)
        {
            try
            {
                Console.Write("Enter Machine ID: ");
                if (!int.TryParse(Console.ReadLine(), out int machineId))
                {
                    Console.WriteLine("Invalid machine ID");
                    return;
                }

                // Get machine status first
                var machineResponse = await Program.client.GetAsync($"api/machines/{machineId}");
                if (!machineResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("Failed to find machine.");
                    return;
                }

                var machine = await machineResponse.Content.ReadFromJsonAsync<Machine>();
                if (machine.Status != "Available")
                {
                    Console.WriteLine($"Machine {machineId} is not available (Current status: {machine.Status})");
                    return;
                }

                // Get available cycles for the machine
                var cyclesResponse = await Program.client.GetAsync($"api/cycles/machine/{machineId}");
                var cyclesContent = await cyclesResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"Debug - Cycles response: {cyclesContent}");

                if (!cyclesResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("Failed to retrieve cycles.");
                    return;
                }

                var cycles = await cyclesResponse.Content.ReadFromJsonAsync<List<Cycle>>();
                if (cycles == null || !cycles.Any())
                {
                    Console.WriteLine("No cycles available for this machine.");
                    return;
                }

                Console.WriteLine("\nAvailable Cycles:");
                foreach (var cycle in cycles)
                {
                    Console.WriteLine($"ID: {cycle.Id}, Type: {cycle.Type}, Cost: ${cycle.Cost:F2}, Duration: {cycle.Duration} minutes");
                }

                Console.Write("\nEnter Cycle ID to start: ");
                if (!int.TryParse(Console.ReadLine(), out int cycleId))
                {
                    Console.WriteLine("Invalid cycle ID");
                    return;
                }

                var selectedCycle = cycles.FirstOrDefault(c => c.Id == cycleId);
                if (selectedCycle == null)
                {
                    Console.WriteLine("Invalid cycle ID selected");
                    return;
                }

                // Create a transaction for the cycle
                var transaction = new
                {
                    IdClient = clientId,
                    IdCycle = cycleId,
                    Amount = selectedCycle.Cost,
                    Status = "Pending"
                };

                Console.WriteLine($"Debug - Sending transaction data: {System.Text.Json.JsonSerializer.Serialize(transaction)}");

                var transactionResponse = await Program.client.PostAsJsonAsync("api/transactions", transaction);
                var transactionContent = await transactionResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"Debug - Transaction response: {transactionContent}");

                if (transactionResponse.IsSuccessStatusCode)
                {
                    // Update machine status to "In Use"
                    var updateResponse = await Program.client.PutAsJsonAsync($"api/machines/{machineId}/status", "\"In Use\"");
                    if (updateResponse.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Machine {machineId} started with {selectedCycle.Type} cycle");
                        Console.WriteLine($"Cost: ${selectedCycle.Cost:F2}, Duration: {selectedCycle.Duration} minutes");
                    }
                    else
                    {
                        var updateError = await updateResponse.Content.ReadAsStringAsync();
                        Console.WriteLine($"Failed to update machine status: {updateError}");
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to create transaction: {transactionContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task StopMachine(int clientId)
{
    Console.Write("Enter Machine ID: ");
    if (!int.TryParse(Console.ReadLine(), out int machineId))
    {
        Console.WriteLine("Invalid Machine ID.");
        return;
    }

    var response = await Program.client.GetAsync($"api/machines/{machineId}");
    if (!response.IsSuccessStatusCode)
    {
        Console.WriteLine("Machine not found.");
        return;
    }

    var machine = await response.Content.ReadFromJsonAsync<Machine>();
    if (machine.Status != "In Use")
    {
        Console.WriteLine("Machine is not in use.");
        return;
    }

    var stopResponse = await Program.client.PostAsync($"api/machines/{machineId}/stop", null);
    if (stopResponse.IsSuccessStatusCode)
    {
        Console.WriteLine($"Machine {machineId} stopped successfully.");
    }
    else
    {
        var error = await stopResponse.Content.ReadAsStringAsync();
        Console.WriteLine($"Failed to stop machine: {error}");
    }
}

        static async Task AddMachine(int ownerId)
        {
            try
            {
                // First, get the laundry for this owner
                var laundryResponse = await Program.client.GetAsync($"api/laundry/owner/{ownerId}");
                var responseContent = await laundryResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"Debug - Laundry response: {responseContent}");

                if (!laundryResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to find laundry for this owner. Status: {laundryResponse.StatusCode}");
                    return;
                }

                var laundry = await laundryResponse.Content.ReadFromJsonAsync<Laundry>();
                if (laundry == null)
                {
                    Console.WriteLine("No laundry found for this owner.");
                    return;
                }

                // Prompt for machine type
                Console.Write("Enter Machine Type (e.g., Washer, Dryer): ");
                var machineType = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(machineType))
                {
                    Console.WriteLine("Machine type cannot be empty.");
                    return;
                }

                // Create the machine object with only the required fields
                var machineRequest = new
                {
                    Type = machineType,
                    Status = "Available",
                    IdLaundry = laundry.Id
                };

                Console.WriteLine($"Debug - Sending machine data: {System.Text.Json.JsonSerializer.Serialize(machineRequest)}");

                // Add the machine first
                var machineResponse = await Program.client.PostAsJsonAsync("api/machines", machineRequest);
                var machineResponseContent = await machineResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"Debug - Machine response: {machineResponseContent}");

                if (machineResponse.IsSuccessStatusCode)
                {
                    var addedMachine = await machineResponse.Content.ReadFromJsonAsync<Machine>();
                    Console.WriteLine($"Successfully added machine {addedMachine.Id}");

                    // Now add cycles
                    var cycles = new List<Cycle>();
                    while (true)
                    {
                        Console.WriteLine("\nAdd a cycle for the machine (or type 'done' to finish):");
                        Console.Write("Enter Cycle Type (e.g., Normal, Delicate): ");
                        var cycleType = Console.ReadLine();

                        if (cycleType?.ToLower() == "done")
                            break;

                        if (string.IsNullOrWhiteSpace(cycleType))
                        {
                            Console.WriteLine("Cycle type cannot be empty. Please try again.");
                            continue;
                        }

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

                        // Create a cycle for the machine
                        var cycleData = new
                        {
                            Type = cycleType,
                            Cost = cycleCost,
                            Duration = cycleDuration,
                            IdMachine = addedMachine.Id
                        };

                        Console.WriteLine($"Debug - Sending cycle data: {System.Text.Json.JsonSerializer.Serialize(cycleData)}");

                        var cycleResponse = await Program.client.PostAsJsonAsync("api/cycles", cycleData);
                        var cycleResponseContent = await cycleResponse.Content.ReadAsStringAsync();
                        Console.WriteLine($"Debug - Cycle response: {cycleResponseContent}");

                        if (cycleResponse.IsSuccessStatusCode)
                        {
                            var addedCycle = await cycleResponse.Content.ReadFromJsonAsync<Cycle>();
                            Console.WriteLine($"Successfully added cycle {cycleType} to machine {addedMachine.Id}");
                            Console.WriteLine($"Cost: ${cycleCost:F2}, Duration: {cycleDuration} minutes");
                            cycles.Add(addedCycle);
                        }
                        else
                        {
                            Console.WriteLine($"Failed to add cycle {cycleType}: {cycleResponseContent}");
                        }
                    }

                    Console.WriteLine($"Machine {addedMachine.Id} added with {cycles.Count} cycle(s).");
                }
                else
                {
                    Console.WriteLine($"Failed to add machine: {machineResponseContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task RemoveMachine()
        {
            Console.Write("Enter Machine ID: ");
            if (!int.TryParse(Console.ReadLine(), out int machineId))
            {
                Console.WriteLine("Invalid Machine ID.");
                return;
            }

            var response = await Program.client.DeleteAsync($"api/machines/{machineId}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Machine {machineId} removed.");
            }
            else
            {
                Console.WriteLine("Failed to remove machine.");
            }
        }

        static async Task ViewIncome()
        {
            var response = await Program.client.GetAsync("api/income");
            if (response.IsSuccessStatusCode)
            {
                var income = await response.Content.ReadFromJsonAsync<Income>();
                Console.WriteLine("\nIncome Report:");
                Console.WriteLine($"Daily Income: {income.Daily:C}");
                Console.WriteLine($"Monthly Income: {income.Monthly:C}");
                Console.WriteLine($"Yearly Income: {income.Yearly:C}");
            }
            else
            {
                Console.WriteLine("Failed to retrieve income.");
            }
        }
    }
}