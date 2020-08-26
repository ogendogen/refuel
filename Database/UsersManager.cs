using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Database.Models;
using Utils;

namespace Database
{
    public class UsersManager : IUsersManager
    {
        private readonly RefuelContext _ctx;

        public UsersManager(RefuelContext refuelContext)
        {
            _ctx = refuelContext;
        }

        public async Task<User> Authenticate(string login, string password)
        {
            string salt = GetUserSalt(login);
            if (salt == null)
            {
                return null;
            }
            string hashed = HashPassword(password, salt);
            return await _ctx.Users.FirstOrDefaultAsync(user => user.Login == login && user.Password == hashed && String.IsNullOrEmpty(user.ExternalProvider));
        }

        public int SaveChanges()
        {
            return _ctx.SaveChanges();
        }

        public string GetUserSalt(string login)
        {
            return _ctx.Users.FirstOrDefault(user => user.Login == login)?.Salt;
        }

        public async Task<User> RegisterNewUser(string login, string password, string email)
        {
            string salt = GenerateSalt();
            string hashedPassword = HashPassword(password, salt);

            var entityUser = await _ctx.AddAsync(new User()
            {
                Login = login,
                Password = hashedPassword,
                Salt = salt,
                Email = email,
                RegisterDate = DateTime.Now,
                VerificationCode = Utils.Utils.GenerateRandomString(32)
            });
            
            return entityUser.Entity;
        }

        public async Task<bool> IsLoginUsed(string login)
        {
            return await _ctx.Users.FirstOrDefaultAsync(user => user.Login == login) != null;
        }

        public async Task<bool> IsEmailUsed(string email)
        {
            return await _ctx.Users.FirstOrDefaultAsync(user => user.Email == email) != null;
        }

        private string GenerateSalt()
        {
            int size = RNGCryptoServiceProvider.GetInt32(1, 21);
            return Utils.Utils.GenerateRandomString(size);
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

        public async Task<bool> IsUsersEmailVerified(User user)
        {
            var dbUser = await _ctx.Users.FirstOrDefaultAsync(dbUser => user.ID == dbUser.ID);
            return dbUser != null && dbUser.Email == "0";
        }

        public async Task<int> VerifyUser(int id, string verificationCode)
        {
            User user = await _ctx.Users.FirstOrDefaultAsync(user => user.ID == id);
            if (user == null)
            {
                return 0;
            }

            if (user.VerificationCode != "0" && user.VerificationCode == verificationCode)
            {
                user.VerificationCode = "0";
                _ctx.Users.Update(user);
            }

            return await _ctx.SaveChangesAsync();
        }

        public async Task<User> RegisterOrLoginGoogleUser(string login, string email)
        {
            var dbUser = await _ctx.Users.FirstOrDefaultAsync(user => user.Login == login && user.Email == email && user.ExternalProvider == "google");
            if (dbUser != null)
            {
                return dbUser;
            }

            var entityUser = await _ctx.Users.AddAsync(new User()
            {
                Login = login,
                Email = email,
                RegisterDate = DateTime.Now,
                VerificationCode = "0",
                ExternalProvider = "google"
            });
            await _ctx.SaveChangesAsync();

            return entityUser.Entity;
        }

        public List<Vehicle> GetUserAllVehicles(int userId)
        {
            return _ctx.Vehicles.Where(vehicle => vehicle.Owner.ID == userId).ToList();
        }
    }
}
