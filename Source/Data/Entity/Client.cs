using System.ComponentModel.DataAnnotations;

namespace Data.Entity
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public bool Adult { get; set; }
    }
}
