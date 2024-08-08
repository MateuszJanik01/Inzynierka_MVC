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
        }
    }
}
