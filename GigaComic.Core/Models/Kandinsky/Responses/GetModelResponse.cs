namespace GigaComic.Core.Models.Kandinsky.Responses;

public class GetModelResponse
{
    public IList<Model> Models { get; set; }

    public class Model
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Type { get; set; }
    }
}