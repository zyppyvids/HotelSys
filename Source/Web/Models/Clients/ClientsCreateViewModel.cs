using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Web.Models.Clients
{
    public class ClientsCreateViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        [MaxLength(80, ErrorMessage = "Name cannot be longer than 80 characters")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(80, ErrorMessage = "Name cannot be longer than 80 characters")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "The phone number must contain only numbers")]
        [StringLength(10, ErrorMessage = "Phone number must be exactly 10 characters", MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }

        [Required]
        public bool Adult { get; set; }
    }
}
