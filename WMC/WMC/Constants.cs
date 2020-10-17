using System;
using System.Collections.Generic;
using System.Text;

namespace WMC
{
    public static class Constants
    {
        // API
        public static string ApiEndpointForAndroid = "https://192.168.2.122:44349";
        public static string ApiEndpointForIos = "https://localhost:44349";

        // Google OAuth
        public static string FacebookAndroidClientId = "781198405780872";
        public static string FacebookAndroidRedirectUrl = "https://www.facebook.com/connect/login_success.html";

        public static string GetFacebookLoginUrl() =>
            $"https://www.facebook.com/v7.0/dialog/oauth?client_id={FacebookAndroidClientId}&response_type=token&redirect_uri={FacebookAndroidRedirectUrl}";

        public static string ExstractToken(string resUrl) =>
            resUrl.Replace("https://www.facebook.com/connect/login_success.html#access_token=", String.Empty).Split('&')[0];
    }
}
