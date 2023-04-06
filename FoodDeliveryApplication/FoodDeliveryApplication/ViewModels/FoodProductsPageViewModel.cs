using FoodDeliveryApplication.Models;
using FoodDeliveryApplication.Repos.Interface;
using FoodDeliveryApplication.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Xamarin.CommunityToolkit.ObjectModel;

namespace FoodDeliveryApplication.ViewModels
{
    public class FoodProductsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IGenericRepository<FoodCartModel> _genericRepository;
        private ObservableRangeCollection<FoodProductModel> _foodProductList;
        public FoodProductsPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IGenericRepository<FoodCartModel> genericRepository) : base(navigationService)
        {
            _navigationService = navigationService;
            _genericRepository = genericRepository;
            _pageDialogService = pageDialogService;
            ConfirmCartCommand = new DelegateCommand(ConfirmCartCommandHandler);
            AddToCartCommand = new DelegateCommand<FoodProductModel>(AddToCartCommandHandler);
            LogoutCommand = new DelegateCommand(LogoutCommandHandler);
        }

        public ObservableRangeCollection<FoodProductModel> FoodProductList { get => _foodProductList; set => SetProperty(ref _foodProductList, value); }
        public DelegateCommand ConfirmCartCommand { get; set; }
        public DelegateCommand LogoutCommand { get; set; }
        public DelegateCommand<FoodProductModel> AddToCartCommand { get; set; }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            FoodProductList = new ObservableRangeCollection<FoodProductModel>(){
                new FoodProductModel { FoodImage = "https://img.onmanorama.com/content/dam/mm/en/food/features/images/2021/10/17/pizza.jpg.transform/schema-16x9/image.jpg", FoodName = "Pizza", FoodPrice = 29, FoodDescription = "The Best Pizza You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://t4.ftcdn.net/jpg/02/74/99/01/360_F_274990113_ffVRBygLkLCZAATF9lWymzE6bItMVuH1.jpg", FoodName = "Burger", FoodPrice = 24, FoodDescription = "The Best Burger You Will Ever Have!!!!" },new FoodProductModel { FoodImage = "https://makeyourmeals.com/wp-content/uploads/2018/08/featured-pizza-rolls.jpg", FoodName = "Pizza Rolls", FoodPrice = 34, FoodDescription = "The Best Pizza Rolls You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://t3.ftcdn.net/jpg/01/91/58/40/360_F_191584072_PaOd2228q4hThUJOtPnjqhGPOPDdebSu.jpg", FoodName = "Sandwich", FoodPrice = 25, FoodDescription = "The Best Sandwich You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://img.freepik.com/free-photo/fresh-ice-tea-plastic-glass_144627-27132.jpg?w=2000", FoodName = "Iced Tea", FoodPrice = 12, FoodDescription = "The Best Iced Tea You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://cdn.pixabay.com/photo/2014/01/24/04/05/fried-chicken-250863__340.jpg", FoodName = "Crispy Fried Chicken", FoodPrice = 40, FoodDescription = "The Best Crispy Fried Chicken You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://blog.kainexus.com/hubfs/French%20fries%202.jpeg", FoodName = "French Fries", FoodPrice = 20, FoodDescription = "The Best French Fries You Will Ever Have!!!!" } };
        }
        private async void AddToCartCommandHandler(FoodProductModel product)
        {
            var cartItem = new FoodCartModel { FoodProductDescription = product.FoodDescription, FoodProductName = product.FoodName, FoodProductPrice = product.FoodPrice };
            await _genericRepository.Insert(cartItem);
        }
        private void LogoutCommandHandler()
        {
            _navigationService.GoBackToRootAsync();
        }
        private async void ConfirmCartCommandHandler()
        { 
            await _navigationService.NavigateAsync(nameof(ConfirmationPage));
        }
    }
}
