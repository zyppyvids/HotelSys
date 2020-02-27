using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Entity
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string PersonalID { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public DateTime DateAppointment { get; set; }

        public bool Active { get; set; }

        public DateTime? DateDismissal { get; set; }
    }
}
