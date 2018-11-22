namespace Eventures.Models.AccountViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-z0-9\.\~_\-*]+$", ErrorMessage = "Username may only contains alphanumeric characters, dashes, underscores, dots, asterisks and tildes.")]
        public string Username { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Unique Citizen Number should consist of exactly 10 numbers.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Unique Citizen Number should contains only numbers.")]
        public string UniqueCitizenNumber { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
