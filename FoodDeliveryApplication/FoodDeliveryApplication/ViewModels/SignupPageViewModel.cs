using FoodDeliveryApplication.Models;
using FoodDeliveryApplication.Repos.Interface;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace FoodDeliveryApplication.ViewModels
{
    public class SignupPageViewModel : ViewModelBase
    {
        private readonly IGenericRepository<UserDataModel> _genericRepository;
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private string _username;
        private string _password;
        public SignupPageViewModel(INavigationService navigationService, IGenericRepository<UserDataModel> genericRepository, IPageDialogService pageDialogService) : base(navigationService)
        {
            _navigationService = navigationService;
            _genericRepository = genericRepository;
            _pageDialogService = pageDialogService;
            SignupCommand = new DelegateCommand(SignupCommandHandler);
        }
        public string Username { get { return _username; } set { SetProperty(ref _username, value); } }
        public string Password { get { return _password; } set { SetProperty(ref _password, value); } }
        public DelegateCommand SignupCommand { get; set; }
        private async void SignupCommandHandler()
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                await _genericRepository.Insert(new UserDataModel { Username = Username, Password = Password });
                await _navigationService.GoBackAsync();
            }
            else
            {
                await _pageDialogService.DisplayAlertAsync("Alert", "Enter Valid Details", "Ok");
            }
        }
    }
}
