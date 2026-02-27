using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Domain
{
    public partial class Permission
    {
        public int PermissionId { get; set; }

        public string? PermissionName { get; set; }

        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
