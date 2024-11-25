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
        //[RegularExpression(@"^[1-9][0-9]{0,2}[a-zA-Z]?$", ErrorMessage = "Numer domu musi być w zakresie od 1 do 999 i może zawierać jedną literę.")]
        public string? Number { get; set; }
        //[RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Nieprawidłowy format kodu pocztowego (wymagany format XX-XXX).")]
        public string ZipCode { get; set; } = null!;
        [Range(0, 999.99, ErrorMessage = "Wartość musi być z zakresu 0 do 999.99.")]
        public double TotalLength { get; set; }
        [Range(0, 9.99, ErrorMessage = "Wartość musi być z zakresu 0 do 9.99")]
        public double Height { get; set; }
        public string JobType { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime DateOfExecution {  get; set; }
        [Range(0.00, 99999.99, ErrorMessage = "Wartość musi być z zakresu 0 do 999999.99")]
        public double TotalPrice { get; set; } = 0.00;
    }
}
