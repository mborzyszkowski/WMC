using WMC.Views;
using Xamarin.Forms;

namespace WMC.ViewModels
{
    class WmcLoginViewModel : BaseViewModel
    {
        private string _name;
        private string _password;

        public WmcLoginViewModel()
        {
            CancelCommand = new Command(() => Application.Current.MainPage = new LoginChooserPage());
            LoginCommand = new Command(OnLogin, CanExecuteLogin);
        }

        public Command CancelCommand { get; }
        public Command LoginCommand { get; }

        private bool CanExecuteLogin() => 
            string.IsNullOrWhiteSpace(_name) && string.IsNullOrWhiteSpace(_password);

        public async void OnLogin()
        {
            AuthenticationService.AuthenticateWithWmc(_name, _password);
            if (await AuthenticationService.GetToken() != null)
            {
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                await Application.Current.MainPage
                    .DisplayAlert("Login error", "Incorrect login credentials", "Ok");
            }
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
    }
}
