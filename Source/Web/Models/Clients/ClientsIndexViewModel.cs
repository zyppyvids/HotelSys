using System.Collections.Generic;

namespace Web.Models.Clients
{
    public class ClientsIndexViewModel
    {
        public PagerViewModel Pager;

        public ICollection<ClientsViewModel> Items { get; set; }
    }
}
