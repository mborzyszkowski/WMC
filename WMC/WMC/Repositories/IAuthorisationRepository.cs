using System.Threading.Tasks;
using WMC.Models;

namespace WMC.Repositories
{
    public interface IAuthorisationRepository
    {
        Task<WmcToken> AuthenticateWithFacebook(string facebookToken);
        Task<WmcToken> AuthenticateWithWmc(string name, string password);
    }
}
