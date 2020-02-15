using System;

namespace Data.Entity
{
    public class Room
    {
        public int Number { get; set; }

        public string type { get; set; }

        public int Capacity { get; set; }

        public bool Free { get; set; }

        public float BedPriceAdult { get; set; }

        public float BedPriceChild { get; set; }
    }
}
