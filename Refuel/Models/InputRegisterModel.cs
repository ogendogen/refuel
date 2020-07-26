using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class InputRegisterModel
    {
        [Required]
        [StringLength(32, ErrorMessage = "Login może mieć maksymalnie 32 znaki!")]
        [Display(Name = "Login")]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Powtórz hasło")]
        public string Password2 { get; set; }
        [Required]
        [StringLength(64, ErrorMessage = "Email może mieć maksymalnie 64 znaki!")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Adres Email")]
        public string Email { get; set; }
    }
}
