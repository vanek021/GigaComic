using System.Net.Http.Headers;
using System.Text;
using GigaComic.Core.Attributes;
using GigaComic.Core.Models.Kandinsky.Responses;
using GigaComic.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace GigaComic.Modules.Kandinsky;

[Injectable]
public class KandinskyApi
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
    
    public async Task<GenerateResponse> Generate(string prompt, int modelId = 4, int count = 1, 
        int width = 1024, int height = 1024, ComicStyle style = ComicStyle.Default)
    {
        var generateParams = new
        {
            type = "GENERATE",
            numImages = count,
            width = width,
            height = height,
            generateParams = new { query = prompt }
        };

        var data = new MultipartFormDataContent
        {
            { new StringContent(modelId.ToString()), "model_id" },
            { new StringContent(JObject.FromObject(generateParams).ToString(), mediaType: MediaTypeHeaderValue.Parse("application/json")), "params" }
        };

        var response = await _httpClient.PostAsync("/key/api/v1/text2image/run", data);
        var rstr = await response.Content.ReadAsStringAsync();
        return await GetDeserializeObject<GenerateResponse>(response);
    }

    public async Task<GenerateResponse> CheckGeneration(string uuid)
    {
        var response = await _httpClient.GetAsync($"key/api/v1/text2image/status/{uuid}");
        return await GetDeserializeObject<GenerateResponse>(response);
    }

    public async Task<GenerateResponse> GenerateAndWait(string prompt, int modelId = 4, int count = 1,
        int width = 1024, int height = 1024, ComicStyle style = ComicStyle.Default)
    {
        var generated = await Generate(prompt, modelId, count, width, height, style);
        var uuid = generated.Uuid;
        var attempts = 60;
        var wait = 2000;

        while (attempts > 0)
        {
            generated = await CheckGeneration(uuid);

            // TODO: To enum
            if (generated.Status == "DONE" || generated.Status == "FAIL")
                return generated;

            attempts--;
            await Task.Delay(wait);
        }

        if (generated.Status != "DONE" || generated.Status != "FAIL")
            throw new Exception("Attempts exceeded");

        return generated;
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