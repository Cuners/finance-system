using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Domain
{
    public partial class User
    {
        public int UserId { get; set; }

        public string? Login { get; set; }

        public string? PassHash { get; set; }

        public string? Email { get; set; }

        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
