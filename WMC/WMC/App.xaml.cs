using System.Collections.Generic;
using Newtonsoft.Json;
using WMC.Models;
using WMC.Repositories;
using WMC.Services;
using WMC.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WMC
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<WarehouseProductsService>();
            DependencyService.Register<AuthorisationRepository>();
            DependencyService.Register<AuthenticationService>();

            MainPage = new LoginChooserPage();
        }

        protected override void OnStart()
        {
            var tokenContentTask = SecureStorage.GetAsync("token");
            tokenContentTask.Wait();
            
            var tokenContent = tokenContentTask.Result;

            if (!string.IsNullOrWhiteSpace(tokenContent))
            {
                var tokenUnsafe = JsonConvert.DeserializeObject<WmcTokenUnsafe>(tokenContent);
                var token = WmcToken.CreateToken(tokenUnsafe.Token, tokenUnsafe.RefreshToken, tokenUnsafe.ExpirationDate);

                var rolesTask = SecureStorage.GetAsync("roles");
                rolesTask.Wait();

                var rolesContent = rolesTask.Result;

                var roles = string.IsNullOrWhiteSpace(rolesContent) 
                    ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(rolesContent);

                DependencyService.Get<IAuthenticationService>().SetupToken(token, roles);
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
