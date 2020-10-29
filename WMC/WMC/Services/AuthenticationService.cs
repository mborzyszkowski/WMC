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
        private UserInfo _userInfo;
        private readonly IAuthorisationRepository _authorisationRepository = DependencyService.Get<IAuthorisationRepository>();
        private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);

        public bool IsManager() => _userInfo != null && _userInfo.Roles.Contains("manager");

        public bool IsEmployee() => _userInfo != null && _userInfo.Roles.Contains("employee");

        public string GetUserName() => _userInfo != null ? _userInfo.Username : "";

        public void SetupAuthenticationData(WmcToken token, UserInfo userInfo)
        {
            _token = token;
            _userInfo = userInfo;
        }

        public void Logout()
        {
            _token = null;
            _userInfo = null;
            var refreshTask = RefreshStoredData();
            refreshTask.Wait();
        }

        public async Task AuthenticateWithFacebook(string authenticationToken)
        {
            await Semaphore.WaitAsync();
            try
            {
                _token = await _authorisationRepository.AuthenticateWithFacebook(authenticationToken);
                await RefreshUserInfo();
                await RefreshStoredData();
            }
            finally
            {
                Semaphore.Release();
            }
        }

        public async Task AuthenticateWithWmc(string userName, string password)
        {
            await Semaphore.WaitAsync();
            try
            {
                _token = await _authorisationRepository.AuthenticateWithWmc(userName, password);
                await RefreshUserInfo();
                await RefreshStoredData();
            }
            finally
            {
                Semaphore.Release();
            }
        }

        public async Task<WmcToken> GetToken()
        {
            if (_token == null || !_token.IsTokenExpired) 
                return _token?.GetTokenCopy();

            await Semaphore.WaitAsync();
            try
            {
                if (_token != null && _token.IsTokenExpired)
                {
                    _token = await _authorisationRepository.RefreshToken(_token.GetTokenCopy());
                    await RefreshUserInfo();
                    await RefreshStoredData();
                }
            }
            finally
            {
                Semaphore.Release();
            }

            return _token?.GetTokenCopy();
        }

        private async Task RefreshUserInfo()
        {
            if (_token != null)
            {
                _userInfo = await _authorisationRepository.GetUserInfo(_token.GetTokenCopy());
            }
            else
            {
                _userInfo = null;
            }
        }

        private async Task RefreshStoredData()
        {
            if (_token != null)
            {
                var serializedToken = JsonConvert.SerializeObject(_token.GetUnsafeCopy());
                await SecureStorage.SetAsync(Constants.StorageToken, serializedToken);
                var serializedRoles = JsonConvert.SerializeObject(_userInfo);
                await SecureStorage.SetAsync(Constants.StorageUserInfo, serializedRoles);
            }
            else
            {
                SecureStorage.Remove(Constants.StorageToken);
                SecureStorage.Remove(Constants.StorageUserInfo);
            }
        }
    }
}
