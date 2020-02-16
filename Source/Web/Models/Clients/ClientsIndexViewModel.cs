using System.Collections.Generic;
using Web.Models.Shared;

namespace Web.Models.Clients
{
    public class ClientsIndexViewModel
    {
        public PagerViewModel Pager;

        public ICollection<ClientsViewModel> Items { get; set; }
    }
}
