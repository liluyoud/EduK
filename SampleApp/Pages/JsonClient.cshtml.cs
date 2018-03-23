using System.Threading.Tasks;
using System.Net.Http;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;

namespace SampleApp.Pages
{
    public class JsonClientModel : PageModel
    {
        public string Texto { get; set; }

        public async Task OnGetAsync()
        {
            var tokenClient = new TokenClient("http://localhost:5000/connect/token", "edukclient", "EduK$Client");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("edukapi");

            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var content = await client.GetStringAsync("http://localhost:5001/identity");

            Texto = JArray.Parse(content).ToString();
        }

    }
}