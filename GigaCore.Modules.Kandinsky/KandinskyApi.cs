using System.Text;
using GigaComic.Core.Attributes;
using GigaComic.Core.Interfaces.Kandinsky;
using GigaComic.Core.Models.Kandinsky.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GigaComic.Modules.Kandinsky;

[Injectable]
public class KandinskyApi : IKandinskyApi
{
    private readonly HttpClient _httpClient;

    public KandinskyApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<GetModelResponse> GetModel()
    {
        var response = await _httpClient.GetAsync("key/api/v1/models");

        return await GetDeserializeObject<GetModelResponse>(response);
    }
    
    public async Task<GenerateResponse> Generate(string prompt, int modelId = 1, int count = 1, int width = 1024, int height = 1024)
    {
        var parameters = new JObject
        {
            ["type"] = "GENERATE",
            ["numImages"] = count,
            ["width"] = width,
            ["height"] = height,
            ["generateParams"] = new JObject
            {
                ["query"] = prompt
            }
        };
        var content = new JObject
        {
            ["model_id"] = modelId,
            ["params"] = parameters
        };
        var data = new StringContent(content.ToString(), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/key/api/v1/text2image/run", data);

        return await GetDeserializeObject<GenerateResponse>(response);
    }
    
    private static async Task<T> GetDeserializeObject<T>(HttpResponseMessage requestMessage)
    {
        if (!requestMessage.IsSuccessStatusCode)
            throw new Exception();
        
        var responseContent = await requestMessage.Content.ReadAsStringAsync();
        var deserializeObject = JsonConvert.DeserializeObject<T>(responseContent);

        if (deserializeObject is null)
            throw new Exception();

        return deserializeObject;
    }
}