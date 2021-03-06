using System;
using Genesis.Logging;
using ReactiveUI;
using Sextant;
using Sextant.Abstraction;
using SextantSample.ViewModels;
using SextantSample.Views;
using Splat;
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

            RxApp.DefaultExceptionHandler = new SextantDefaultExceptionHandler();

            SextantHelper.RegisterView<HomeView, HomeViewModel>();
            SextantHelper.RegisterView<FirstModalView, FirstModalViewModel>();
            SextantHelper.RegisterView<SecondModalView, SecondModalViewModel>();
            SextantHelper.RegisterView<RedView, RedViewModel>();
            SextantHelper.RegisterNavigation<BlueNavigationView, SecondModalViewModel>();

            MainPage = SextantHelper.Initialise<HomeViewModel>();
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
