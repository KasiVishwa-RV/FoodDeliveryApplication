using System.Diagnostics.CodeAnalysis;
using Flurl.Http;
using FoodDeliveryApplication.Repos;
using FoodDeliveryApplication.Repos.Interface;
using FoodDeliveryApplication.Services;
using FoodDeliveryApplication.Services.Interfaces;
using FoodDeliveryApplication.ViewModels;
using FoodDeliveryApplication.Views;
using Prism;
using Prism.Ioc;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace FoodDeliveryApplication
{
    [ExcludeFromCodeCoverage]
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            await NavigationService.NavigateAsync("NavigationPage/LoginPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.RegisterSingleton<IGeolocation, GeolocationImplementation>();
            containerRegistry.RegisterSingleton<IGeocoding, GeocodingImplementation>();
            containerRegistry.RegisterSingleton<ILocationService, LocationService>();
            containerRegistry.Register(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<FoodProductsPage, FoodProductsPageViewModel>();
            containerRegistry.RegisterForNavigation<SignupPage, SignupPageViewModel>();
            containerRegistry.RegisterForNavigation<ConfirmationPage, ConfirmationPageViewModel>();
        }
    }
}
