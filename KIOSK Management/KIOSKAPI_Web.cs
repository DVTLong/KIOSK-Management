using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace KIOSK_Management
{
    public class KIOSKAPI_Web
    {
        private static string uri = "https://localhost:44370";

        public static string Uri { get => uri; set => uri = value; }

        public static async Task<bool> ClearAllTokensAsync(string password)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Uri);
                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("password", password)
            });
                var result = await client.PostAsync("/api/ClearAllTokens", content);
                switch (result.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        return false;
                        break;
                    case HttpStatusCode.InternalServerError:
                        return false;
                        break;
                    case HttpStatusCode.OK:
                        return true;
                        break;
                }

            }
            return false;
        }

    }
}