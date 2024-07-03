using ProjektTabAPI.Entities.Domain;
using System.Threading.Tasks;

namespace ProjektTabAPI.Repositories
{
    public interface IVerificationCodeRepository
    {
        Task SaveVerificationCode(Guid clientId, string code);
        Task<string?> GetVerificationCode(Guid clientId);
    }
}
