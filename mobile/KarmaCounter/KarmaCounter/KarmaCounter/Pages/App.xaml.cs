using Xamarin.Forms;

namespace KarmaCounter.Pages
{
    public partial class App : Application
    {
        public App()
        {
            DI.Init();
            InitializeComponent();
            MainPage = new AuthPage();
        }

        protected override async void OnStart()
        {
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
