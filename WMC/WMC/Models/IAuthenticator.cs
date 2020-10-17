using System.Threading.Tasks;
using WMC.Services;

namespace WMC.Models
{
    public interface IAuthenticator
    {
        Task<WmcToken> GetAuthenticationToken();
    }
}
