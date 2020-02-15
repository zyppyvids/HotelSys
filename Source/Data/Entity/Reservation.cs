using System;

namespace Data.Entity
{
    public class Reservation
    {
        public int RoomNumber { get; set; }

        public int UserId { get; set; }

        public int[] ClientIds { get; set; }

        public DateTime DateAccomodation { get; set; }

        public DateTime DateRelease { get; set; }

        public bool BreakfastIncluded { get; set; }

        public bool AllInclusive { get; set; }

        public float PaymentAmount { get; set; }
    }
}
