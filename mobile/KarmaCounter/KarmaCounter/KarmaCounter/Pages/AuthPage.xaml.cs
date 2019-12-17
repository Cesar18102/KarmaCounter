using System;

using Autofac;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using KarmaCounter.Validation;
using KarmaCounter.Controllers;
using KarmaCounter.Server.Output;
using KarmaCounter.Controls.Popups;
using KarmaCounter.Pages.Additional.Validators.Templates;

namespace KarmaCounter.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthPage : ContentPage
    {
        private PopupControlSystem PopupControl { get; set; }
        private SignUpValidator SignUpValidator { get; set; }
        private LogInValidator LogInValidator { get; set; }

        private ValidationHandler<Entry> InvalidHandler = E => E.BackgroundColor = (Color)Application.Current.Resources["InvalidColor"];
        private ValidationHandler<Entry> ValidHandler = E => E.BackgroundColor = (Color)Application.Current.Resources["ValidColor"];

        public AuthPage()
        {
            InitializeComponent();

            PopupControl = new PopupControlSystem(OnBackButtonPressed);
            ErrorPopup.OnFirstButtonClicked += (sender, e) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            SignUpValidator = new SignUpValidator(SignUpLoginEntry, SignUpPasswordEntry, SignUpPasswordConfirmEntry, SignUpEmailEntry, ValidHandler, InvalidHandler);
            LogInValidator = new LogInValidator(LogInLoginEntry, LogInPasswordEntry, ValidHandler, InvalidHandler);
        }

        private void SignUpButton_Clicked(object sender, EventArgs e) => PopupControl.OpenPopup(SignUpPopup);
        private void LogInButton_Clicked(object sender, EventArgs e) => PopupControl.OpenPopup(LogInPopup);

        private void SignUpPopupConfirm_Clicked(object sender, EventArgs e)
        {
            if (!SignUpValidator.ValidateAll())
                return;

            SignUpAsync(SignUpLoginEntry.Text, SignUpPasswordEntry.Text, SignUpEmailEntry.Text);
        }

        private void LogInPopupConfirm_Clicked(object sender, EventArgs e)
        {
            if (!LogInValidator.ValidateAll())
                return;

            LogInAsync(LogInLoginEntry.Text, LogInPasswordEntry.Text);
        }

        private async void SignUpAsync(string login, string password, string email)
        {
            PopupControl.OpenPopup(ActivityPopup);

            try
            {
                await DI.Services.Resolve<AuthController>().SignUp(login, password, email);
                App.Current.MainPage = new MainPage();

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
            catch (ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
        }

        private async void LogInAsync(string login, string password)
        {
            PopupControl.OpenPopup(ActivityPopup);

            try
            {
                await DI.Services.Resolve<AuthController>().LogIn(login, password);
                App.Current.MainPage = new MainPage();

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
            catch (ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (PopupControl.OpenedPopupsCount == 0 || PopupControl.IsKeyboardVisible())
                return false;

            PopupControl.CloseTopPopup();
            return true;
        }
    }
}