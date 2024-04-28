using EAppointment.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EAppointment.WebApi
{
    public static class Helper
    {
        public static async Task CreateUserAsync(WebApplication app)
        {
            using (var scoped = app.Services.CreateScope())
            {
                var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                if (userManager.Users.Any())
                {
                 userManager.CreateAsync(new()
                    {
                        FirstName = "Ali Can",
                        LastName = "Yücel",
                        Email = "yucealican@hotmail.com",
                        UserName = "Admin",
                    }, "1").Wait();
                }

            }
        }
    }
}
