using AutoMapper;
using GigaComic.Models.Entities.Comic;
using GigaComic.Models.Enums;
using GigaComic.Shared.Requests.Comic;
using GigaComic.Shared.Responses.Comic;

namespace GigaComic.Configurations
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile() {
            CreateMap<Comic, ComicResponse>();
            CreateMap<ComicAbstract, ComicAbstractResponse>();
            CreateMap<CreateComicRequest, Comic>();
            CreateMap<AbstractRequest, ComicAbstract>();
            CreateMap<ComicSetupRequest, Comic>();
        }

    }
}
