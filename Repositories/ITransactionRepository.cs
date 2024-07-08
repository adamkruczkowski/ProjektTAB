using ProjektTabAPI.Entities.Domain;
using ProjektTabAPI.Entities.Dtos.Transaction;

namespace ProjektTabAPI.Repositories
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAll();
        Task<Transaction?> GetById(Guid id);
        Task<Transaction> Create(Transaction transaction);
        Task<List<Transaction>> GetAllByBAId(Guid BA_id);
        Task<List<Transaction>> GetSentByBAId(Guid BA_id);
        Task<List<Transaction>> GetReceivedByBAId(Guid BA_id);

        Task<int> DoTransfer(BankingAccount sender, BankingAccount reciepier, decimal amount);
    }
}
