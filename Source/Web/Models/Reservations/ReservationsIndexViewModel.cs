using System.Collections.Generic;

namespace Web.Models.Reservations
{
    public class ReservationsIndexViewModel
    {
        public PagerViewModel Pager;

        public ICollection<ReservationsViewModel> Items { get; set; }
    }
}
