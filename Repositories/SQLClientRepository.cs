using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjektTabAPI.Data;
using ProjektTabAPI.Entities.Domain;

namespace ProjektTabAPI.Repositories
{
    public class SQLClientRepository : IClientRepository
    {
        private readonly PolBankDbContext dbContext;
        public SQLClientRepository(PolBankDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Client?> GetClientById(Guid id)
        {
            var foundClient = await dbContext.Clients.Include(c => c.BankingAccounts).Include(c => c.Logins).FirstOrDefaultAsync(c => c.Id == id);
            if (foundClient is null)
            {
                return null;
            }
            return foundClient;
        }
        public async Task<Client?> GetClientByLogin(string login)
        {
            var foundClient = await dbContext.Clients.FirstOrDefaultAsync(c => c.Login == login);
            if (foundClient is null)
            {
                return null;
            }
            return foundClient;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();
        }
    }
}
