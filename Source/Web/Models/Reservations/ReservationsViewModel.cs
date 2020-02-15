using System;

namespace Web.Models.Reservations
{
    public class ReservationsViewModel
    {
        public int RoomNumber { get; set; }

        public int UserId { get; set; }

        public int[] ClientsIds { get; set; }

        public DateTime DateAccommodation { get; set; }

        public DateTime DateRelease { get; set; }

        public bool BreakfastIncluded { get; set; }

        public bool AllInclusive { get; set; }

        public float PaymentAmount { get; set; }
    }
}
