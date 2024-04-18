using GigaComic.Core.Models.Kandinsky.Responses;

namespace GigaComic.Core.Interfaces.Kandinsky;

public interface IKandinskyApi
{
    public Task<GetModelResponse> GetModel();

    public Task<GenerateResponse> Generate(string prompt, int modelId = 1, int count = 1, int width = 1024,
        int height = 1024);
}