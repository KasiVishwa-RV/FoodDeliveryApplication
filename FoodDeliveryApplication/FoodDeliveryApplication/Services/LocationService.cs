using System.Linq;
using System.Threading.Tasks;
using FoodDeliveryApplication.Services.Interfaces;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms.Maps;

namespace FoodDeliveryApplication.Services
{
    public class LocationService : ILocationService
    {
        private readonly IGeocoding _geocoding;
        private readonly IGeolocation _geolocation;
        public LocationService(IGeolocation geolocation, IGeocoding geocoding)
        {
            _geocoding = geocoding;
            _geolocation = geolocation;
        }
        public async Task<string> GetLocationAsync()
        {
            Location location = await _geolocation.GetLastKnownLocationAsync();
            if (location != null)
            {
                var place = await _geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                var address = place?.FirstOrDefault();
                var userLocation = address.FeatureName + ", " + address.SubLocality + ", " + address.Locality + ", " + address.AdminArea + ", " + address.PostalCode;
                return userLocation;
            }
            return null;
        }

        public async Task<Location> FindLocationAsync(string address)
        {
            var locations = await _geocoding.GetLocationsAsync(address);
            return locations.FirstOrDefault();
        }
    }
}
