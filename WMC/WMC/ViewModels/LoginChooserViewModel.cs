using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using WMC.Models;
using Xamarin.Auth;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WMC.ViewModels
{
    class LoginChooserViewModel : BaseViewModel
    {
        private readonly WebView _webView;

        public LoginChooserViewModel(WebView webView)
        {
            _webView = webView;
            FacebookLoginCommand = new Command(OnFacebookLogin);
            WmcLoginCommand = new Command(() => {});
        }

        public Command FacebookLoginCommand { get; }
        public Command WmcLoginCommand { get; }

        public void OnFacebookLogin()
        {
            _webView.Source = Constants.GetFacebookLoginUrl();
            _webView.Navigated += OnNavigated;
        }

        private void OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            var AccessURL = e.Url;

            if (AccessURL.Contains("access_token"))
            {
                var accessToken = Constants.ExstractToken(AccessURL);
                
                // TODO: check if token expire + build service for login to api
                //SecureStorage.SetAsync("accessToken", accessToken);

                // TODO: Add to api
                // HttpClient client = new HttpClient();
                // var task = client.GetStringAsync("https://graph.facebook.com/me?fields=id,name&access_token=" + accessToken);
                // task.Wait();
                //
                // var result = task.Result;
                //
                // var User = JsonConvert.DeserializeObject<FacebookUser>(result);

                _webView.Navigated -= OnNavigated;
            }
        }
    }
}
