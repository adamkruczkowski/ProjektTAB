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

        public async Task<Client?> GetClientByLogin(string login)
        {
            var foundClient = await dbContext.Clients.FirstOrDefaultAsync(c => c.Login == login);
            if (foundClient is null)
            {
                return null;
            }
            return foundClient;
        }
    }
}
