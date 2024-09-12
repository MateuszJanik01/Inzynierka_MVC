namespace Fences.ViewModels.VM
{
    public class AddOrUpdateJobVm
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public string Town { get; set; } = null!;
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string ZipCode { get; set; } = null!;
        public float TotalLength { get; set; }
        public float Height { get; set; }
        public string JobType { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime DateOfExecution { get; set; }
    }
}
