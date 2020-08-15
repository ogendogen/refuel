using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Utils
{
    public interface IEmailManager
    {
        void SendEmail(Email email);
    }
}