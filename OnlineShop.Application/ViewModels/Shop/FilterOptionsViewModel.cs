namespace OnlineShop.Application.ViewModels.Shop
{
    public class FilterOptionsViewModel
    {
        public int CurrentPage { get; set; }

        public string SearchInput { get; set; }

		public List<string> PriceRanges { get; set; } = new List<string>();

		public List<string> Categories { get; set; } = new List<string>();

		public bool InStock { get; set; }

		public string SortByPrice { get; set; }
	}
}
