using Microsoft.EntityFrameworkCore;
using RetroStore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroStore.DataAccess {
    public class ApplicationDbContext : DbContext {

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Product> Products { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }
    }
}
