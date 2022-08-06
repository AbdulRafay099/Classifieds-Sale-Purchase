using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApniShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int Stock { get; set; }
      
        public int Demand { get; set; }
        public byte[] Image { get; set; }
        [Range(1, 5, ErrorMessage = "Rating must be between 1-5.")]
        public int Rating { get; set; }

    }
}
