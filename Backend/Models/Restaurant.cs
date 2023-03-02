using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public partial class Restaurant
    {
       
        public int RestaurantId { get; set; }
       
        [Required]
       
        public string RestaurantName { get; set; }
        [Required]
        public string Address { get; set; }

        public ICollection<FoodBox> FoodBoxes { get; set; }
            

    }
}
