using System.ComponentModel.DataAnnotations;

namespace Backend
{
    public partial class FoodBox
    {

        public int FoodBoxId { get; set; }
        [Required]
        public Restaurant Restaurant { get; set; }
        public string FoodName { get; set; }
        public string Type { get; set; }

        public decimal Price { get; set; }
        public Order Order { get; set; }

     

    }
}
