using System.Collections.Generic;
using System.Threading.Tasks;

namespace WMC.Models
{
    public interface IAuthenticator
    {
        Task<WmcToken> GetAuthenticationToken();
        Task<IEnumerable<string>> GetUserRoles(WmcToken token);
    }
}
