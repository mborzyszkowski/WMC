using static System.String;

namespace WMC
{
    public static class Constants
    {
        // API
        public static string ApiEndpointForAndroid = "http://192.168.2.122:8080";
        public static string ApiEndpointForIos = "http://localhost:8080";

        // Google OAuth
        public static string FacebookAndroidClientId = "781198405780872";
        public static string FacebookAndroidRedirectUrl = "https://www.facebook.com/connect/login_success.html";

        public static string GetFacebookLoginUrl() =>
            $"https://www.facebook.com/v7.0/dialog/oauth?client_id={FacebookAndroidClientId}&response_type=token&redirect_uri={FacebookAndroidRedirectUrl}";

        public static string ExstractToken(string resUrl) =>
            resUrl.Replace("https://www.facebook.com/connect/login_success.html#access_token=", Empty).Split('&')[0];
    }
}
