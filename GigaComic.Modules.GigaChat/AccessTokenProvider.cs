using GigaComic.Modules.GigaChat.Models;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace GigaComic.Modules.GigaChat
{
    public class AccessTokenProvider
    {
        private readonly string _authToken;
        private readonly ReaderWriterLock _accessTokenLock = new ReaderWriterLock();
        private readonly HttpClient _client;
        private readonly IDateTimeProvider _dateTimeProvider;
        private AccessTokenInfo _accessToken;

        public AccessTokenProvider(string authToken, HttpClient client, IDateTimeProvider dateTimeProvider)
        {
            _authToken = authToken;
            _client = client;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<AccessTokenInfo> GetToken()
        {
            _accessTokenLock.AcquireReaderLock(10_000);
            var accessToken = _accessToken;
            _accessTokenLock.ReleaseReaderLock();

            if (accessToken is null || 
                _dateTimeProvider.FromUtcMilliseconds(accessToken.ExpiresAt).AddMinutes(-1) < _dateTimeProvider.Now)
            {
                _accessTokenLock.AcquireWriterLock(10_000);
                accessToken = _accessToken;
                if (accessToken is null ||
                _dateTimeProvider.FromUtcMilliseconds(accessToken.ExpiresAt).AddMinutes(-1) < _dateTimeProvider.Now)
                {
                    if (accessToken != null)
                        Thread.Sleep(3000);
                    Console.WriteLine("Запрос токена");
                    accessToken = RequestToken().Result;
                    _accessToken = accessToken;
                }
                _accessTokenLock.ReleaseWriterLock();
                Console.WriteLine(_dateTimeProvider.FromUtcMilliseconds(accessToken.ExpiresAt).AddMinutes(-1));
                Console.WriteLine(_dateTimeProvider.Now);
            }

            _accessTokenLock.AcquireReaderLock(10_000);
            accessToken = _accessToken;
            _accessTokenLock.ReleaseReaderLock();

            return accessToken;
        }

        private async Task<AccessTokenInfo> RequestToken()
        {
            var request = CreateTokenRequest(_authToken);

            var response = await _client.SendAsync(request);

            return await GetDeserializeObject<AccessTokenInfo>(response);
        }

        private HttpRequestMessage CreateTokenRequest(string authToken)
        {
            var uri = @"/api/v2/oauth";

            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("scope", "GIGACHAT_API_PERS") });
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            content.Headers.Add("RqUID", Guid.NewGuid().ToString());

            request.Content = content;

            return request;
        }

        private static async Task<T> GetDeserializeObject<T>(HttpResponseMessage requestMessage)
        {
            if (!requestMessage.IsSuccessStatusCode)
                throw new Exception();

            var responseContent = await requestMessage.Content.ReadAsStringAsync();
            var deserializeObject = JsonConvert.DeserializeObject<T>(responseContent, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
            });

            if (deserializeObject is null)
                throw new Exception();

            return deserializeObject;
        }
    }
}
