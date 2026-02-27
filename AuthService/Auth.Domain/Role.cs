using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Domain
{
    public partial class Role
    {
        public int RoleId { get; set; }

        public string? RoleName { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
