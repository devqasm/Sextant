using Sextant;
using SextantSample.ViewModels;
using SextantSample.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SextantSample
{
	public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

			SextantCore.Instance.RegisterPage<HomeView, HomeViewModel, HomeNavigationView, HomeNavigationViewModel>();
			SextantCore.Instance.RegisterPage<FirstModalView, FirstModalViewModel, FirstModalNavigationView, FirstModalNavigationViewModel>();
			SextantCore.Instance.RegisterPage<SecondModalView, SecondModalViewModel, SecondModalNavigationView, SecondModalNavigationViewModel>();
			SextantCore.Instance.RegisterPage<RedView, RedViewModel>();

			MainPage = SextantCore.Instance.GetNavigationPage<HomeNavigationViewModel>() as NavigationPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
