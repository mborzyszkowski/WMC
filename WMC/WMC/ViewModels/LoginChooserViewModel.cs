using WMC.Views;
using Xamarin.Forms;

namespace WMC.ViewModels
{
    class LoginChooserViewModel : BaseViewModel
    {
        public LoginChooserViewModel()
        {
            LoginWithFacebookCommand = new Command(() => Application.Current.MainPage = new FacebookLoginPage());
            LoginWithWmcCommand = new Command(() => Application.Current.MainPage = new WmcLoginPage());
        }

        public Command LoginWithFacebookCommand { get; }
        public Command LoginWithWmcCommand { get; }
    }
}
