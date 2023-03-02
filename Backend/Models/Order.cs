using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend
{
    public partial class Order
    {

        public int? OrderId { get; set; }
        [Required]
        public Customer Customer{ get; set; }
       
        [Required]

        [Column(TypeName = "date")]
        public DateTime OrderDate { get; set; }
     

       // public int FoodboxId { get; set; }
        public ICollection<FoodBox> Foodbox { get; set; }  

    }
}
