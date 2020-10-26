using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMC.Models;
using WMC.Repositories;
using Xamarin.Forms;

namespace WMC.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private WmcToken _token;
        private IEnumerable<string> _userRoles = new List<string>();
        private readonly IAuthorisationRepository _authorisationRepository = DependencyService.Get<IAuthorisationRepository>();

        public bool IsManager() => _userRoles.Contains("manager");

        public bool IsEmployee() => _userRoles.Contains("employee");

        public void Logout()
        {
            _token = null;
            _userRoles = new List<string>();
        }

        public async Task AuthenticateWithFacebook(string authenticationToken)
        {
            _token = await _authorisationRepository.AuthenticateWithFacebook(authenticationToken);
            await RefreshRoles();
        }

        public async Task AuthenticateWithWmc(string userName, string password)
        {
            _token = await _authorisationRepository.AuthenticateWithWmc(userName, password);
            await RefreshRoles();
        }

        public async Task<WmcToken> GetToken()
        {
            if (_token != null && _token.IsTokenExpired)
            {
                _token = await _authorisationRepository.RefreshToken(_token.GeTokenCopy());
                await RefreshRoles();
            }

            return _token?.GeTokenCopy();
        }

        private async Task RefreshRoles()
        {
            if (_token != null)
            {
                _userRoles = await _authorisationRepository.GetUserRoles(_token.GeTokenCopy());
            }
            else
            {
                _userRoles = new List<string>();
            }
        }
    }
}
