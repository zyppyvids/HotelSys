using System.Collections.Generic;
using Web.Models.Shared;

namespace Web.Models.Users
{
    public class UsersIndexViewModel
    {
        public PagerViewModel Pager;

        public string[] Sorts = new string[] { "Username", "FirstName", "MiddleName", "FamilyName", "Email" };
        public int CurrentSort { get; set; }

        public ICollection<UsersViewModel> Items { get; set; }
    }
}
