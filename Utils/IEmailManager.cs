using System.Reflection.Metadata;
using System.Threading.Tasks;
using Database.Models;

namespace Utils
{
    public interface IEmailManager
    {
        Task<bool> SendVerificationEmail(User user);
    }
}