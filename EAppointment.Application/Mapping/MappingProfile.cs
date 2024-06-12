using AutoMapper;
using EAppointment.Application.Features.Doctors.CreateDoctor;
using EAppointment.Application.Features.Doctors.UpdateDoctor;
using EAppointment.Application.Features.Patients.CreatePatient;
using EAppointment.Application.Features.Patients.UpdatePatient;
using EAppointment.Application.Features.Users.CreateUser;
using EAppointment.Application.Features.Users.UpdateUser;
using EAppointment.Domain.Entities;

namespace EAppointment.Application.Mapping
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateDoctorCommand, Doctor>().ForMember(member => member.Department, options =>
            {
                options.MapFrom(map => DepartmentEnum.FromValue(map.DepartmentValue));
            });
            CreateMap<UpdataDoctorCommand, Doctor>().ForMember(member => member.Department, options =>
            {
                options.MapFrom(map => DepartmentEnum.FromValue(map.DepartmentValue));
            });
            CreateMap<CreatePatientCommand, Patient>().ReverseMap();
            CreateMap<UpdatePatientCommand, Patient>().ReverseMap();
            CreateMap<CreateUserCommand, AppUser>();
            CreateMap<UpdateUserCommand, AppUser>();
        }
    }
}