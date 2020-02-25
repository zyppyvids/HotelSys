using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Reservations
{
    public class ReservationsViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        public int RoomNumber { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string[] ClientsNames { get; set; }

        [Required]
        public String DateAccomodation { get; set; }

        [Required]
        public String DateRelease { get; set; }


        public bool BreakfastIncluded { get; set; }

        public bool AllInclusive { get; set; }

        [Required]
        public float PaymentAmount { get; set; }
    }
}
