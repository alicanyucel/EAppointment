using AutoMapper;
using EAppointment.Application.Features.Doctors.CreateDoctor;
using EAppointment.Application.Features.Doctors.UpdateDoctor;
using EAppointment.Domain.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}