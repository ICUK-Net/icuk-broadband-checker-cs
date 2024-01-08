using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace IcukBroadbandCheckerCs {
    public class BroadbandAvailabilityCheckerProxy {
        public static string Url = "https://api.interdns.co.uk";
        public HttpClient HttpClient = new HttpClient();

        private string Username;
        private string Password;
        private OAuthResponse Token;

        public BroadbandAvailabilityCheckerProxy(string Username, string Password) {
            this.Username = Username;
            this.Password = Password;
            HttpClient.DefaultRequestHeaders.Add("APIPlatform", "live");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /*
         * If token expired or nonexistant then request a new one
         */
        public void UpdateAuth() {
            if (Token == null) {
                Authorize();
                return;
            }

            if (Token.ExpireTime >= DateTime.Now) {
                Authorize();
            }
        }

        /*
         * General function to combine all the others based on a single input.
         * Used since developers have to hook up this function to form inputs and 
         * we want difficulty of usage very low.
         */
        public string Api(string data) {
            if (data.StartsWith("$") && Validation.IsValidPostcode(data.TrimStart('$'))) {
                return AvailabilityGet(data.TrimStart('$'));
            }

            if (Validation.IsValidPostcode(data) ||
                Validation.IsValidPhonenumber(data)) {
                return AvailabilityGet(data);
            }

            return AvailabilityPost(data);
        }

        /*
         * Get availability data with base64 encoded address json data
         */
        public string AvailabilityPost(string base64Address) {
            string availabilityEndpoint = Url + "/broadband/availability/";
            string jsonAddress = Encoding.UTF8.GetString(Convert.FromBase64String(base64Address));

            UpdateAuth();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, availabilityEndpoint);
            request.Content = new StringContent(jsonAddress, Encoding.UTF8, "application/json");
            HttpResponseMessage response = HttpClient.Send(request);

            Stream stream = response.Content.ReadAsStream();
            StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            return json;
        }

        /*
         * Get availability data from a phone number or postcode
         */
        public string AvailabilityGet(string cliOrPostcode) {
            string availabilityEndpoint = Url + "/broadband/availability/" + cliOrPostcode.Replace(" ", "");

            UpdateAuth();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, availabilityEndpoint);
            HttpResponseMessage response = HttpClient.Send(request);

            Stream stream = response.Content.ReadAsStream();
            StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            return json;
        }

        /*
         * Get Oauth token using basic auth
         */
        public void Authorize() {
            string oauthEndpoint = Url + "/oauth/token";
            string basicToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(Username + ":" + Password));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, oauthEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicToken);
            HttpResponseMessage response = HttpClient.Send(request);

            Stream stream = response.Content.ReadAsStream();
            StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();

            JObject o = JObject.Parse(json);
            Token = new OAuthResponse();
            Token.AccessToken = (string)o["access_token"];
            Token.TokenType = (string)o["token_type"];
            Token.ExpireTime = DateTime.Now.AddSeconds((int)o["expires_in"]);
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token.AccessToken);
        }
    }
}