using AutoMapper;

namespace LessonMonitor.API
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<Contracts.NewMember, Core.Member>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Core.Member, Contracts.Member>().ReverseMap();

            CreateMap<Contracts.NewLesson, Core.Lesson>();

            //CreateMap<Core.Lesson, Contracts.Lesson>().ReverseMap();
        }
    }
}
