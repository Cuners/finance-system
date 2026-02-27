using Auth.Application.Services;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Auth.Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        public string Hash(string password)
        {
            return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
        }

        public bool Verify(string hashedPassword, string password)
        {
            var computedHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
            return hashedPassword == computedHash;
        }
    }
}
