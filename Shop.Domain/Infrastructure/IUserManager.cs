using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Infrastructure
{
    public interface IUserManager
    {
        Task<bool> CreateUserAsync(string userName, string password);
        Task<bool> AddClaimToUserAsync(string userName, string type, string value);
    }
}
