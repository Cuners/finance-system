using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Application.Services
{
    public interface IPasswordService
    {
        string Hash(string password);
        bool Verify(string hashedPassword, string password);
    }
}
