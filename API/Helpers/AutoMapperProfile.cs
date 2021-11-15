using System.Linq;
using API.Entities;
using API.Models;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Users, MemberDto>()
                .ForMember(x => x.PhotoUrl, c => c.MapFrom(v => v.Photos.FirstOrDefault(b => b.IsMain).Url));

            CreateMap<Photo, PhotoDto>();
        }
    }
}