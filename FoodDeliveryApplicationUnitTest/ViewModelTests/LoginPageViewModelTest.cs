using AutoFixture;
using Moq;
using Prism.Navigation;
using Prism.Services;
using FluentAssertions;
using FoodDeliveryApplication.Repos.Interface;
using FoodDeliveryApplication.Models;
using FoodDeliveryApplication.ViewModels;
using FoodDeliveryApplicationUnitTest.Helper;
using FoodDeliveryApplication.Views;

namespace FoodDeliveryApplicationUnitTest.ViewModelTests
{
    public class LoginPageViewModelTest
    {
        private readonly Fixture _fixture;
        private readonly MockRepository _mockRepository;
        private readonly Mock<IPageDialogService> _pageDialogServiceMock;
        private readonly Mock<IGenericRepository<UserDataModel>> _genericRepositoryMock;
        private readonly Mock<INavigationService> _navigationServiceMock;

        public LoginPageViewModelTest()
        {
            _fixture = new Fixture();
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _genericRepositoryMock = _mockRepository.Create<IGenericRepository<UserDataModel>>();
            _navigationServiceMock = _mockRepository.Create<INavigationService>();
            _pageDialogServiceMock = _mockRepository.Create<IPageDialogService>();
        }

        private LoginPageViewModel CreateViewModel()
        {
            return new LoginPageViewModel(_navigationServiceMock.Object, _genericRepositoryMock.Object, _pageDialogServiceMock.Object);
        }

        [Fact]
        public void ShouldNotThrowAnyException_WhenLoginPageViewModelConstructor_IsCalled()
        {
            Action action = () => CreateViewModel();
            action.Should().NotThrow<Exception>();
        }

        [Fact]
        public void ShouldEmptyTheUserNameAndPasswordField_WhenCalledOnNavigatedTo()
        {
            //Arrange
            var loginPageViewModel = CreateViewModel();
            var navigationParameters = NavigationHelper.GetNavigationParameters();

            //Act
            loginPageViewModel.Username = _fixture.Create<string>();
            loginPageViewModel.Password = _fixture.Create<string>();
            loginPageViewModel.OnNavigatedTo(navigationParameters);

            //Assert
            loginPageViewModel.Username.Should().BeNullOrEmpty();
            loginPageViewModel.Password.Should().BeNullOrEmpty();
            _mockRepository.VerifyAll();
            _mockRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldNavigateToSignupPage_WhenExecuteSignupCommand()
        {
            //Arrange
            var loginPageViewModel = CreateViewModel();
            var navigationParameters = NavigationHelper.GetNavigationParameters();
            _navigationServiceMock.Setup(x => x.NavigateAsync(nameof(SignupPage))).ReturnsAsync(_fixture.Create<NavigationResult>());

            //Act
            loginPageViewModel.OnNavigatedTo(navigationParameters);
            loginPageViewModel.SignupCommand.Execute();

            //Assert
            _navigationServiceMock.Verify(x => x.NavigateAsync(nameof(SignupPage)), Times.Once);
            _mockRepository.VerifyAll();
            _mockRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldNavigateToProductsPage_WhenExecuteLoginCommand()
        {
            //Arrange
            var loginPageViewModel = CreateViewModel();
            var navigationParameters = NavigationHelper.GetNavigationParameters();
            var userData = _fixture.Create<List<UserDataModel>>();
            _genericRepositoryMock.Setup(x => x.Get()).ReturnsAsync(userData);
            _navigationServiceMock.Setup(x => x.NavigateAsync(nameof(FoodProductsPage))).ReturnsAsync(_fixture.Create<NavigationResult>());

            //Act
            loginPageViewModel.OnNavigatedTo(navigationParameters);
            loginPageViewModel.Username = userData.First().Username;
            loginPageViewModel.Password = userData.First().Password;
            loginPageViewModel.LoginCommand.Execute();

            //Assert
            _genericRepositoryMock.Verify(x => x.Get(), Times.Once);
            _navigationServiceMock.Verify(x => x.NavigateAsync(nameof(FoodProductsPage)), Times.Once);
            _mockRepository.VerifyAll();
            _mockRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldDisplayWrongCredentialsAlert_WhenExecuteLoginCommand()
        {
            //Arrange
            var loginPageViewModel = CreateViewModel();
            var navigationParameters = NavigationHelper.GetNavigationParameters();
            var userData = _fixture.Create<List<UserDataModel>>();
            _genericRepositoryMock.Setup(x => x.Get()).ReturnsAsync(userData);
            _pageDialogServiceMock.Setup(x => x.DisplayAlertAsync("Alert", "Wrong Credentials", "Retry")).Returns(Task.FromResult(true));

            //Act
            loginPageViewModel.OnNavigatedTo(navigationParameters);
            loginPageViewModel.Username = _fixture.Create<string>();
            loginPageViewModel.Password = _fixture.Create<string>();
            loginPageViewModel.LoginCommand.Execute();

            //Assert
            _genericRepositoryMock.Verify(x => x.Get(), Times.Once);
            _pageDialogServiceMock.Verify(x => x.DisplayAlertAsync("Alert", "Wrong Credentials", "Retry"), Times.Once);
            _mockRepository.VerifyAll();
            _mockRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldDisplayEnterUserDetailsAlert_WhenExecuteLoginCommand()
        {
            //Arrange
            var loginPageViewModel = CreateViewModel();
            var navigationParameters = NavigationHelper.GetNavigationParameters();
            _pageDialogServiceMock.Setup(x => x.DisplayAlertAsync("Alert", "Username and Password is Empty", "Retry")).Returns(Task.FromResult(true));

            //Act
            loginPageViewModel.OnNavigatedTo(navigationParameters);
            loginPageViewModel.Username = null;
            loginPageViewModel.Password = null;
            loginPageViewModel.LoginCommand.Execute();

            //Assert
            _pageDialogServiceMock.Verify(x => x.DisplayAlertAsync("Alert", "Username and Password is Empty", "Retry"), Times.Once);
            _mockRepository.VerifyAll();
            _mockRepository.VerifyNoOtherCalls();
        }
    }
}
