using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases
{
    public class UserInfo
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Permissions { get; set; }
    }
}
