using System.Threading.Tasks;
using RMDesktopUI.Library.Models;
using RMDesktopUI.Models;

namespace RMDesktopUI.Library.API 
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);

        Task<LoggedInUserModel> GetLoggedInUserInfo(string token);
    }
}