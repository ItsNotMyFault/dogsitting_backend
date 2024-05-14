using System.Net.Http.Headers;
using System.Text;

namespace dogsitting_backend.Infrastructure
{
    public abstract class RestApiRepository
    {
        protected HttpClient HttpClient { get; set; } = new();
        public static AuthenticationHeaderValue GetBasicAuthenticationHeader(string username, string password)
        {
            string creds = string.Format("{0}:{1}", username, password);
            byte[] bytes = Encoding.ASCII.GetBytes(creds);
            AuthenticationHeaderValue header = new("Basic", Convert.ToBase64String(bytes));
            return header;
        }

        public AuthenticationHeaderValue GetBearerTokenAuthenticationHeader(string accessToken)
        {
            AuthenticationHeaderValue header = GetTokenAuthenticationHeader("Bearer", accessToken);
            return header;
        }
        public AuthenticationHeaderValue GetTokenAuthenticationHeader(string TokenName, string accessToken)
        {
            AuthenticationHeaderValue header = new(TokenName, accessToken);
            return header;
        }
        public async Task<string> PostRequest(string requestURL, HttpContent content, AuthenticationHeaderValue? authHeader = null)
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = authHeader;

            HttpResponseMessage response = client.PostAsync(requestURL, content).Result;
            string responseString = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"POST: {response.StatusCode} => {response.ReasonPhrase} : {responseString}", new HttpRequestException(), response.StatusCode);
            }
            return responseString;
        }

        public async Task<HttpResponseMessage> PostRequest(string requestURL, AuthenticationHeaderValue? authHeader = null)
        {
            Dictionary<string, string> values = new();
            FormUrlEncodedContent content = new(values);
            HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = authHeader;

            HttpResponseMessage response = client.PostAsync(requestURL, content).Result;
            string responseString = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"POST: {response.StatusCode} => {response.ReasonPhrase} : {responseString}", new HttpRequestException(), response.StatusCode);
            }
            return response;
        }

        public async Task<string> PatchRequest(string requestURL, HttpContent content, AuthenticationHeaderValue? authHeader = null)
        {
            this.HttpClient.DefaultRequestHeaders.Authorization = authHeader;
            HttpResponseMessage response = this.HttpClient.PatchAsync(requestURL, content).Result;
            string responseString = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"POST: {response.StatusCode} => {response.ReasonPhrase}", new HttpRequestException(), response.StatusCode);
            }
            return responseString;
        }

        public async Task<string> GetRequest(string requestURL, AuthenticationHeaderValue? authHeader = null)
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = authHeader;
            HttpResponseMessage response = await client.GetAsync(requestURL);
            string responseString = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"GET: {requestURL} - {response.StatusCode} => {response.ReasonPhrase}", new HttpRequestException(), response.StatusCode);
            }
            return responseString;
        }

        public async Task<string> PUTRequest(string requestURL, AuthenticationHeaderValue authHeader, MultipartFormDataContent content)
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = authHeader;
            HttpResponseMessage response = await client.PutAsync(requestURL, content);
            string responseString = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"PUT: {requestURL} - {response.StatusCode} => {response.ReasonPhrase}", new HttpRequestException(), response.StatusCode);
            }
            return responseString;
        }
    }
}
