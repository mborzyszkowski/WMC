using System.Threading.Tasks;
using WMC.Repositories;
using Xamarin.Forms;

namespace WMC.Models
{
    public class WmcAuthenticator : IAuthenticator
    {
        private readonly string _userName;
        private readonly string _password;
        private readonly IAuthorisationRepository _authorisationRepository = DependencyService.Get<IAuthorisationRepository>();

        public WmcAuthenticator(string userName, string password)
        {
            _userName = userName;
            _password = password;
        }

        public async Task<WmcToken> GetAuthenticationToken() => 
            await _authorisationRepository.AuthenticateWithWmc(_userName, _password);
    }
}