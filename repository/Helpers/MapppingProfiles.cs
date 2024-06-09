using AutoMapper;
using Repository.Models;
using Repository.ViewModels;

namespace server.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<UserDto, User>();
    }
}