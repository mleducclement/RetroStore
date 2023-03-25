using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroStore.Models.Models {
    public class Genre {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        public override bool Equals(object? obj) {
            return obj is Genre genre &&
                   Id == genre.Id &&
                   Name == genre.Name;
        }

        public override int GetHashCode() {
            return HashCode.Combine(Id, Name);
        }
    }
}
