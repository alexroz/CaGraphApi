using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace GraphApiDemo
{
    class Program
    {
        static string graphUrl = "https://graph.windows.net";
        static string loginUrl = "https://login.windows.net/myAD.onmicrosoft.com";
        static string tenantId = "94effaa8-8e58-451a-b107-03a5da9c0422";
        static string clientId = "64642343-c74d-4506-b164-71348f248bcd"; // client identifier
        static string clientSecret = "SECRET KEY"; // client secret key to access AD

        static void Main(string[] args)
        {
            string root = graphUrl + "/" +tenantId;
            var client = new ActiveDirectoryClient(new Uri(root), async () => await GetAuthToken());

            QueryDirectory(client);
        }

        private static void QueryDirectory(ActiveDirectoryClient client)
        {
            List<IUser> users = client.Users
                .Where(u => u.Country != null && u.Country.Equals("UK"))
                .Take(2)
                .ExecuteAsync()
                .Result.CurrentPage.ToList();
        }

        private static async Task<string> GetAuthToken()
        {
            AuthenticationContext authContext = new AuthenticationContext(loginUrl, false);
            ClientCredential creds = new ClientCredential(clientId, clientSecret);
            var results = await authContext.AcquireTokenAsync(graphUrl, creds);

            return results.AccessToken;
        }
    }
}
