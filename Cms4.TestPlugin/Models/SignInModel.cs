using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Cms4.TestPlugin.Models
{
    public class SignInModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ReturnUrl { get; set; }
    }
}
