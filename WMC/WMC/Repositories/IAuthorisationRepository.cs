using System.Collections.Generic;
using System.Threading.Tasks;
using WMC.Models;

namespace WMC.Repositories
{
    public interface IAuthorisationRepository
    {
        Task<WmcToken> AuthenticateWithFacebook(string facebookToken);
        Task<WmcToken> AuthenticateWithWmc(string name, string password);
        Task<IEnumerable<string>> GetUserRoles(WmcToken token);
    }
}
