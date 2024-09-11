using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fences.Model.DataModels
{
    public class ContactForm
    {
        [Required(ErrorMessage = "Proszę podać swoje imię i nazwisko.")]
        [RegularExpression(@"^[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ]+ [A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ]+$", ErrorMessage = "Proszę podać poprawne imię i nazwisko, oddzielone spacją.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Proszę podać swój e-mail.")]
        [EmailAddress(ErrorMessage = "Proszę podać poprawny adres e-mail.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Proszę podać treść wiadomości.")]
        [MaxLength(200, ErrorMessage = "Treść wiadomości nie może przekraczać 200 znaków.")]
        public string Content { get; set; } = null!;
    }

}
