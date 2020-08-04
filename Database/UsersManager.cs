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
        private readonly RandomNumberGenerator rng;

        public UsersManager(RefuelContext refuelContext)
        {
            ctx = refuelContext;
            rng = RNGCryptoServiceProvider.Create();
        }

        public async Task<User> Authenticate(string login, string password)
        {
            string salt = GetUserSalt(login);
            if (salt == null)
            {
                return null;
            }
            string hashed = HashPassword(password, salt);
            return await ctx.Users.FirstOrDefaultAsync(user => user.Login == login && user.Password == hashed);
        }

        public int SaveChanges()
        {
            return ctx.SaveChanges();
        }

        public string GetUserSalt(string login)
        {
            return ctx.Users.FirstOrDefault(user => user.Login == login)?.Salt;
        }

        public async Task<User> RegisterNewUser(string login, string password, string email)
        {
            string salt = GenerateSalt();
            string hashedPassword = HashPassword(password, salt);

            var entityUser = await ctx.AddAsync(new User()
            {
                Login = login,
                Password = hashedPassword,
                Salt = salt,
                Email = email,
                RegisterDate = DateTime.Now
            });
            
            return entityUser.Entity;
        }

        public async Task<bool> IsLoginUsed(string login)
        {
            return await ctx.Users.FirstOrDefaultAsync(user => user.Login == login) != null;
        }

        public async Task<bool> IsEmailUsed(string email)
        {
            return await ctx.Users.FirstOrDefaultAsync(user => user.Email == email) != null;
        }

        private string GenerateSalt()
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890~!@#$%^&*()_+{}|:\"<>?`-=[]\\;',./";

            int size = RNGCryptoServiceProvider.GetInt32(1, 21);
            int length = chars.Length;
            StringBuilder salt = new StringBuilder();
            for (int i=0; i<size; i++)
            {
                int choosen = RNGCryptoServiceProvider.GetInt32(length);
                salt.Append(chars[choosen]);
            }

            return salt.ToString();
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
