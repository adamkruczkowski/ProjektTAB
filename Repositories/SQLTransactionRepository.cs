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

        public async Task<int> DoTransfer(BankingAccount sender, BankingAccount recipier, decimal amount)
        {
            sender.Amount -= amount;
            recipier.Amount += amount;
            return await dbContext.SaveChangesAsync();
        }
        public async Task<List<Transaction>> GetAllByBAId(Guid BA_id)
        {
            return await dbContext.Transactions.Where(t => t.Sender_BAId == BA_id || t.Recipient_BAId == BA_id).Include(t => t.Sender).ThenInclude(ba => ba.Client).Include(t => t.Recipient).ThenInclude(ba => ba.Client).ToListAsync();
        }

        public async Task<List<Transaction>> GetSentByBAId(Guid BA_id)
        {
            return await dbContext.Transactions.Where(t => t.Sender_BAId == BA_id).Include(t => t.Sender).ThenInclude(ba => ba.Client).Include(t => t.Recipient).ThenInclude(ba => ba.Client).ToListAsync();
        }

        public async Task<List<Transaction>> GetReceivedByBAId(Guid BA_id)
        {
            return await dbContext.Transactions.Where(t => t.Recipient_BAId == BA_id).Include(t => t.Sender).ThenInclude(ba => ba.Client).Include(t => t.Recipient).ThenInclude(ba => ba.Client).ToListAsync();
        }
    }
}
