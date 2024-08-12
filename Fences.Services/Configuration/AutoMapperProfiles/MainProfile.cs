using AutoMapper;
using Fences.Model.DataModels;
using Fences.ViewModels.VM;


namespace Fences.Services.Configuration.AutoMapperProfiles
{
    public class MainProfile : Profile
    {
        public MainProfile() 
        {
            CreateMap<Job, JobVm>();

            CreateMap<RegisterVm, User>()
                .ForMember(dest => dest.UserName, y => y.MapFrom(src => src.Email))
                .ForMember(dest => dest.RegistrationDate, y => y.MapFrom(src => DateTime.Now));
        }
    }
}
