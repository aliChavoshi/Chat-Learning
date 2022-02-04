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
                // .ForMember(x => x.DateOfBirth, c => c.MapFrom(v => v.DateOfBirth.Date.ToString("yyyy/MM/dd")))
                .ForMember(x => x.Age, c => c.MapFrom(v => v.GetAge()))
                .ForMember(x => x.PhotoUrl, c => c.MapFrom(v => v.Photos.FirstOrDefault(b => b.IsMain).Url));
            //create Register
            CreateMap<RegisterDto, Users>();

            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, Users>();
            //user Likes
            CreateMap<Users, LikeDto>()
                .ForMember(x => x.Age, c => c.MapFrom(v => v.GetAge()))
                .ForMember(x => x.PictureUrl, c => c.MapFrom(v => v.Photos.FirstOrDefault(x => x.IsMain).Url));
            //message
            CreateMap<Message, MessageDto>()
                .ForMember(x => x.ReceiverPhotoUrl, c => c.MapFrom(v => v.Receiver.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(x => x.SenderPhotoUrl, c => c.MapFrom(v => v.Sender.Photos.FirstOrDefault(x => x.IsMain).Url));
            CreateMap<MessageDto, Message>();
        }
    }
}