namespace GigaComic.Core.Models.Kandinsky.Responses;

public class GenerateResponse
{
    public string Uuid { get; set; }
    public string Status { get; set; }
    public List<string> Images { get; set; } = [];
    public bool Censored { get; set; }
    public string ErrorDescription { get; set; }
}