using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Reservations
{
    public class ReservationsCreateViewModel
    {
        [Required]
        public int RoomNumber { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int[] ClientsIds { get; set; }

        [Required]
        public DateTime DateAccomodation { get; set; }

        [Required]
        public DateTime DateRelease { get; set; }

        [Required]
        public bool BreakfastIncluded { get; set; }

        [Required]
        public bool AllInclusive { get; set; }

        [Required]
        public float PaymentAmount { get; set; }
    }
}
