using AutoFixture;
using FluentAssertions;
using FoodDeliveryApplication.Models;
using FoodDeliveryApplication.Repos.Interface;
using FoodDeliveryApplication.ViewModels;
using FoodDeliveryApplication.Views;
using FoodDeliveryApplicationUnitTest.Helper;
using Moq;
using Prism.Navigation;
using Prism.Services;
using Xamarin.CommunityToolkit.ObjectModel;

namespace FoodDeliveryApplicationUnitTest.ViewModelTests
{
    public class FoodProductsPageViewModelTest
    {
        private readonly Fixture _fixture;
        private readonly MockRepository _mockRepository;
        private readonly Mock<IPageDialogService> _pageDialogServiceMock;
        private readonly Mock<IGenericRepository<FoodCartModel>> _genericRepositoryMock;
        private readonly Mock<INavigationService> _navigationServiceMock;

        public FoodProductsPageViewModelTest()
        {
            _fixture = new Fixture();
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _genericRepositoryMock = _mockRepository.Create<IGenericRepository<FoodCartModel>>();
            _navigationServiceMock = _mockRepository.Create<INavigationService>();
            _pageDialogServiceMock = _mockRepository.Create<IPageDialogService>();
        }

        private FoodProductsPageViewModel CreateViewModel()
        {
            return new FoodProductsPageViewModel(_navigationServiceMock.Object, _pageDialogServiceMock.Object, _genericRepositoryMock.Object);
        }

        [Fact]
        public void ShouldNotThrowAnyException_WhenFoodProductsPageViewModelConstructor_IsCalled()
        {
            Action action = () => CreateViewModel();
            action.Should().NotThrow<Exception>();
        }

