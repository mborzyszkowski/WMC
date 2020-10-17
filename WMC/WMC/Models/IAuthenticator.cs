using System.Collections.Generic;
using System.Threading.Tasks;
using WMC.Services;

namespace WMC.Models
{
    public interface IAuthenticator
    {
        Task<WmcToken> GetAuthenticationToken();
        Task<IEnumerable<string>> GetUserRoles(WmcToken token);
    }
}
