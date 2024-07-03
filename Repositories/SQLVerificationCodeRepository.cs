using Microsoft.EntityFrameworkCore;
using ProjektTabAPI.Data;
using ProjektTabAPI.Entities.Domain;
using System;
using System.Threading.Tasks;

namespace ProjektTabAPI.Repositories
{
    public class SQLVerificationCodeRepository : IVerificationCodeRepository
    {
        private readonly PolBankDbContext _dbContext;

        public SQLVerificationCodeRepository(PolBankDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveVerificationCode(Guid clientId, string code)
        {
            var expirationTime = DateTime.Now.AddMinutes(10); // Code valid for 10 minutes
            var verificationCode = new VerificationCode { ClientId = clientId, Code = code, ExpirationTime = expirationTime };

            _dbContext.VerificationCodes.Add(verificationCode);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<string?> GetVerificationCode(Guid clientId)
        {
            var verificationCode = await _dbContext.VerificationCodes
                .FirstOrDefaultAsync(vc => vc.ClientId == clientId && vc.ExpirationTime > DateTime.Now);

            return verificationCode?.Code;
        }

    }
}
