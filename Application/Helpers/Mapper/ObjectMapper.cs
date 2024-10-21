using AutoMapper;
using Domain.Dtos;
using Domain.Entites;

namespace Application.Helpers.Mapper;
public class ObjectMapper
{
    private static readonly Lazy<IMapper> Lazy = new(() =>
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.ShouldMapProperty = p => p.GetMethod != null && (p.GetMethod.IsPublic || p.GetMethod.IsAssembly);
            cfg.AddProfile<AppMapper>();
        });
        var mapper = config.CreateMapper();
        return mapper;
    });
    public static IMapper Mapper => Lazy.Value;

    public class AppMapper : Profile
    {
        public AppMapper()
        {
            CreateMap<AppUser, UserDataDto>().ReverseMap();

        }
    }
}
