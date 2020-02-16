using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Entity
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        public int RoomNumber { get; set; }

        public int UserId { get; set; }

        public string ClientsIds { get; set; }

        public DateTime DateAccomodation { get; set; }

        public DateTime DateRelease { get; set; }

        public bool BreakfastIncluded { get; set; }

        public bool AllInclusive { get; set; }

        public float PaymentAmount { get; set; }
    }
}
