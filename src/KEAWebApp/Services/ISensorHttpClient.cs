using System.Net.Http;
using System.Threading.Tasks;

namespace KEAWebApp.Services
{
    public interface ISensorHttpClient
    {
        Task<HttpClient> GetClient();
    }
}
