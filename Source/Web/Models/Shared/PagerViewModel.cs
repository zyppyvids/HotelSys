namespace Web.Models.Shared
{
    public class PagerViewModel
    {
        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public int PageSize { get; set; }

        public int[] PageSizes = new int[3] { 10, 25, 50 };
    }
}
