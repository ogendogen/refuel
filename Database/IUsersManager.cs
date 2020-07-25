using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public interface IUsersManager
    {
        Task<bool> Authenticate(string login, string password);
        int SaveChanges();
        string GetUserSalt(string login);
    }
}
