using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class LoginDto
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمائید")]
        [MaxLength(50, ErrorMessage = "بیشتری کارکتر 50 میباشد")]
        [MinLength(3, ErrorMessage = "کمترین کاراکتر 3 میباشد ")]
        public string userName { get; set; }

        [Display(Name = "گذرواژه")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمائید")]
        [MaxLength(20, ErrorMessage = "بیشتری کارکتر 20 میباشد")]
        [MinLength(5, ErrorMessage = "کمترین کاراکتر 5 میباشد ")]
        public string Password { get; set; }
    }
}