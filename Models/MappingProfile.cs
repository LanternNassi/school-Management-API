using AutoMapper;
using School_Management_System.Models.UserGroupX;
using School_Management_System.Models.UserX;
using School_Management_System.Models.ContactInfoX;
using School_Management_System.Models.ClassX;
using School_Management_System.Models.StudentX;

namespace School_Management_System.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile() { 

            CreateMap<UserGroup , UserGroupDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<ContactInfo , ContactInfoDto>().ReverseMap();

            CreateMap<Class, ClassDto>().ReverseMap();
            CreateMap<StreamX.Stream, StreamX.StreamDto>().ReverseMap();


            CreateMap<Student, StudentDto>().ReverseMap();
            CreateMap<Student, StudentDtoUpdate>().ReverseMap();

        }

    }
}
