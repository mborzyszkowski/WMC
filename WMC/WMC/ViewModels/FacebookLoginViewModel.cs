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
            var accessUrl = e.Url;

            if (!accessUrl.Contains("access_token"))
            {
                return;
            }

            var accessToken = Constants.ExstractToken(accessUrl);

            _webView.Navigated -= OnNavigated;

            await AuthenticationService.AuthenticateWithFacebook(accessToken);

            Application.Current.MainPage = new AppShell();
        }
    }
}
