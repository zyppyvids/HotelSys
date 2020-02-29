using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Reservations
{
    public class ReservationsEditViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public int[] AllAvailableRooms { get; set; }

        [HiddenInput]
        public String[] AllAvailableRoomsTypes { get; set; }

        [HiddenInput]
        public int RoomNumber { get; set; }

        [HiddenInput]
        public string[] AllUsersNames { get; set; }

        [HiddenInput]
        public int[] AllUsersIds { get; set; }

        [HiddenInput]
        public int UserId { get; set; }

        [HiddenInput]
        public string[] AllClientsNames { get; set; }

        [HiddenInput]
        public int[] AllClientsIds { get; set; }

        [HiddenInput]
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
