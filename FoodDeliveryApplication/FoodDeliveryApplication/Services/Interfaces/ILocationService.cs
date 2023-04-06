using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace FoodDeliveryApplication.Services.Interfaces
{
    public interface ILocationService
    {
        Task<string> GetLocationAsync();
        Task<Location> FindLocationAsync(string location);
    }
}
