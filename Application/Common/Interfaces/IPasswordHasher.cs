using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash (User user, string password);
        bool Verify (User user, string password, string passwordHash);
    }
}
