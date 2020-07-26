using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Database.Models;

namespace Database
{
    public interface IUsersManager
    {
        Task<User> Authenticate(string login, string password);
        int SaveChanges();
        string GetUserSalt(string login);
        Task<User> RegisterNewUser(string login, string password, string email);
    }
}
