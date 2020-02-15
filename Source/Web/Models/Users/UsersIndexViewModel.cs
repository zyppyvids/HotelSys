using System.Collections.Generic;
using Web.Models.Shared;

namespace Web.Models.Users
{
    public class UsersIndexViewModel
    {
        public PagerViewModel Pager;

        public ICollection<UsersViewModel> Items { get; set; }
    }
}
