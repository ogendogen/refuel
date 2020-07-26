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
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Zapamiętaj mnie")]
        public bool RememberMe { get; set; }
    }
}