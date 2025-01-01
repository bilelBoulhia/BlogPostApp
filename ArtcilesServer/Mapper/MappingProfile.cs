using ArtcilesServer.DTO;
using ArtcilesServer.Models;
using AutoMapper;

namespace ArtcilesServer.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {

            CreateMap<UserDTO,User>();
            CreateMap<CommentDTO, Comment>();
            CreateMap<ReportDTO, Report>();
            CreateMap<ArticleDTO, Article>();

        }
    }
}
