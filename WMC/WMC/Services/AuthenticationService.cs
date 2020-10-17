using System.Threading.Tasks;
using WMC.Models;

namespace WMC.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private IAuthenticator _authenticator;
        private WmcToken _token;

        public void AuthenticateWithFacebook(string authenticationToken) => 
            _authenticator = new FacebookAuthenticator(authenticationToken);

        public void AuthenticateWithWmc(string userName, string password) => 
            _authenticator = new WmcAuthenticator(userName, password);

        public async Task<WmcToken> GetToken()
        {
            if (_token == null || _token.IsTokenExpired)
            {
                _token = await _authenticator.GetAuthenticationToken();
            }

            return _token.GeTokenCopy();
        }
    }
}
