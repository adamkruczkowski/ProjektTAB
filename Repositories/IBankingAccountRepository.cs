using ProjektTabAPI.Entities.Domain;

namespace ProjektTabAPI.Repositories
{
    public interface IBankingAccountRepository
    {
        Task<List<BankingAccount>> GetAllFromUserId(Guid id);
        Task<BankingAccount?> GetById(Guid id);
        Task<BankingAccount> Create(BankingAccount addBankingAccount);

    }
}
