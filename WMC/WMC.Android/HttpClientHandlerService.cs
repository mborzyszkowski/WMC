using System.Net.Http;
using WMC.Droid;
using WMC.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(HttpClientHandlerService))]
namespace WMC.Droid
{
    class HttpClientHandlerService : IHttpClientHandlerService
    {
        public HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }
    }
}