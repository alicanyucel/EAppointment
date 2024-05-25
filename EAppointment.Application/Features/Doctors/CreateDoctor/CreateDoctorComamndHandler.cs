using AutoMapper;
using EAppointment.Domain.Entities;
using EAppointment.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace EAppointment.Application.Features.Doctors.CreateDoctor
{
    internal sealed class CreateDoctorComamndHandler(IDoctorRepository doctorRepository,IUnitOfWork unitOfWork,IMapper mapper) : IRequestHandler<CreateDoctorCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
            Doctor doctor = mapper.Map<Doctor>(request);
            
            await doctorRepository.AddAsync(doctor,cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return "Doktor kaydı yapıldı"
;        }
    }

}
