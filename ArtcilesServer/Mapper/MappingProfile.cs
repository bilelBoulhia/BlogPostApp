using ArtcilesServer.DTO;
using ArtcilesServer.Models;
using AutoMapper;

namespace ArtcilesServer.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {

            CreateMap<UserDTO,User>();
            CreateMap<User,UserDTO>();
            CreateMap<Hobby,HobbyDTO>();

            CreateMap<int, Hobby>()
            .ForMember(dest => dest.HobbyId, opt => opt.MapFrom(src => src));
            CreateMap<HobbyDTO, Hobby>()
           .ForMember(dest => dest.Users, opt => opt.Ignore()); 

            CreateMap<User,UserDTO>();
            CreateMap<CommentDTO, Comment>();
            CreateMap<Comment, CommentDTO>();
            CreateMap<ReportDTO, Report>();
            CreateMap<Article,ArticleDTO>();
            CreateMap<Article,CategoryDTO>();
            CreateMap<CategoryDTO,Article>();
            CreateMap<Category,CategoryDTO>();
            CreateMap<ArticleDTO, Article>();

        }
    }
}
