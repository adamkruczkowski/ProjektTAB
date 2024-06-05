using ProjektTabAPI.Entities.Domain;

namespace ProjektTabAPI.Repositories
{
    public interface IClientRepository
    {
        Task<Client?> GetClientByLogin(string login);
    }
}
