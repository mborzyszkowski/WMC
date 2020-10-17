using System.Collections.Generic;
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

        public async Task<IEnumerable<string>> GetUserRoles(WmcToken token) =>
            await _authorisationRepository.GetUserRoles(token);
    }
}