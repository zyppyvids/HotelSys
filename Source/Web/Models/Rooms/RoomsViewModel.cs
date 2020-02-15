namespace Web.Models.Rooms
{
    public class RoomsViewModel
    {
        public int Number { get; set; }

        public string Type { get; set; }

        public int Capacity { get; set; }

        public bool Free { get; set; }

        public float BedPriceAdult { get; set; }

        public float BedPriceChild { get; set; }
    }
}