        [Fact]
        public void ShoulStoreFoodProductList_WhenCalled_OnNavigatedTo()
        {
            //Arrange
            var foodProductsPageViewModel = CreateViewModel();
            var navigationParameters = NavigationHelper.GetNavigationParameters();
            var products = new ObservableRangeCollection<FoodProductModel>(){
                new FoodProductModel { FoodImage = "https://img.onmanorama.com/content/dam/mm/en/food/features/images/2021/10/17/pizza.jpg.transform/schema-16x9/image.jpg", FoodName = "Pizza", FoodPrice = 29, FoodDescription = "The Best Pizza You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://t4.ftcdn.net/jpg/02/74/99/01/360_F_274990113_ffVRBygLkLCZAATF9lWymzE6bItMVuH1.jpg", FoodName = "Burger", FoodPrice = 24, FoodDescription = "The Best Burger You Will Ever Have!!!!" },new FoodProductModel { FoodImage = "https://makeyourmeals.com/wp-content/uploads/2018/08/featured-pizza-rolls.jpg", FoodName = "Pizza Rolls", FoodPrice = 34, FoodDescription = "The Best Pizza Rolls You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://t3.ftcdn.net/jpg/01/91/58/40/360_F_191584072_PaOd2228q4hThUJOtPnjqhGPOPDdebSu.jpg", FoodName = "Sandwich", FoodPrice = 25, FoodDescription = "The Best Sandwich You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://img.freepik.com/free-photo/fresh-ice-tea-plastic-glass_144627-27132.jpg?w=2000", FoodName = "Iced Tea", FoodPrice = 12, FoodDescription = "The Best Iced Tea You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://cdn.pixabay.com/photo/2014/01/24/04/05/fried-chicken-250863__340.jpg", FoodName = "Crispy Fried Chicken", FoodPrice = 40, FoodDescription = "The Best Crispy Fried Chicken You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://blog.kainexus.com/hubfs/French%20fries%202.jpeg", FoodName = "French Fries", FoodPrice = 20, FoodDescription = "The Best French Fries You Will Ever Have!!!!" } };

            //Act
            foodProductsPageViewModel.OnNavigatedTo(navigationParameters);

            //Assert
            foodProductsPageViewModel.FoodProductList.Should().NotBeNull();
            foodProductsPageViewModel.FoodProductList.Should().BeEquivalentTo(products);
            _mockRepository.VerifyAll();
            _mockRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldAddTheSelecteditemToTheFoodCart_WhenExecute_AddToCartCommand()
        {
            //Arrange
            var foodProductsPageViewModel = CreateViewModel();
            var navigationParameters = NavigationHelper.GetNavigationParameters();
            var products = new ObservableRangeCollection<FoodProductModel>(){
                new FoodProductModel { FoodImage = "https://img.onmanorama.com/content/dam/mm/en/food/features/images/2021/10/17/pizza.jpg.transform/schema-16x9/image.jpg", FoodName = "Pizza", FoodPrice = 29, FoodDescription = "The Best Pizza You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://t4.ftcdn.net/jpg/02/74/99/01/360_F_274990113_ffVRBygLkLCZAATF9lWymzE6bItMVuH1.jpg", FoodName = "Burger", FoodPrice = 24, FoodDescription = "The Best Burger You Will Ever Have!!!!" },new FoodProductModel { FoodImage = "https://makeyourmeals.com/wp-content/uploads/2018/08/featured-pizza-rolls.jpg", FoodName = "Pizza Rolls", FoodPrice = 34, FoodDescription = "The Best Pizza Rolls You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://t3.ftcdn.net/jpg/01/91/58/40/360_F_191584072_PaOd2228q4hThUJOtPnjqhGPOPDdebSu.jpg", FoodName = "Sandwich", FoodPrice = 25, FoodDescription = "The Best Sandwich You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://img.freepik.com/free-photo/fresh-ice-tea-plastic-glass_144627-27132.jpg?w=2000", FoodName = "Iced Tea", FoodPrice = 12, FoodDescription = "The Best Iced Tea You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://cdn.pixabay.com/photo/2014/01/24/04/05/fried-chicken-250863__340.jpg", FoodName = "Crispy Fried Chicken", FoodPrice = 40, FoodDescription = "The Best Crispy Fried Chicken You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://blog.kainexus.com/hubfs/French%20fries%202.jpeg", FoodName = "French Fries", FoodPrice = 20, FoodDescription = "The Best French Fries You Will Ever Have!!!!" } };

            var cartItem = products[new Random().Next(0, 2)];
            _genericRepositoryMock.Setup(x => x.Insert(It.IsAny<FoodCartModel>())).ReturnsAsync(1);

            //Act
            foodProductsPageViewModel.OnNavigatedTo(navigationParameters);
            foodProductsPageViewModel.AddToCartCommand.Execute(cartItem);

            //Assert
            foodProductsPageViewModel.FoodProductList.Should().NotBeNull();
            foodProductsPageViewModel.FoodProductList.Should().BeEquivalentTo(products);
            _genericRepositoryMock.Verify(x => x.Insert(It.IsAny<FoodCartModel>()), Times.Once);
            _mockRepository.VerifyAll();
            _mockRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldGoBackToRootPage_WhenExecute_LogoutCommand()
        {
            //Arrange
            var foodProductsPageViewModel = CreateViewModel();
            var navigationParameters = NavigationHelper.GetNavigationParameters();
            var products = new ObservableRangeCollection<FoodProductModel>(){
                new FoodProductModel { FoodImage = "https://img.onmanorama.com/content/dam/mm/en/food/features/images/2021/10/17/pizza.jpg.transform/schema-16x9/image.jpg", FoodName = "Pizza", FoodPrice = 29, FoodDescription = "The Best Pizza You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://t4.ftcdn.net/jpg/02/74/99/01/360_F_274990113_ffVRBygLkLCZAATF9lWymzE6bItMVuH1.jpg", FoodName = "Burger", FoodPrice = 24, FoodDescription = "The Best Burger You Will Ever Have!!!!" },new FoodProductModel { FoodImage = "https://makeyourmeals.com/wp-content/uploads/2018/08/featured-pizza-rolls.jpg", FoodName = "Pizza Rolls", FoodPrice = 34, FoodDescription = "The Best Pizza Rolls You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://t3.ftcdn.net/jpg/01/91/58/40/360_F_191584072_PaOd2228q4hThUJOtPnjqhGPOPDdebSu.jpg", FoodName = "Sandwich", FoodPrice = 25, FoodDescription = "The Best Sandwich You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://img.freepik.com/free-photo/fresh-ice-tea-plastic-glass_144627-27132.jpg?w=2000", FoodName = "Iced Tea", FoodPrice = 12, FoodDescription = "The Best Iced Tea You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://cdn.pixabay.com/photo/2014/01/24/04/05/fried-chicken-250863__340.jpg", FoodName = "Crispy Fried Chicken", FoodPrice = 40, FoodDescription = "The Best Crispy Fried Chicken You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://blog.kainexus.com/hubfs/French%20fries%202.jpeg", FoodName = "French Fries", FoodPrice = 20, FoodDescription = "The Best French Fries You Will Ever Have!!!!" } };
            _navigationServiceMock.Setup(x => x.GoBackToRootAsync(null)).ReturnsAsync(_fixture.Create<NavigationResult>());

            //Act
            foodProductsPageViewModel.OnNavigatedTo(navigationParameters);
            foodProductsPageViewModel.LogoutCommand.Execute();

            //Assert
            foodProductsPageViewModel.FoodProductList.Should().NotBeNull();
            foodProductsPageViewModel.FoodProductList.Should().BeEquivalentTo(products);
            _navigationServiceMock.Verify(x => x.GoBackToRootAsync(null), Times.Once);
            _mockRepository.VerifyAll();
            _mockRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldNavigateToConfirmationPage_WhenExecute_ConfirmCartCommand()
        {
            //Arrange
            var foodProductsPageViewModel = CreateViewModel();
            var navigationParameters = NavigationHelper.GetNavigationParameters();
            var products = new ObservableRangeCollection<FoodProductModel>(){
                new FoodProductModel { FoodImage = "https://img.onmanorama.com/content/dam/mm/en/food/features/images/2021/10/17/pizza.jpg.transform/schema-16x9/image.jpg", FoodName = "Pizza", FoodPrice = 29, FoodDescription = "The Best Pizza You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://t4.ftcdn.net/jpg/02/74/99/01/360_F_274990113_ffVRBygLkLCZAATF9lWymzE6bItMVuH1.jpg", FoodName = "Burger", FoodPrice = 24, FoodDescription = "The Best Burger You Will Ever Have!!!!" },new FoodProductModel { FoodImage = "https://makeyourmeals.com/wp-content/uploads/2018/08/featured-pizza-rolls.jpg", FoodName = "Pizza Rolls", FoodPrice = 34, FoodDescription = "The Best Pizza Rolls You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://t3.ftcdn.net/jpg/01/91/58/40/360_F_191584072_PaOd2228q4hThUJOtPnjqhGPOPDdebSu.jpg", FoodName = "Sandwich", FoodPrice = 25, FoodDescription = "The Best Sandwich You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://img.freepik.com/free-photo/fresh-ice-tea-plastic-glass_144627-27132.jpg?w=2000", FoodName = "Iced Tea", FoodPrice = 12, FoodDescription = "The Best Iced Tea You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://cdn.pixabay.com/photo/2014/01/24/04/05/fried-chicken-250863__340.jpg", FoodName = "Crispy Fried Chicken", FoodPrice = 40, FoodDescription = "The Best Crispy Fried Chicken You Will Ever Have!!!!" },
                new FoodProductModel { FoodImage = "https://blog.kainexus.com/hubfs/French%20fries%202.jpeg", FoodName = "French Fries", FoodPrice = 20, FoodDescription = "The Best French Fries You Will Ever Have!!!!" } };
            _navigationServiceMock.Setup(x => x.NavigateAsync(nameof(ConfirmationPage))).ReturnsAsync(_fixture.Create<NavigationResult>());

            //Act
            foodProductsPageViewModel.OnNavigatedTo(navigationParameters);
            foodProductsPageViewModel.ConfirmCartCommand.Execute();

            //Assert
            foodProductsPageViewModel.FoodProductList.Should().NotBeNull();
            foodProductsPageViewModel.FoodProductList.Should().BeEquivalentTo(products);
            _navigationServiceMock.Verify(x => x.NavigateAsync(nameof(ConfirmationPage)), Times.Once);
            _mockRepository.VerifyAll();
            _mockRepository.VerifyNoOtherCalls();
        }
    }
}
