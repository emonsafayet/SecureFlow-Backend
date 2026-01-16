using AutoMapper; 
using SecureFlow.Application.Users;
using SecureFlow.Domain.Auth;

namespace SecureFlow.Application.Common.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(
                dest => dest.UserType,
                opt => opt.MapFrom(src => src.UserType.ToString())
            );
    }
}
