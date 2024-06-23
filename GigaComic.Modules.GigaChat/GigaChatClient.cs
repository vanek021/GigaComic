using GigaComic.Modules.GigaChat.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;

namespace GigaComic.Modules.GigaChat
{
    public class GigaChatClient
    {
        private readonly ChatDialog _dialog;
        private readonly HttpClient _client;
        private readonly AccessTokenProvider _accessTokenProvider;

        public GigaChatClient(HttpClient client, AccessTokenProvider accessTokenProvider)
        {
            _client = client;
            _dialog = new ChatDialog()
            {
                Messages = new List<Message>(),
                Temperature = 0.7f,
                Model = "GigaChat:latest"
            };
            _accessTokenProvider = accessTokenProvider;
        }

        public async Task<string> GenerateAnswer(string prompt)
        {
            try
            {
                var accessToken = await _accessTokenProvider.GetToken();

                var message = new Message()
                {
                    Content = prompt,
                    Role = "user"
                };

                _dialog.Messages.Add(message);

                var request = await CreateGenerateAnswerRequest(_dialog, accessToken.AccessToken);

                var response = await _client.SendAsync(request);

                var chatResponse = await GetDeserializeObject<ChatResponse>(response);
                _dialog.Messages.Add(chatResponse.Choices[0].Message);
                return chatResponse.Choices[0].Message.Content;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<HttpRequestMessage> CreateGenerateAnswerRequest(ChatDialog dialog, string accessToken)
        {
            var uri = @"/api/v1/chat/completions";
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(dialog, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
            }));
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
