using LaundryManagement.Infrastructure.Models;
using LaundryManagement.API.Interfaces;
using LaundryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LaundryManagement.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly LaundryDbContext _context;

        public AuthService(LaundryDbContext context)
        {
            _context = context;
        }

        public async Task<Client> RegisterClient(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<Owner> RegisterOwner(Owner owner)
        {
            _context.Owners.Add(owner);
            await _context.SaveChangesAsync();

            // Create a laundry for the owner
            var laundry = new Laundry
            {
                Nom = $"{owner.Prenom}'s Laundry",
                IdProprietaire = owner.Id,
                Proprietaire = owner
            };

            _context.Laundries.Add(laundry);
            await _context.SaveChangesAsync();

            return owner;
        }

        public async Task<Client> LoginClient(string email, string password)
        {
            return await _context.Clients.FirstOrDefaultAsync(c => c.Email == email && c.Password == password);
        }

        public async Task<Owner> LoginOwner(string email, string password)
        {
            return await _context.Owners.FirstOrDefaultAsync(o => o.Email == email && o.Password == password);
        }

        public async Task<Owner> GetOwner(int id)
        {
            return await _context.Owners.FindAsync(id);
        }
    }
}
