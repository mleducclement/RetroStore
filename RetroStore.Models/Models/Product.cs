using RetroStore.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroStore.Models.Models {
    public class Product {

        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = null!;

        [Required]
        public Genre Genre { get; set; } = null!;

        [Required]
        [Range(1, 1000)]
        public double Price { get; set; }

        [Required]
        public string Developer { get; set; } = null!;

        [Required]
        public string Publisher { get; set; } = null!;

        [Required]
        public Platform Platform { get; set; }

        [Required]
        public string ImageUrl { get; set; } = null!;
    }
}
