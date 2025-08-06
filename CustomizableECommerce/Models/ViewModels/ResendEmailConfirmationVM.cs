using System.ComponentModel.DataAnnotations;

namespace CustomizableECommerce.Models.ViewModels
{
    public class ResendEmailConfirmationVM
    {
      public  int Id { get; set; }

        [Required]
        public string EmailOrUserName { get; set; } = string.Empty;



    }
}
