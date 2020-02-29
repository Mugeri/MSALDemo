using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MSALDemo
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            string ClientId = "";
            string Tenant = "";
            string[] scopes = new string[] { "User.Read"};

            // Create an application to call apis
            var publicClientApplication = PublicClientApplicationBuilder.Create(ClientId)
                .WithRedirectUri("http://localhost:1234")
                .WithAuthority(AzureCloudInstance.AzurePublic, Tenant)
                .Build();

            // get the token
            var authResult = await publicClientApplication.AcquireTokenInteractive(scopes)
                .ExecuteAsync();
            var token = authResult.AccessToken;

            // Try sending a message across with the Auth header set
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me/");
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            // Print out response
            var response = await httpResponseMessage.Content.ReadAsStringAsync();
            string jsonFormatted = JValue.Parse(response).ToString(Formatting.Indented);
            Console.WriteLine(jsonFormatted);
        }
    }
}
