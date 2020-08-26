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
        Task<bool> IsLoginUsed(string login);
        Task<bool> IsEmailUsed(string email);
        Task<bool> IsUsersEmailVerified(User user);
        Task<int> VerifyUser(int id, string verificationCode);
        Task<User> RegisterOrLoginGoogleUser(string login, string email);
        List<Vehicle> GetUserAllVehicles(int userId);
    }
}
