using Microsoft.EntityFrameworkCore;
using ProjektTabAPI.Data;
using ProjektTabAPI.Entities.Domain;
using ProjektTabAPI.Entities.Dtos.Transaction;

namespace ProjektTabAPI.Repositories
{
    public class SQLTransactionRepository : ITransactionRepository
    {
        private readonly PolBankDbContext dbContext;
        public SQLTransactionRepository(PolBankDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<Transaction>> GetAll()
        {
            return await dbContext.Transactions.Include(t => t.Sender).ThenInclude(ba => ba.Client).Include(t => t.Recipient).ThenInclude(ba => ba.Client).ToListAsync();
        }

        public async Task<Transaction?> GetById(Guid id)
        {
            return await dbContext.Transactions.Include(t => t.Sender).ThenInclude(ba => ba.Client).Include(t => t.Recipient).ThenInclude(ba => ba.Client).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Transaction> Create(Transaction transaction)
        {
            await dbContext.Transactions.AddAsync(transaction);
            await dbContext.SaveChangesAsync();
            return transaction;
        }
    }
}
