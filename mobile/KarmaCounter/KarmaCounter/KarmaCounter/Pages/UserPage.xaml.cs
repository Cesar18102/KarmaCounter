using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Autofac;

using Android.Views;

using CustomControls;

using KarmaCounter.Models;
using KarmaCounter.Resources;
using KarmaCounter.Controllers;
using System.Collections.Generic;
using KarmaCounter.Server.Output;
using KarmaCounter.Controls.Popups;

namespace KarmaCounter.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        private static BindableProperty ObservedUserProperty =
            BindableProperty.Create("ObservedUser", typeof(User), typeof(UserPage));

        public User ObservedUser
        {
            get => GetValue(ObservedUserProperty) as User;
            private set
            {
                SetValue(ObservedUserProperty, value);
                OnPropertyChanged("ObservedUser");
            }
        }

        private PopupControlSystem PopupControl { get; set; }

        public UserPage() : this(null) { }

        public UserPage(User user = null)
        {
            InitializeComponent();

            ExitConfirmPopup.OnFirstButtonClicked += (sender, e) => App.Current.MainPage = new AuthPage();
            ExitConfirmPopup.OnSecondButtonClicked += (sender, e) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            ErrorPopup.OnFirstButtonClicked += (sender, e) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
            PopupControl = new PopupControlSystem(OnBackButtonPressed);
            ObservedUser = user;
        }

        private Label CreateHeaderLabel(string text) =>
            new Label() { Style = App.Current.Resources["ListLabel"] as Style, Text = text };

        private Label CreateInfoLabel(string text) =>
            new Label() { Style = App.Current.Resources["ListInfoLabel"] as Style, Text = text };

        private async void CurrentUserPage_Appearing(object sender, System.EventArgs e)
        {
            try
            {
                PopupControl.OpenPopup(ActivityPopup);

                ObservedUser = ObservedUser == null ? await DI.Services.Resolve<UserController>().GetUserById(
                    DI.Services.Resolve<SessionWrapper>().CurrentUserSession.UserId) : ObservedUser;

                List<RuleAction> actions = await DI.Services.Resolve<UserController>().GetActions(ObservedUser.Id);

                Actions.Children.Clear();
                Actions.RowDefinitions.Clear();
                Actions.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                Actions.Children.Add(CreateHeaderLabel(AppResources.ActionsRuleHeaderLabelText), 0, 0);
                Actions.Children.Add(CreateHeaderLabel(AppResources.ActionsRuleScopeLabelText), 1, 0);
                Actions.Children.Add(CreateHeaderLabel(AppResources.ActionsDateTimeHeaderLabelText), 2, 0);
                Actions.Children.Add(CreateHeaderLabel(AppResources.ActionsStatusHeaderLabelText), 3, 0);

                int row = 1;
                foreach (RuleAction action in actions)
                {
                    Actions.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    Actions.Children.Add(CreateInfoLabel(action.ActionObject.Title), 0, row);
                    Actions.Children.Add(CreateInfoLabel(action.ActionObject.Text), 1, row);
                    Actions.Children.Add(CreateInfoLabel(action.TimeStamp.ToShortDateString()), 2, row);
                    Actions.Children.Add(new StackLayout()
                    {
                        BackgroundColor = (Color)App.Current.Resources["ContentBackColor"],
                        Children = {
                            new BoxView() { 
                                Style = (Style)App.Current.Resources["Indicator"],
                                BackgroundColor = action.Violated ? Color.IndianRed : Color.LightGreen
                            }
                        }
                    }, 3, row++);
                }

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
            catch (ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
        }

        private bool Burger_OnClick(MotionEvent ME, IClickable sender)
        {
            if (ME.Action != MotionEventActions.Down)
                return false;

            if (App.Current.MainPage is MasterDetailPage)
                (App.Current.MainPage as MasterDetailPage).IsPresented = true;

            return true;
        }

        protected override bool OnBackButtonPressed()
        {
            if (PopupControl.OpenedPopupsCount != 0)
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
            else
                PopupControl.OpenPopup(ExitConfirmPopup);

            return true;
        }
    }
}