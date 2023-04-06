using Prism.Navigation;

namespace FoodDeliveryApplicationUnitTest.Helper
{
    public static class NavigationHelper
    {
        private const string NavigationModeKey = "__NavigationMode";
        public static NavigationParameters GetNavigationParameters(NavigationMode navigationMode = NavigationMode.New)
        {
            var navigationParameters = new NavigationParameters();
            INavigationParametersInternal navigationParametersInternal = navigationParameters;
            navigationParametersInternal.Add(NavigationModeKey, navigationMode);
            return navigationParameters;
        }
    }
}
