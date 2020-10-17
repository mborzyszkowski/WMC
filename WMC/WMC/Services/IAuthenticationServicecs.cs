using System.Threading.Tasks;
using WMC.Models;

namespace WMC.Services
{
    public interface IAuthenticationService
    {
        bool IsManager();
        bool IsEmployee();
        void Logout();
        void AuthenticateWithFacebook(string authenticationToken);
        void AuthenticateWithWmc(string userName, string password);
        Task<WmcToken> GetToken();
    }
}
