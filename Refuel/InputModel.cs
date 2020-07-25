using System.ComponentModel.DataAnnotations;

namespace Refuel
{
    public class InputModel
    {
        [Required]
        [Display(Name = "Login")]
        [StringLength(20, ErrorMessage = "Login może mieć maksymalnie 20 znaków")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Zapamiętać?")]
        public bool Remember { get; set; }
    }
}