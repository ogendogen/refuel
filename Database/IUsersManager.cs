using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    public interface IUsersManager
    {
        public bool Authenticate(string login, string password);
        public int SaveChanges();
        public string GetUserSalt(string login);
    }
}
