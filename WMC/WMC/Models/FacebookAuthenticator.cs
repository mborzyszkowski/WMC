using System.Threading.Tasks;
using WMC.Repositories;
using Xamarin.Forms;

namespace WMC.Models
{
    public class FacebookAuthenticator : IAuthenticator
    {
        private readonly string _facebookToken;
        private readonly IAuthorisationRepository _authorisationRepository = 
            DependencyService.Get<IAuthorisationRepository>();

        public FacebookAuthenticator(string authenticationToken) => 
            _facebookToken = authenticationToken;

        public async Task<WmcToken> GetAuthenticationToken() => 
            await _authorisationRepository.AuthenticateWithFacebook(_facebookToken);
    }
}