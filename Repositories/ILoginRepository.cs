using ProjektTabAPI.Entities.Domain;

namespace ProjektTabAPI.Repositories
{
    public interface ILoginRepository
    {


        public Task<int> Create(Login login);
    }
}
