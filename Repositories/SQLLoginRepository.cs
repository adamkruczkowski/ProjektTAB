using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjektTabAPI.Data;
using ProjektTabAPI.Entities.Domain;

namespace ProjektTabAPI.Repositories
{
    public class SQLLoginRepository : ILoginRepository
    {
        private readonly PolBankDbContext dbContext;
        public SQLLoginRepository(PolBankDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> Create(Login login)
        {
            await dbContext.Logins.AddAsync(login);
            return await dbContext.SaveChangesAsync();
            
        }
    }
}
