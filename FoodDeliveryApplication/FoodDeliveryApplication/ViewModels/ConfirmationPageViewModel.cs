using System.Linq;
using System.Threading.Tasks;
using FoodDeliveryApplication.Models;
using FoodDeliveryApplication.Repos.Interface;
using FoodDeliveryApplication.Services.Interfaces;
using FoodDeliveryApplication.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms.Maps;

namespace FoodDeliveryApplication.ViewModels
{
    public class ConfirmationPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly ILocationService _locationService;
        private readonly IGenericRepository<FoodCartModel> _genericRepository;
        private ObservableRangeCollection<FoodCartModel> _cartlist;
        private int _totalCost;
        private string _userLocation;
        private string _address;

        public ConfirmationPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IGenericRepository<FoodCartModel> genericRepository, ILocationService locationService) : base(navigationService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _genericRepository = genericRepository;
            _locationService = locationService;
            PayCommand = new DelegateCommand(PayCommandHandler);
            FindUserLocationCommand = new DelegateCommand(FindUserLocationCommandHandler);
            FindLocationCommand = new DelegateCommand(FindLocationCommandHandler);
            RemoveCommand = new DelegateCommand<FoodCartModel>(RemoveCommandHandler);
        }
        public ObservableRangeCollection<FoodCartModel> CartList { get { return _cartlist; } set { SetProperty(ref _cartlist, value); } }
        public DelegateCommand PayCommand { get; set; }
        public DelegateCommand FindUserLocationCommand { get; set; }
        public DelegateCommand FindLocationCommand { get; set; }
        public DelegateCommand<FoodCartModel> RemoveCommand { get; set; }
        public Position Position { get; set; }
        public Pin Pin { get; set; }
        public string UserLocation { get { return _userLocation; } set { SetProperty(ref _userLocation, value); } }
        public string Address { get { return _address; } set { SetProperty(ref _address, value); } }
        public int TotalCost { get { return _totalCost; } set { SetProperty(ref _totalCost, value); } }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            await GetProducts();
        }
        private async Task GetProducts()
        {
            var data = await _genericRepository.Get();
            CartList = new ObservableRangeCollection<FoodCartModel>(data);
            TotalCost = CartList.Sum(c => c.FoodProductPrice);
        }
        private async void RemoveCommandHandler(FoodCartModel removedProduct)
        {
            await _genericRepository.DeleteCart(removedProduct);
            await GetProducts();
        }
        private async void PayCommandHandler()
        {
            var response = await _pageDialogService.DisplayAlertAsync("ConfirmPayment", "Are the Location and Payment Confirmed?", "Yes", "No");
            if (response)
            {
                await _pageDialogService.DisplayAlertAsync("Successful", "PaymentDone", "Return To Products Page");
                await _navigationService.NavigateAsync(nameof(FoodProductsPage));
            }
        }
        private async void FindLocationCommandHandler()
        {
            var location = await _locationService.FindLocationAsync(Address);
            var latitude = location.Latitude;
            var longitude = location.Longitude;
            //var pin = new Pin { Position = position, Label = Address, Type = PinType.Place };
            //Position = position;
            //Pin = pin;
        }
        private async void FindUserLocationCommandHandler()
        {
            UserLocation = await _locationService.GetLocationAsync();
        }
    }
}
