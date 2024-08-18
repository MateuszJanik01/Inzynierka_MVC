namespace Fences.Model.DataModels
{
    public class Job
    {
        public int Id { get; set; }
        public virtual User User { get; set; } = null!;
        public int UserId { get; set; }
        public string Town { get; set; } = null!;
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string ZipCode { get; set; } = null!;
        public string JobType { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime DateOfExecution {  get; set; }
    }
}
