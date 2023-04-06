using System.Linq;
using FoodDeliveryApplication.Models;
using FoodDeliveryApplication.Repos.Interface;
using FoodDeliveryApplication.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace FoodDeliveryApplication.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IGenericRepository<UserDataModel> _genericRepository;
        private readonly IPageDialogService _pageDialogService;
        private string _username;
        private string _password;
        public LoginPageViewModel(INavigationService navigationService, IGenericRepository<UserDataModel> genericRepository, IPageDialogService pageDialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _genericRepository = genericRepository;
            _pageDialogService = pageDialogService;
            LoginCommand = new DelegateCommand(LoginCommandHandler);
            SignupCommand = new DelegateCommand(SignupCommandHandler);
        }

        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand SignupCommand { get; set; }
        public string Username { get { return _username; } set { SetProperty(ref _username, value); } }
        public string Password { get { return _password; } set { SetProperty(ref _password, value); } }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            Username = string.Empty;
            Password = string.Empty;
        }
        private async void LoginCommandHandler()
        {
            if (!string.IsNullOrEmpty(Username) || !string.IsNullOrEmpty(Password))
            {
                var result = await _genericRepository.Get();
                var user = result.Where(x => x.Username == Username && x.Password == Password).FirstOrDefault();
                if (user != null)
                {
                    await _navigationService.NavigateAsync(nameof(FoodProductsPage));
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync("Alert", "Wrong Credentials", "Retry");
                }
            }
            else
            {
                await _pageDialogService.DisplayAlertAsync("Alert", "Username and Password is Empty", "Retry");
            }

        }
        private async void SignupCommandHandler()
        {
            await _navigationService.NavigateAsync(nameof(SignupPage));
        }
    }
}
