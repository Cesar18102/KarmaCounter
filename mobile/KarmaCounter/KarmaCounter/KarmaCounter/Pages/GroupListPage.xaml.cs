using System.Resources;

using Android.Views;

using Autofac;

using CustomControls;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using KarmaCounter.Models;
using KarmaCounter.Controls;
using KarmaCounter.Resources;
using KarmaCounter.Controllers;
using KarmaCounter.Server.Output;
using KarmaCounter.Controls.Popups;

[assembly : NeutralResourcesLanguage("en-US")]
namespace KarmaCounter.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupListPage : ContentPage
    {
        private PopupControlSystem PopupControl { get; set; }

        public GroupListPage()
        {
            InitializeComponent();

            PopupControl = new PopupControlSystem(OnBackButtonPressed);
            ErrorPopup.OnFirstButtonClicked += (sender, e) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            ExitConfirmPopup.OnFirstButtonClicked += (sender, e) => App.Current.MainPage = new AuthPage();
            ExitConfirmPopup.OnSecondButtonClicked += (sender, e) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();

            PayPopup.OnSecondButtonClicked += (sender, e) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
        }

        private void ContentPage_Appearing(object sender, System.EventArgs e) => UpdateGroupList();

        private void EnlistGroup(Group group)
        {
            GroupView view = new GroupView() {
                SourceGroup = group,
                PublicGroupIndicatorColor = Color.LightGreen,
                PrivateGroupIndicatorColor = Color.IndianRed,
                GlobalGroupIndicatorColor = Color.LightGreen,
                LocalGroupIndicatorColor = Color.IndianRed
            };

            view.OnClick += (ME, ctx) =>
            {
                if (ME.Action != MotionEventActions.Down)
                    return false;

                (App.Current.MainPage as MasterDetailPage).Detail = new GroupPage(group);
                return true;
            };

            GroupListLayout.Children.Add(view);
        }

        private async void UpdateGroupList()
        {
            try
            {
                PopupControl.OpenPopup(ActivityPopup);

                GroupListLayout.Children.Clear();
                foreach (Group group in await DI.Services.Resolve<GroupController>().GetAll())
                    EnlistGroup(group);

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
            catch(ResponseException ex)
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

        private bool AddGroupButton_OnClick(MotionEvent ME, IClickable sender)
        {
            if (ME.Action != MotionEventActions.Down)
                return false;

            PopupControl.OpenPopup(AddGroupPopup);
            return true;
        }

        private async void ConfirmGroupCreationButton_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                PopupControl.OpenPopup(ActivityPopup);

                Ownership rights = await DI.Services.Resolve<GroupController>().Create(GroupNameInput.Text, GroupDescriptionInput.Text, 
                                                                                       GroupPublicChecker.Checked, GroupLocalChecker.Checked);
                UpdateGroupList();

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
            }
            catch (ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
            catch(PaymentRequiredException pex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                LiqpayPayment payment = await DI.Services.Resolve<GroupController>().CreateGroupCreatingPayment(
                    GroupNameInput.Text, GroupDescriptionInput.Text,
                    GroupPublicChecker.Checked, GroupLocalChecker.Checked, pex.Amount);

                PayPopup.OnFirstButtonClicked += (ctx, arg) => App.Current.MainPage = new WebPayPage(payment);
                PayPopup.MessageText = $"{AppResources.PaymentExceptionText} {pex.Amount}$";
                PopupControl.OpenPopup(PayPopup);
            }
        }

        private bool CheckerLabel_OnClick(MotionEvent ME, IClickable sender) =>
            ((sender as Element).BindingContext as IClickable).Click(ME);

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