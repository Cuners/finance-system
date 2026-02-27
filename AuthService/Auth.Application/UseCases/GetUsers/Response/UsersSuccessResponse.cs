using Auth.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.UseCases.GetUsers.Response
{
    public class UsersSuccessResponse : UsersResponse
    {
        public IEnumerable<User> Users { get; set; }
        public UsersSuccessResponse(IEnumerable<User> users)
        {
            Users = users;
        }
    }
}
