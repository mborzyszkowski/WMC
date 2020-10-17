// using System;
// using Android.App;
// using Android.Content;
// using Android.Content.PM;
// using Android.OS;
// using Xamarin.Auth;
//
// namespace WMC.Droid
// {
//     [Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
//     [IntentFilter(
//         new[] { Intent.ActionView },
//         Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
//         DataSchemes = new[] { "com.googleusercontent.apps.509096001030-1dg0g13cfud32sq8il02ailfks3jh9rt" },
//         DataPath = "/oauth2redirect")]
// 	class CustomUrlSchemeInterceptorActivity : Activity
//     {
//         protected override void OnCreate(Bundle savedInstanceState)
//         {
//             base.OnCreate(savedInstanceState);
//
//             // Convert Android.Net.Url to Uri
//             var uri = new Uri(Intent.Data.ToString());
//
//             // Load redirectUrl page
//             AuthenticationState.Authenticator.OnPageLoading(uri);
//             var intent = new Intent(this, typeof(MainActivity));
//             intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
//             StartActivity(intent);
//
//             Finish();
//         }
//     }
//
//     public class AuthenticationState
//     {
//         public static OAuth2Authenticator Authenticator;
//     }
// }