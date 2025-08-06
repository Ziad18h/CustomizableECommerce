using System.ComponentModel.DataAnnotations;

namespace CustomizableECommerce.Models.ViewModels
{
    public class LoginVM
    {
        public int Id { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string EmailOrUserName { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; } = false;
    }
}
