using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

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
            await _semaphore.WaitAsync();
            try
            {
                _token = await _authorisationRepository.AuthenticateWithFacebook(authenticationToken);
                await RefreshRoles();
                await RefreshStoredData();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task AuthenticateWithWmc(string userName, string password)
        {
            await _semaphore.WaitAsync();
            try
            {
                _token = await _authorisationRepository.AuthenticateWithWmc(userName, password);
                await RefreshRoles();
                await RefreshStoredData();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<WmcToken> GetToken()
        {
            if (_token == null || !_token.IsTokenExpired) 
                return _token?.GetTokenCopy();

            await _semaphore.WaitAsync();
            try
            {
                if (_token != null && _token.IsTokenExpired)
                {
                    _token = await _authorisationRepository.RefreshToken(_token.GetTokenCopy());
                    await RefreshRoles();
                    await RefreshStoredData();
                }
            }
            finally
            {
                _semaphore.Release();
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
