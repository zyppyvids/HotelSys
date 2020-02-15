using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Web.Models.Users
{
    public class UsersEditViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        [MaxLength(80, ErrorMessage = "Username cannot be longer than 80 characters")]
        public string Username { get; set; }

        [Required]
        [StringLength(18, ErrorMessage = "The password must be at least 6 characters", MinimumLength = 6)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MaxLength(80, ErrorMessage = "Name cannot be longer than 80 characters")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(80, ErrorMessage = "Name cannot be longer than 80 characters")]
        public string MiddleName { get; set; }

        [Required]
        [MaxLength(80, ErrorMessage = "Name cannot be longer than 80 characters")]
        public string LastName { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Personal ID number must be exactly 10 characters", MinimumLength = 10)]
        public string PersonalID { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Phone number must be exactly 10 characters", MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }

        [Required]
        public DateTime DateAppointment { get; set; }

        [Required]
        public bool Active { get; set; }

        public DateTime DateDismissal { get; set; }
    }
}
