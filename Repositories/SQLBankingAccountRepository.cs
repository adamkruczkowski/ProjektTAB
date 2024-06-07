using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjektTabAPI.Data;
using ProjektTabAPI.Entities.Domain;

namespace ProjektTabAPI.Repositories
{
    public class SQLBankingAccountRepository : IBankingAccountRepository
    {
        private readonly PolBankDbContext dbContext;
        public SQLBankingAccountRepository(PolBankDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<BankingAccount> Create(BankingAccount addBankingAccount)
        {
            await dbContext.BankingAccounts.AddAsync(addBankingAccount);
            await dbContext.SaveChangesAsync();
            return addBankingAccount;
        }

        public async Task<List<BankingAccount>> GetAllFromUserId(Guid id)
        {
            return await dbContext.BankingAccounts.Include(ba => ba.Client).Where(ba => ba.Id_client == id).ToListAsync();
        }

        public async Task<BankingAccount?> GetById(Guid id)
        {
            return await dbContext.BankingAccounts.Include(ba => ba.Client).FirstOrDefaultAsync(ba => ba.Id == id);
        }
    }
}
