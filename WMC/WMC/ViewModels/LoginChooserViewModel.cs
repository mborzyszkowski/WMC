using WMC.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WMC.ViewModels
{
    class LoginChooserViewModel : BaseViewModel
    {
        public LoginChooserViewModel()
        {
            LoginWithFacebookCommand = new Command(OnLoginWithFacebook);
            LoginWithWmcCommand = new Command(OnLoginWithWmc);
        }

        private void OnLoginWithFacebook()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Application.Current.MainPage = new FacebookLoginPage();
            }
            else
            {
                Application.Current.MainPage
                    .DisplayAlert("Connection", "No internet connection", "Ok");
            }
        }

        private void OnLoginWithWmc()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Application.Current.MainPage = new WmcLoginPage();
            }
            else
            {
                Application.Current.MainPage
                    .DisplayAlert("Connection", "No internet connection", "Ok");
            }
        }

        public Command LoginWithFacebookCommand { get; }
        public Command LoginWithWmcCommand { get; }
    }
}
