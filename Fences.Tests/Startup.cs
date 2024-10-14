using Fences.DAL.EF;
using Fences.Model.DataModels;
using Fences.Services.ConcreteServices;
using Fences.Services.Configuration.AutoMapperProfiles;
using Fences.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fences.Tests;
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MainProfile));
        services.AddEntityFrameworkInMemoryDatabase()
            .AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("InMemoryDb")
            );
        services.AddIdentity<User, Role>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 0;
            options.Password.RequireNonAlphanumeric = false;
        })
            .AddRoleManager<RoleManager<Role>>()
            .AddUserManager<UserManager<User>>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        services.AddTransient(typeof(ILogger), typeof(Logger<Startup>));
        services.AddTransient<IJobService, JobService>();
        services.SeedData().Wait();
    }
}