using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RetroStore.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroStore.Models.Models {
    public class Product {

        public int Id { get; set; }

        [DisplayName("Title")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = null!;

        [Required]
        [DisplayName("Genre")]
        public int GenreId { get; set; }

        [ValidateNever]
        public Genre Genre { get; set; } = null!;

        [DisplayName("List Price")]
        [Required]
        [Range(1, 1000)]
        public double ListPrice { get; set; }

        [Required]
        [Range(1, 1000)]
        public double Price { get; set; }

        [DisplayName("Release Date")]
        [Required]
        [Column(TypeName = "date")]
        public DateTime ReleaseDate { get; set; }

        [ValidateNever]
        [NotMapped]
        public string ReleaseDateWithoutTime { get; set; } = null!;

        [Required]
        public string Developer { get; set; } = null!;

        [Required]
        public string Publisher { get; set; } = null!;

        [Required]
        public Platform Platform { get; set; }

        [NotMapped]
        [ValidateNever]
        public string PlatformName { get; set; } = null!;

        [DisplayName("Image")]
        public string? ImageUrl { get; set; }

        public override bool Equals(object? obj) {
            return obj is Product product &&
                   Name == product.Name &&
                   Description == product.Description &&
                   GenreId == product.GenreId &&
                   EqualityComparer<Genre>.Default.Equals(Genre, product.Genre) &&
                   ListPrice == product.ListPrice &&
                   Price == product.Price &&
                   ReleaseDate == product.ReleaseDate &&
                   Developer == product.Developer &&
                   Publisher == product.Publisher &&
                   Platform == product.Platform &&
                   ImageUrl == product.ImageUrl;
        }

        public override int GetHashCode() {
            HashCode hash = new HashCode();
            hash.Add(Name);
            hash.Add(Description);
            hash.Add(GenreId);
            hash.Add(Genre);
            hash.Add(ListPrice);
            hash.Add(Price);
            hash.Add(ReleaseDate);
            hash.Add(Developer);
            hash.Add(Publisher);
            hash.Add(Platform);
            hash.Add(ImageUrl);
            return hash.ToHashCode();
        }
    }
}
