using Microsoft.AspNetCore.Identity;

namespace Fences.Model.DataModels
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime RegistrationDate { get; } = DateTime.Now;
        public virtual List<Job>? JobList { get; set; }
    }
}