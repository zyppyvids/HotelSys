using System.Collections.Generic;
using Web.Models.Shared;

namespace Web.Models.Clients
{
    public class ClientsIndexViewModel
    {
        public PagerViewModel Pager;

        public string[] Sorts = new string[] { "FirstName", "FamilyName" };
        public int CurrentSort { get; set; }

        public ICollection<ClientsViewModel> Items { get; set; }
    }
}
