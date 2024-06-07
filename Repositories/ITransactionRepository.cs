using ProjektTabAPI.Entities.Domain;
using ProjektTabAPI.Entities.Dtos.Transaction;

namespace ProjektTabAPI.Repositories
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAll();
        Task<Transaction?> GetById(Guid id);
        Task<Transaction> Create(Transaction transaction);
    }
}
