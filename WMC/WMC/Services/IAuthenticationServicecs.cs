using System.Collections.Generic;
using System.Threading.Tasks;
using WMC.Models;

namespace WMC.Services
{
    public interface IAuthenticationService
    {
        bool IsManager();
        bool IsEmployee();
        string GetUserName();
        void SetupAuthenticationData(WmcToken token, UserInfo userInfo);
        void Logout();
        Task AuthenticateWithFacebook(string authenticationToken);
        Task AuthenticateWithWmc(string userName, string password);
        Task<WmcToken> GetToken();
    }
}
