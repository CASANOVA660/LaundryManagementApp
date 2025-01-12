using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using LaundryManagement.API.Interfaces;
using LaundryManagement.Infrastructure.Models;
using LaundryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LaundryManagement.API.Services
{
    public class ClientService : IClientService
    {
        private readonly LaundryDbContext _context;

        public ClientService(LaundryDbContext context)
        {
            _context = context;
        }

        public async Task<Client> GetClient(int id)
        {
            return await _context.Clients.FindAsync(id);
        }

        public async Task<IEnumerable<Client>> GetAllClients()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<Client> AddClient(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<bool> UpdateClient(Client client)
        {
            if (client == null)
                return false;

            _context.Entry(client).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ClientExists(client.Id))
                    return false;
                throw;
            }
        }

        public async Task<bool> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
                return false;

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> ClientExists(int id)
        {
            return await _context.Clients.AnyAsync(e => e.Id == id);
        }
    }
}
