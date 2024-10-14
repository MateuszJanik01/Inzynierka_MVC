using Fences.DAL.EF;
using Fences.Model.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Fences.Tests
{
    public static class Extensions
    {
        // Creata sample data
        public static async Task SeedData(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

            // Roles
            var userRole = new Role()
            {
                Id = 1,
                Name = "User",
                RoleValue = RoleValue.User
            };
            await roleManager.CreateAsync(userRole);

            var adminRole = new Role()
            {
                Id = 2,
                Name = "Admin",
                RoleValue = RoleValue.Admin
            };
            await roleManager.CreateAsync(adminRole);

            var userPassword = "User1234";
            var u1 = new User()
            {
                Id = 1,
                FirstName = "Marcin",
                LastName = "Parkitny",
                UserName = "u1@eg.eg",
                Email = "real_email@eg.eg",
                RegistrationDate = new DateTime(2010, 1, 1),
            };
            await userManager.CreateAsync(u1, userPassword);
            await userManager.AddToRoleAsync(u1, userRole.Name);

            var u2 = new User()
            {
                Id = 2,
                FirstName = "Anna",
                LastName = "Nowak",
                UserName = "u2@eg.eg",
                Email = "u2@eg.eg",
                RegistrationDate = new DateTime(2012, 5, 12),
            };
            await userManager.CreateAsync(u2, userPassword);
            await userManager.AddToRoleAsync(u2, userRole.Name);

            var u3 = new User()
            {
                Id = 3,
                FirstName = "Krzysztof",
                LastName = "Zieliński",
                UserName = "u3@eg.eg",
                Email = "u3@eg.eg",
                RegistrationDate = new DateTime(2015, 8, 23),
            };
            await userManager.CreateAsync(u3, userPassword);
            await userManager.AddToRoleAsync(u3, userRole.Name);

            var u4 = new User()
            {
                Id = 4,
                FirstName = "Ewa",
                LastName = "Kowalska",
                UserName = "u4@eg.eg",
                Email = "u4@eg.eg",
                RegistrationDate = new DateTime(2018, 3, 10),
            };
            await userManager.CreateAsync(u4, userPassword);
            await userManager.AddToRoleAsync(u4, userRole.Name);

            var u5 = new User()
            {
                Id = 5,
                FirstName = "Tomasz",
                LastName = "Jankowski",
                UserName = "u5@eg.eg",
                Email = "u5@eg.eg",
                RegistrationDate = new DateTime(2020, 11, 1),
            };
            await userManager.CreateAsync(u5, userPassword);
            await userManager.AddToRoleAsync(u5, userRole.Name);

            // Created Admins
            var adminPassword = "Admin1234";

            var a1 = new User()
            {
                Id = 6,
                FirstName = "Paweł",
                LastName = "Wiśniewski",
                UserName = "admin1@eg.eg",
                Email = "admin1@eg.eg",
                RegistrationDate = new DateTime(2010, 9, 15),
            };
            await userManager.CreateAsync(a1, adminPassword);
            await userManager.AddToRoleAsync(a1, adminRole.Name);

            var a2 = new User()
            {
                Id = 7,
                FirstName = "Katarzyna",
                LastName = "Wróbel",
                UserName = "admin2@eg.eg",
                Email = "admin2@eg.eg",
                RegistrationDate = new DateTime(2013, 6, 21),
            };
            await userManager.CreateAsync(a2, adminPassword);
            await userManager.AddToRoleAsync(a2, adminRole.Name);


            // Create Jobs
            var random = new Random();

            DateTime GetFutureWeekdayDate()
            {
                DateTime date;
                do
                {
                    date = DateTime.Now.AddDays(random.Next(1, 365));
                } while (date.DayOfWeek == DayOfWeek.Sunday);
                return date;
            }

            var jobs = new List<Job>
            {
                new Job
                {
                    UserId = 1,
                    Town = "Warsaw",
                    Street = "Kwiatowa",
                    Number = "12A",
                    ZipCode = "00-001",
                    TotalLength = 100.5,
                    Height = 1.8,
                    JobType = "Fence Installation",
                    Description = "Install wooden fence around garden",
                    RegistrationDate = DateTime.Now,
                    DateOfExecution = GetFutureWeekdayDate()
                },
                new Job
                {
                    UserId = 2,
                    Town = "Krakow",
                    Street = "Mickiewicza",
                    Number = "5B",
                    ZipCode = "30-001",
                    TotalLength = 50.0,
                    Height = 1.5,
                    JobType = "Fence Repair",
                    Description = "Repair broken metal fence",
                    RegistrationDate = DateTime.Now,
                    DateOfExecution = GetFutureWeekdayDate()
                },
                new Job
                {
                    UserId = 3,
                    Town = "Poznan",
                    Street = "Polna",
                    Number = "7C",
                    ZipCode = "61-001",
                    TotalLength = 80.0,
                    Height = 1.7,
                    JobType = "Fence Replacement",
                    Description = "Replace old wooden fence with a new one",
                    RegistrationDate = DateTime.Now,
                    DateOfExecution = GetFutureWeekdayDate()
                },
                new Job
                {
                    UserId = 4,
                    Town = "Gdansk",
                    Street = "Sobieskiego",
                    Number = "9D",
                    ZipCode = "80-001",
                    TotalLength = 120.0,
                    Height = 2.0,
                    JobType = "New Fence Construction",
                    Description = "Build a new stone fence around the property",
                    RegistrationDate = DateTime.Now,
                    DateOfExecution = GetFutureWeekdayDate()
                },
                new Job
                {
                    UserId = 5,
                    Town = "Wroclaw",
                    Street = "Słoneczna",
                    Number = "11E",
                    ZipCode = "50-001",
                    TotalLength = 65.5,
                    Height = 1.9,
                    JobType = "Fence Painting",
                    Description = "Paint metal fence with anti-corrosion paint",
                    RegistrationDate = DateTime.Now,
                    DateOfExecution = GetFutureWeekdayDate()
                },
                new Job
                {
                    UserId = 1,
                    Town = "Lodz",
                    Street = "Piotrkowska",
                    Number = "15",
                    ZipCode = "90-001",
                    TotalLength = 200.0,
                    Height = 2.2,
                    JobType = "Fence Construction",
                    Description = "Install a concrete fence around the industrial area",
                    RegistrationDate = DateTime.Now,
                    DateOfExecution = GetFutureWeekdayDate()
                },
                new Job
                {
                    UserId = 2,
                    Town = "Szczecin",
                    Street = "Matejki",
                    Number = "21F",
                    ZipCode = "70-001",
                    TotalLength = 75.0,
                    Height = 1.6,
                    JobType = "Fence Replacement",
                    Description = "Replace broken wooden fence with a metal one",
                    RegistrationDate = DateTime.Now,
                    DateOfExecution = GetFutureWeekdayDate()
                },
                new Job
                {
                    UserId = 3,
                    Town = "Lublin",
                    Street = "Lipowa",
                    Number = "33G",
                    ZipCode = "20-001",
                    TotalLength = 55.0,
                    Height = 1.5,
                    JobType = "Fence Installation",
                    Description = "Install a new metal fence for garden",
                    RegistrationDate = DateTime.Now,
                    DateOfExecution = GetFutureWeekdayDate()
                },
                new Job
                {
                    UserId = 4,
                    Town = "Bialystok",
                    Street = "Chrobrego",
                    Number = "12H",
                    ZipCode = "15-001",
                    TotalLength = 90.0,
                    Height = 2.1,
                    JobType = "Fence Painting",
                    Description = "Paint wooden fence around the house",
                    RegistrationDate = DateTime.Now,
                    DateOfExecution = GetFutureWeekdayDate()
                },
                new Job
                {
                    UserId = 5,
                   Town = "Katowice",
                    Street = "Kościuszki",
                    Number = "8I",
                    ZipCode = "40-001",
                    TotalLength = 100.0,
                    Height = 1.9,
                    JobType = "Fence Repair",
                    Description = "Fix broken metal gate and repaint",
                    RegistrationDate = DateTime.Now,
                    DateOfExecution = GetFutureWeekdayDate()
                },
                new Job
                {
                    UserId = 1,
                    Town = "Rzeszow",
                    Street = "Warszawska",
                    Number = "19J",
                    ZipCode = "35-001",
                    TotalLength = 85.0,
                    Height = 1.8,
                    JobType = "Fence Construction",
                    Description = "Install a new aluminum fence for backyard",
                    RegistrationDate = DateTime.Now,
                    DateOfExecution = GetFutureWeekdayDate()
                },
                new Job
                {
                    UserId = 2,
                    Town = "Kielce",
                    Street = "Sienkiewicza",
                    Number = "10K",
                    ZipCode = "25-001",
                    TotalLength = 110.0,
                    Height = 1.9,
                    JobType = "Fence Replacement",
                    Description = "Replace old fence with modern aluminum fence",
                    RegistrationDate = DateTime.Now,
                    DateOfExecution = GetFutureWeekdayDate()
                },
                new Job
                {
                    UserId = 3,
                    Town = "Zielona Gora",
                    Street = "Wyszyńskiego",
                    Number = "4L",
                    ZipCode = "65-001",
                    TotalLength = 60.0,
                    Height = 1.7,
                    JobType = "Fence Repair",
                    Description = "Repair broken wooden gate",
                    RegistrationDate = DateTime.Now,
                    DateOfExecution = GetFutureWeekdayDate()
                },
                new Job
                {
                    UserId = 4,
                    Town = "Opole",
                    Street = "Reymonta",
                    Number = "22M",
                    ZipCode = "45-001",
                    TotalLength = 45.0,
                    Height = 1.5,
                    JobType = "Fence Painting",
                    Description = "Paint stone fence in the front yard",
                    RegistrationDate = DateTime.Now,
                    DateOfExecution = GetFutureWeekdayDate()
                },
                new Job
                {
                    UserId = 5,
                    Town = "Radom",
                    Street = "Traugutta",
                    Number = "14N",
                    ZipCode = "26-001",
                    TotalLength = 70.0,
                    Height = 1.6,
                    JobType = "Fence Replacement",
                    Description = "Replace chain-link fence with wooden fence",
                    RegistrationDate = DateTime.Now,
                    DateOfExecution = GetFutureWeekdayDate()
                }
            };

            await dbContext.Jobs.AddRangeAsync(jobs);
            await dbContext.SaveChangesAsync();
        }
    }
}
