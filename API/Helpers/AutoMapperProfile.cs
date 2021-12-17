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
                .ForMember(x => x.DateOfBirth, c => c.MapFrom(v => v.DateOfBirth.Date.ToString("yyyy/MM/dd")))
                .ForMember(x => x.Age, c => c.MapFrom(v => v.GetAge()))
                .ForMember(x => x.PhotoUrl, c => c.MapFrom(v => v.Photos.FirstOrDefault(b => b.IsMain).Url));
            //create Register
            CreateMap<RegisterDto,Users>();


            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, Users>();
        }
    }
}