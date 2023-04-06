using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FoodDeliveryApplication.Services;
using FoodDeliveryApplication.Services.Interfaces;
using Moq;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;

namespace FoodDeliveryApplicationUnitTest.ServiceTest
{

    public class LocationServiceTest
    {
        private readonly Fixture _fixture;
        private readonly MockRepository _mockRepository;
        private readonly ILocationService _locationService;
        private readonly Mock<IGeolocation> _geolocationMock;
        private readonly Mock<IGeocoding> _geocodingMock;

        public LocationServiceTest()
        {
            _fixture = new Fixture();
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _geolocationMock = _mockRepository.Create<IGeolocation>();
            _geocodingMock = _mockRepository.Create<IGeocoding>();
            _locationService = new LocationService(_geolocationMock.Object, _geocodingMock.Object);
        }

        [Fact]
        public void Create_Service()
        {
            // Arrange

            // Act

            // Assert
            _locationService.Should().NotBeNull();

            _mockRepository.VerifyAll();
            _mockRepository.VerifyNoOtherCalls();
        }
        [Fact]
        public async Task ShouldReturnCurrentLocationOfUser_WhenCalledGetLocationAsync()
        {
            //Arrange
            var location = _fixture.Create<Location>();
            _geolocationMock.Setup(x => x.GetLastKnownLocationAsync()).ReturnsAsync(location);
            var place = _fixture.Create<IEnumerable<Placemark>>();
            var address = place.FirstOrDefault();
            _geocodingMock.Setup(x => x.GetPlacemarksAsync(location.Latitude, location.Longitude)).ReturnsAsync(place);

            //Act
            var userLocation = await _locationService.GetLocationAsync();

            //Assert
            userLocation.Should().NotBeNull();
            userLocation.Should().BeEquivalentTo(address.FeatureName.ToString() + ", " + address.SubLocality.ToString() + ", " + address.Locality.ToString() + ", " + address.AdminArea + ", " + address.PostalCode.ToString());
            _mockRepository.VerifyAll();
            _mockRepository.VerifyNoOtherCalls();
        }
        [Fact]
        public async Task ShouldReturnNull_WhenCalledGetLocationAsync()
        {
            //Arrange
            var location = _fixture.Create<Location>();
            location = null;
            _geolocationMock.Setup(x => x.GetLastKnownLocationAsync()).ReturnsAsync(location);

            //Act
            var userLocation = await _locationService.GetLocationAsync();

            //Assert
            userLocation.Should().BeNull();
            _mockRepository.VerifyAll();
            _mockRepository.VerifyNoOtherCalls();
        }
    }
}
