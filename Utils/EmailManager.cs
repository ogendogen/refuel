using System;
using System.Threading.Tasks;
using Database.Models;

namespace Utils
{
    public class EmailManager : IEmailManager
    {
        public Task<bool> SendVerificationEmail(User user)
        {
            throw new NotImplementedException();
        }
    }
}
