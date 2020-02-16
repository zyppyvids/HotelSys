using System.Collections.Generic;
using Web.Models.Shared;

namespace Web.Models.Reservations
{
    public class ReservationsIndexViewModel
    {
        public PagerViewModel Pager;

        public ICollection<ReservationsViewModel> Items { get; set; }
    }
}
