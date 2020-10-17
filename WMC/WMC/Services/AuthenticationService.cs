using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMC.Models;

namespace WMC.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private IAuthenticator _authenticator;
        private WmcToken _token;
        private IEnumerable<string> _userRoles = new List<string>();

        public bool IsManager() => _userRoles.Contains("manager");

        public bool IsEmployee() => _userRoles.Contains("employee");

        public void Logout()
        {
            _authenticator = null;
            _token = null;
            _userRoles = new List<string>();
        }

        public void AuthenticateWithFacebook(string authenticationToken) => 
            _authenticator = new FacebookAuthenticator(authenticationToken);

        public void AuthenticateWithWmc(string userName, string password) => 
            _authenticator = new WmcAuthenticator(userName, password);

        public async Task<WmcToken> GetToken()
        {
            if (_token == null || _token.IsTokenExpired)
            {
                _token = await _authenticator.GetAuthenticationToken();
                _userRoles = _token != null ? await _authenticator.GetUserRoles(_token.GeTokenCopy()) : new List<string>();
            }

            return _token != null ? _token.GeTokenCopy() : null;
        }
    }
}
