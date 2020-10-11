using System.Net.Http;

namespace WMC.Services
{
    public interface IHttpClientHandlerService
    {
        HttpClientHandler GetInsecureHandler();
    }
}
