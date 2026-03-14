using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.Services
{
    public interface ICurrentUserService
    {
        int UserId { get; }
    }
}
