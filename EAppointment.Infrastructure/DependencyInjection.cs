using EAppointment.Domain.Entities;
using EAppointment.Domain.Repositories;
using EAppointment.Infrastructure.Context;
using EAppointment.Infrastructure.Repositories;
using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EAppointment.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection  AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>

            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
            });
            services.AddIdentity<AppUser, AppRole>(action =>
            {
                action.Password.RequiredLength = 1;
                action.Password.RequireUppercase = false;
                action.Password.RequireLowercase = false;
                action.Password.RequireNonAlphanumeric = false;
                action.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddScoped<IAppointmentsRepository,AppointmentRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IPatientRepository,PatientRepository>();
            services.AddScoped<IUnitOfWork>(srv=>srv.GetRequiredService<ApplicationDbContext>());
            return services;
        }
    }
}
