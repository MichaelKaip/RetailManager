using System.Threading.Tasks;
using RMDesktopUI.Library.Models;

namespace RMDesktopUI.Library.API
{
    public interface ISaleEndPoint
    {
        Task PostSale(SaleModel sale);
    }
}