using GigaComic.Modules.GigaChat.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;

namespace GigaComic.Modules.GigaChat
{
    public class GigaChatClient
    {
        private readonly Dictionary<int, ChatDialog> _dialogs;
        private int _dialogIdGen;
        private readonly HttpClient _client;
        private readonly string _authToken;
        private AccessToken _accessToken;

        public GigaChatClient(HttpClient client, string authToken)
        {
            _client = client;
            _dialogs = new Dictionary<int, ChatDialog>();
            _authToken = authToken;
        }

        public async Task<int> StartDialog()
        {
            lock(_dialogs){
                var id = _dialogIdGen++;
                if (_dialogs.ContainsKey(id))
                    throw new Exception($"Диалог с id {id} уже есть");

                _dialogs[id] = new ChatDialog()
                {
                    Messages = new List<Message>(),
                    Temperature = 0.7f,
                    Model = "GigaChat:latest"
                };
                return id;
            }
        }

        public async Task StopDialog(int dialogId)
        {
            lock (_dialogs)
            {
                if (!_dialogs.ContainsKey(dialogId))
                    throw new ArgumentException($"Диалога с id {dialogId} не существует");

                _dialogs.Remove(dialogId);
            }
        }

        public async Task<string> GenerateAnswer(int dialogId, string prompt)
        {
            ChatDialog dialog;
            lock (_dialogs)
            {
                if (!_dialogs.ContainsKey(dialogId))
                    throw new ArgumentException($"Диалога с id {dialogId} не существует");

                dialog = _dialogs[dialogId];
            }

            if (new DateTime(_accessToken.ExpiresAt) < DateTime.Now.AddMinutes(1))
                _accessToken = await GetToken();

            var message = new Message()
            {
                Content = prompt,
                Role = "user"
            };

            dialog.Messages.Add(message);

            var request = await CreateGenerateAnswerRequest(dialog);

            var response = await _client.SendAsync(request);

            var chatResponse = await GetDeserializeObject<ChatResponse>(response);
            dialog.Messages.Add(chatResponse.Choices[0].Message);
            return chatResponse.Choices[0].Message.Content;
        }

        private async Task<HttpRequestMessage> CreateGenerateAnswerRequest(ChatDialog dialog)
        {
            var uri = @"/api/v1/chat/completions";
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken.Token);
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            var content = new StringContent(JsonConvert.SerializeObject(dialog));
            request.Content = content;

            return request;
        }

        private async Task<AccessToken> GetToken()
        {
            var request = CreateGetTokenRequest();

            var response = await _client.SendAsync(request);

            return await GetDeserializeObject<AccessToken>(response);
        }

        private HttpRequestMessage CreateGetTokenRequest()
        {
            var uri = @"/api/v2/oauth";

            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", _authToken);

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
