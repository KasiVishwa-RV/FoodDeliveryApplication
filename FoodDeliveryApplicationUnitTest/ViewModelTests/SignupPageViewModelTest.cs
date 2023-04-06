using AutoFixture;
using Moq;
using Prism.Navigation;
using Prism.Services;
using FluentAssertions;
using FoodDeliveryApplication.Models;
using FoodDeliveryApplication.Repos.Interface;
using FoodDeliveryApplication.ViewModels;

namespace FoodDeliveryApplicationUnitTest.ViewModelTests
{
    public class SignupPageViewModelTest
    {
        private readonly Fixture _fixture;
        private readonly MockRepository _mockRepository;
        private readonly Mock<IPageDialogService> _pageDialogServiceMock;
        private readonly Mock<IGenericRepository<UserDataModel>> _genericRepositoryMock;
        private readonly Mock<INavigationService> _navigationServiceMock;

        public SignupPageViewModelTest()
        {
            _fixture = new Fixture();
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _genericRepositoryMock = _mockRepository.Create<IGenericRepository<UserDataModel>>();
            _navigationServiceMock = _mockRepository.Create<INavigationService>();
            _pageDialogServiceMock = _mockRepository.Create<IPageDialogService>();
        }

        private SignupPageViewModel CreateViewModel()
        {
            return new SignupPageViewModel(_navigationServiceMock.Object, _genericRepositoryMock.Object, _pageDialogServiceMock.Object);
        }

        [Fact]
        public void ShouldNotThrowAnyException_WhenSignupPageViewModelConstructor_IsCalled()
        {
            Action action = () => CreateViewModel();
            action.Should().NotThrow<Exception>();
        }

        [Fact]
        public void ShouldAddTheUserDataAndGoBackToLoginPage_WhenExecute_SignupCommand()
        {
            //Arrange
            var signupPageViewModel = CreateViewModel();
            _genericRepositoryMock.Setup(x => x.Insert(It.IsAny<UserDataModel>())).ReturnsAsync(1);
            _navigationServiceMock.Setup(x => x.GoBackAsync()).ReturnsAsync(_fixture.Create<NavigationResult>());

            //Act
            signupPageViewModel.Username = _fixture.Create<string>();
            signupPageViewModel.Password = _fixture.Create<string>();
            signupPageViewModel.SignupCommand.Execute();

            //Assert
            _genericRepositoryMock.Verify(x => x.Insert(It.IsAny<UserDataModel>()), Times.Once);
            _navigationServiceMock.Verify(x => x.GoBackAsync(), Times.Once());
            _mockRepository.VerifyAll();
            _mockRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldDisplayEnterValidDetailsAlert_WhenExecute_SignupCommand()
        {
            //Arrange
            var signupPageViewModel = CreateViewModel();
            _pageDialogServiceMock.Setup(x => x.DisplayAlertAsync("Alert", "Enter Valid Details", "Ok")).Returns(Task.FromResult(true));

            //Act
            signupPageViewModel.SignupCommand.Execute();

            //Assert
            _pageDialogServiceMock.Verify(x => x.DisplayAlertAsync("Alert", "Enter Valid Details", "Ok"), Times.Once);
            _mockRepository.VerifyAll();
            _mockRepository.VerifyNoOtherCalls();
        }
    }
}
