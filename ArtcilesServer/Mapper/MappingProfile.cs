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
            CreateMap<CommentDTO, Comment>();
            CreateMap<Comment, CommentDTO>();
            CreateMap<ReportDTO, Report>();
            CreateMap<Article,ArticleDTO>();
            CreateMap<ArticleDTO, Article>();

        }
    }
}
