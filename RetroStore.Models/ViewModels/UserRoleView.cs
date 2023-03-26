using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroStore.Models.ViewModels {
    public class UserRoleView {
        public IdentityUser User { get; set; } = null!;
        public ICollection<string> Roles { get; set; } = null!;
        public ICollection<IdentityRole> AllRoles { get; set; } = null!;
    }
}
