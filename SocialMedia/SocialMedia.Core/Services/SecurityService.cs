using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly IUnitOfWork unitOfWork;

        public SecurityService(IUnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;
        }

        public async Task<Security> GetLoginByCredentials(UserLogin userLogin)
        {
            return await this.unitOfWork.SecurityRepository.GetLoginByCredentials(userLogin);
        }

        public async Task RegisterUser(Security security)
        {
            await this.unitOfWork.SecurityRepository.Add(security);
            await this.unitOfWork.SaveChangesAsync();
        }
    }
}
