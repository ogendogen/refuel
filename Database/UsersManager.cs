using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Database.Models;

namespace Database
{
    public class UsersManager : IUsersManager
    {
        private readonly RefuelContext ctx;

        public UsersManager(RefuelContext refuelContext)
        {
            ctx = refuelContext;
        }

        public async Task<User> Authenticate(string login, string password)
        {
            string salt = GetUserSalt(login);
            if (salt == null)
            {
                return null;
            }
            string hashed = HashPassword(password, salt);
            return await ctx.Users.FirstOrDefaultAsync(user => user.Login == login && user.Password == password);
        }

        public int SaveChanges()
        {
            return ctx.SaveChanges();
        }

        public string GetUserSalt(string login)
        {
            return ctx.Users.FirstOrDefault(user => user.Login == login)?.Salt;
        }

        private string HashPassword(string password, string salt)
        {
            string saltedPassword = $"{salt}{password}{salt}";
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
