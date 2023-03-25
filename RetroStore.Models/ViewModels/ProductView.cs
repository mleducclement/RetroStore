using Microsoft.AspNetCore.Mvc.Rendering;
using RetroStore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroStore.Models.ViewModels {
    public class ProductView {
        public Product Product { get; set; } = new Product();

        [ValidateNever]
        public IEnumerable<SelectListItem> GenreListItems { get; set; } = new List<SelectListItem>();
    }
}
