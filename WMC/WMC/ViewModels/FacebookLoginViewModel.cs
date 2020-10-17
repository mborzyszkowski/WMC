using WMC.Views;
using Xamarin.Forms;

namespace WMC.ViewModels
{
    class FacebookLoginViewModel : BaseViewModel
    {
        private readonly WebView _webView;

        public FacebookLoginViewModel(WebView webView)
        {
            _webView = webView;
            _webView.Source = Constants.GetFacebookLoginUrl();
            _webView.Navigated += OnNavigated;

            CancelCommand = new Command(() => Application.Current.MainPage = new LoginChooserPage());
        }

        public Command CancelCommand { get; }

        private async void OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            var AccessURL = e.Url;

            if (AccessURL.Contains("access_token"))
            {
                var accessToken = Constants.ExstractToken(AccessURL);

                _webView.Navigated -= OnNavigated;

                AuthenticationService.AuthenticateWithFacebook(accessToken);

                var tokent = await AuthenticationService.GetToken();

                // TODO: unauthorise or no internet connection

                Application.Current.MainPage = new AppShell();
            }
        }
    }
}
