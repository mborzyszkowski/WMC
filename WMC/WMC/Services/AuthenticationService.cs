using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WMC.Models;
using WMC.Repositories;
using Xamarin.Essentials;
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
        public void SetupToken(WmcToken token, List<string> list)
        {
            _token = token;
            _userRoles = list;
        }

        public void Logout()
        {
            _token = null;
            _userRoles = new List<string>();
        }

        public async Task AuthenticateWithFacebook(string authenticationToken)
        {
            _token = await _authorisationRepository.AuthenticateWithFacebook(authenticationToken);
            await RefreshStoredData();
            await RefreshRoles();
        }

        public async Task AuthenticateWithWmc(string userName, string password)
        {
            _token = await _authorisationRepository.AuthenticateWithWmc(userName, password);
            await RefreshStoredData();
            await RefreshRoles();
        }

        public async Task<WmcToken> GetToken()
        {
            if (_token != null && _token.IsTokenExpired)
            {
                _token = await _authorisationRepository.RefreshToken(_token.GetTokenCopy());
                await RefreshRoles();
                await RefreshStoredData();
            }

            return _token?.GetTokenCopy();
        }

        private async Task RefreshRoles()
        {
            if (_token != null)
            {
                _userRoles = await _authorisationRepository.GetUserRoles(_token.GetTokenCopy());
            }
            else
            {
                _userRoles = new List<string>();
            }
        }

        private async Task RefreshStoredData()
        {
            if (_token != null)
            {
                var serializedToken = JsonConvert.SerializeObject(_token.GetUnsafeCopy());
                await SecureStorage.SetAsync("token", serializedToken);
                var serializedRoles = JsonConvert.SerializeObject(_userRoles);
                await SecureStorage.SetAsync("roles", serializedRoles);
            }
            else
            {
                SecureStorage.Remove("token");
                SecureStorage.Remove("roles");
            }
        }
    }
}
