using Akavache;
using Newtonsoft.Json;
using WMC.Models;
using WMC.Repositories;
using WMC.Services;
using WMC.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Reactive.Linq;

namespace WMC
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            BlobCache.ApplicationName = "WMC";
            BlobCache.EnsureInitialized();
            DependencyService.Register<WarehouseProductsService>();
            DependencyService.Register<AuthorisationRepository>();
            DependencyService.Register<AuthenticationService>();

            MainPage = new LoginChooserPage();
        }

        protected override void OnStart()
        {
            var tokenContentTask = SecureStorage.GetAsync(Constants.StorageToken);
            tokenContentTask.Wait();
            
            var tokenContent = tokenContentTask.Result;

            if (!string.IsNullOrWhiteSpace(tokenContent))
            {
                var tokenUnsafe = JsonConvert.DeserializeObject<WmcTokenUnsafe>(tokenContent);
                var token = WmcToken.CreateToken(tokenUnsafe.Token, tokenUnsafe.RefreshToken, tokenUnsafe.ExpirationDate);

                var userInfoTask = SecureStorage.GetAsync(Constants.StorageUserInfo);
                userInfoTask.Wait();

                var userInfoContent = userInfoTask.Result;

                var userInfo = string.IsNullOrWhiteSpace(userInfoContent) ? null : JsonConvert.DeserializeObject<UserInfo>(userInfoContent);

                DependencyService.Get<IAuthenticationService>().SetupAuthenticationData(token, userInfo);
                MainPage = new AppShell();
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
