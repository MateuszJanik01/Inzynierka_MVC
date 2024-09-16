using System.ComponentModel.DataAnnotations;

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
        [Range(0, 999.99, ErrorMessage = "Wartość musi być z zakresu 0 do 999.99.")]
        public double TotalLength { get; set; }
        [Range(0, 9.99, ErrorMessage = "Wartość musi być z zakresu 0 do 9.99")]
        public double Height { get; set; }
        public string JobType { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime DateOfExecution {  get; set; }
    }
}
