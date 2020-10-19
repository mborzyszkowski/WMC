using WMC.Repositories;
using WMC.Services;
using WMC.Views;
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

            var authRepo = DependencyService.Get<IAuthenticationService>();

            MainPage = new LoginChooserPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
