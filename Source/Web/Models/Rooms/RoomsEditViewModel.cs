using System.ComponentModel.DataAnnotations;

namespace Web.Models.Rooms
{
    public class RoomsEditViewModel
    {
        [Required]
        public int Number { get; set; }

        [Required]
        [MaxLength(80, ErrorMessage = "Type cannot be longer than 80 characters")]
        public string Type { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public bool Free { get; set; }

        [Required]
        public float BedPriceAdult { get; set; }

        [Required]
        public float BedPriceChild { get; set; }
    }
}
