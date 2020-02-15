using System.Collections.Generic;
using Web.Models.Shared;

namespace Web.Models.Rooms
{
    public class RoomsIndexViewModel
    {
        public PagerViewModel Pager;

        public ICollection<RoomsViewModel> Items { get; set; }
    }
}
