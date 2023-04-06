using AutoFixture;
using FluentAssertions;
using FoodDeliveryApplication.Models;
using FoodDeliveryApplication.Repos.Interface;
using FoodDeliveryApplication.Services.Interfaces;
using FoodDeliveryApplication.ViewModels;
using FoodDeliveryApplication.Views;
using FoodDeliveryApplicationUnitTest.Helper;
using Moq;
using Prism.Navigation;
using Prism.Services;

namespace FoodDeliveryApplicationUnitTest.ViewModelTests
{
    public class ConfirmationPageViewModelTest
    {
        private readonly Fixture _fixture;
        private readonly MockRepository _mockRepository;
        private readonly Mock<IPageDialogService> _pageDialogServiceMock;
        private readonly Mock<ILocationService> _locationServiceMock;
        private readonly Mock<IGenericRepository<FoodCartModel>> _genericRepositoryMock;
        private readonly Mock<INavigationService> _navigationServiceMock;

        public ConfirmationPageViewModelTest()
        {
            _fixture = new Fixture();
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _genericRepositoryMock = _mockRepository.Create<IGenericRepository<FoodCartModel>>();
            _navigationServiceMock = _mockRepository.Create<INavigationService>();
            _locationServiceMock = _mockRepository.Create<ILocationService>();
            _pageDialogServiceMock = _mockRepository.Create<IPageDialogService>();
        }

        private ConfirmationPageViewModel CreateViewModel()
        {
            return new ConfirmationPageViewModel(_navigationServiceMock.Object, _pageDialogServiceMock.Object, _genericRepositoryMock.Object,_locationServiceMock.Object);
        }

        [Fact]
        public void ShouldNotThrowAnyException_WhenConfirmationPageViewModelConstructor_IsCalled()
        {
            Action action = () => CreateViewModel();
            action.Should().NotThrow<Exception>();
        }

        [Fact]
        public void ShouldStoreFoodCartData_WhenCalledOnNavigatedTo()
        {
            //Arrange
            var confirmationPageViewModel = CreateViewModel();
            var navigationParameters = NavigationHelper.GetNavigationParameters();
            var cartData = _fixture.Create<List<FoodCartModel>>();
            _genericRepositoryMock.Setup(x => x.Get()).ReturnsAsync(cartData);

            //Act
            confirmationPageViewModel.OnNavigatedTo(navigationParameters);

            //Assert
            _genericRepositoryMock.Verify(x => x.Get(), Times.Once);
            confirmationPageViewModel.CartList.Should().NotBeNull();
            confirmationPageViewModel.CartList.Should().BeEquivalentTo(cartData);
            _mockRepository.VerifyAll();
            _mockRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRemoveTheSelectedItem_WhenExecute_RemoveCommand()
        {
            //Arrange
            var confirmationPageViewModel = CreateViewModel();
            var navigationParameters = NavigationHelper.GetNavigationParameters();
            var cartData = _fixture.Create<List<FoodCartModel>>();
            _genericRepositoryMock.Setup(x => x.Get()).ReturnsAsync(cartData);

            var removeditem = cartData[new Random().Next(0, 2)];
            _genericRepositoryMock.Setup(x => x.DeleteCart(removeditem)).ReturnsAsync(1);

            //Act
            confirmationPageViewModel.OnNavigatedTo(navigationParameters);
            confirmationPageViewModel.RemoveCommand.Execute(removeditem);

            //Assert
            _genericRepositoryMock.Verify(x => x.Get(), Times.Exactly(2));
            confirmationPageViewModel.CartList.Should().NotBeNull();
            confirmationPageViewModel.CartList.Should().BeEquivalentTo(cartData);
            _mockRepository.VerifyAll();
            _mockRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldDisplayAlertForPaymentConfirmations_AndIfApproved_ShouldNavigateTo_FoodProductsPage_WhenExecute_PayCommand()
        {
            //Arrange
            var confirmationPageViewModel = CreateViewModel();
            var navigationParameters = NavigationHelper.GetNavigationParameters();
            var cartData = _fixture.Create<List<FoodCartModel>>();
            _genericRepositoryMock.Setup(x => x.Get()).ReturnsAsync(cartData);

            _pageDialogServiceMock.Setup(x => x.DisplayAlertAsync("ConfirmPayment", "Are the Location and Payment Confirmed?", "Yes", "No")).Returns(Task.FromResult(true));
            _pageDialogServiceMock.Setup(x => x.DisplayAlertAsync("Successful", "PaymentDone", "Return To Products Page")).Returns(Task.FromResult(true));
            _navigationServiceMock.Setup(x => x.NavigateAsync(nameof(FoodProductsPage))).ReturnsAsync(_fixture.Create<NavigationResult>());

            //Act
            confirmationPageViewModel.OnNavigatedTo(navigationParameters);
            confirmationPageViewModel.PayCommand.Execute();

            //Assert
            _genericRepositoryMock.Verify(x => x.Get(), Times.Once);
            confirmationPageViewModel.CartList.Should().NotBeNull();
            confirmationPageViewModel.CartList.Should().BeEquivalentTo(cartData);
            _pageDialogServiceMock.Verify(x => x.DisplayAlertAsync("ConfirmPayment", "Are the Location and Payment Confirmed?", "Yes", "No"), Times.Once);
            _pageDialogServiceMock.Verify(x => x.DisplayAlertAsync("Successful", "PaymentDone", "Return To Products Page"), Times.Once);
            _navigationServiceMock.Verify(x => x.NavigateAsync(nameof(FoodProductsPage)), Times.Once);
            _mockRepository.VerifyAll();
            _mockRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldReturnUserLocation_WhenExecute_FindUserLocationCommand()
        {
            //Arrange
            var confirmationPageViewModel = CreateViewModel();
            var navigationParameters = NavigationHelper.GetNavigationParameters();
            var cartData = _fixture.Create<List<FoodCartModel>>();
            _genericRepositoryMock.Setup(x => x.Get()).ReturnsAsync(cartData);

            var address = _fixture.Create<string>();
            _locationServiceMock.Setup(x => x.GetLocationAsync()).ReturnsAsync(address);
            //Act
            confirmationPageViewModel.OnNavigatedTo(navigationParameters);
            confirmationPageViewModel.FindUserLocationCommand.Execute();

            //Assert
            _genericRepositoryMock.Verify(x => x.Get(), Times.Once);
            _locationServiceMock.Verify(x => x.GetLocationAsync(),Times.Once);
            confirmationPageViewModel.CartList.Should().NotBeNull();
            confirmationPageViewModel.CartList.Should().BeEquivalentTo(cartData);
            confirmationPageViewModel.UserLocation.Should().BeEquivalentTo(address);
            _mockRepository.VerifyAll();
            _mockRepository.VerifyNoOtherCalls();
        }
    }
}
