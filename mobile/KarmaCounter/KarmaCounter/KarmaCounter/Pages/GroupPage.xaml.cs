using System;
using System.Threading.Tasks;
using RG = System.Text.RegularExpressions;

using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;

using Autofac;

using Android.Views;

using CustomControls;

using KarmaCounter.Models;
using KarmaCounter.Controls;
using KarmaCounter.Resources;
using KarmaCounter.Controllers;
using KarmaCounter.Server.Output;
using KarmaCounter.Controls.Popups;

namespace KarmaCounter.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupPage : ContentPage
    {
        private static readonly BindableProperty SourceGroupProperty =
            BindableProperty.Create("SourceGroup", typeof(Group), typeof(GroupPage));

        public Group SourceGroup
        {
            get => (Group)GetValue(SourceGroupProperty);
            set
            {
                if (value == null)
                    return;

                SetValue(SourceGroupProperty, value);
                OnPropertyChanged("SourceGroup");
            }
        }

        private static readonly BindableProperty RightsProperty =
            BindableProperty.Create("Rights", typeof(Ownership), typeof(GroupPage));

        public Ownership Rights
        {
            get => (Ownership)GetValue(RightsProperty);
            set
            {
                if (value == null)
                    return;

                SetValue(RightsProperty, value);
                OnPropertyChanged("Rights");
            }
        }

        private PopupControlSystem PopupControl { get; set; }

        public GroupPage(Group group)
        {
            InitializeComponent();

            ErrorPopup.OnFirstButtonClicked += (sender, e) => PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
            PopupControl = new PopupControlSystem(OnBackButtonPressed);
            SourceGroup = group;
        }

        private async void CurrentGroupPage_Appearing(object sender, EventArgs e)
        {
            PopupControl.OpenPopup(ActivityPopup);

            try 
            { 
                Rights = await DI.Services.Resolve<UserController>().GetOwnership(SourceGroup); 
                Keys.IsVisible = true; 
            }
            catch (ResponseException ex) { Keys.IsVisible = false; }

            PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

            await UpdateGroupInfo();
        }

        private Label CreateHeaderLabel(string text) =>
            new Label() { Style = Resources["ListLabel"] as Style, Text = text };

        private Label CreateInfoLabel(string text) =>
            new Label() { Style = Resources["ListInfoLabel"] as Style, Text = text };

        private async Task UpdateGroupInfo()
        {
            try
            {
                PopupControl.OpenPopup(ActivityPopup);

                SourceGroup = await DI.Services.Resolve<GroupController>().GetGroupDetailInfo(SourceGroup);

                Rules.Children.Clear();
                Rules.Children.Add(CreateHeaderLabel(AppResources.RuleNameHeader), 0, 0);
                Rules.Children.Add(CreateHeaderLabel(AppResources.RuleTextHeader), 1, 0);

                int row = 1;
                foreach (Rule rule in SourceGroup.Rules)
                {
                    Rules.Children.Add(CreateInfoLabel(rule.Title), 0, row);
                    Rules.Children.Add(CreateInfoLabel(rule.Text), 1, row++);
                }

                Members.Children.Clear();
                Members.Children.Add(CreateHeaderLabel(AppResources.MemberListLoginHeaderText), 0, 0);
                Members.Children.Add(CreateHeaderLabel(AppResources.MemberListGlobalKarmaHeaderText), 1, 0);
                Members.Children.Add(CreateHeaderLabel(AppResources.MemberListLocalKarmaHeaderText), 2, 0);

                row = 1;
                foreach (Membership membership in SourceGroup.Members)
                {
                    Members.Children.Add(CreateInfoLabel(membership.Member.Login), 0, row);
                    Members.Children.Add(CreateInfoLabel(membership.Member.Karma.ToString()), 1, row);
                    Members.Children.Add(CreateInfoLabel(membership.Karma.ToString()), 2, row++);
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

        private async void JoinGroupButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                PopupControl.OpenPopup(ActivityPopup);

                await DI.Services.Resolve<GroupController>().Join(SourceGroup);
                await UpdateGroupInfo();

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
            }
            catch(ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
        }

        private void InviteButton_Clicked(object sender, EventArgs e) =>
            PopupControl.OpenPopup(InvitePopup);

        private async void InviteConfirm_Clicked(object sender, EventArgs e)
        {
            try
            {
                PopupControl.OpenPopup(ActivityPopup);

                User user = await DI.Services.Resolve<UserController>().GetUserByLogin(LoginEntry.Text);
                await DI.Services.Resolve<GroupController>().Invite(SourceGroup, user);
                DependencyService.Get<IToast>().Show($"{user.Login} {AppResources.InvitedMessageText}", false);

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
            }
            catch (ResponseException ex)
            {
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);

                ErrorPopup.MessageText = ex.message;
                PopupControl.OpenPopup(ErrorPopup);
            }
        }

        private bool KeyLabel_Click(MotionEvent ME, IClickable sender)
        {
            if (ME.Action != MotionEventActions.Down)
                return false;

            CopyToClipboard((sender as Label).Text, AppResources.CopiedMessageText);
            return true;
        }

        private async void CopyToClipboard(string text, string messageText)
        {
            await Clipboard.SetTextAsync(text);
            DependencyService.Get<IToast>().Show(messageText, false);
        }

        private void AddRuleButton_Clicked(object sender, EventArgs e) =>
            PopupControl.OpenPopup(AddRulePopup);

        private static readonly RG.Regex FEE_FORMULA_VAR = new RG.Regex("\\{([1-9][0-9]*)\\}");

        private void AddVariableButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string formula = RuleFeeFormula.Text;

                if(string.IsNullOrEmpty(formula))
                {
                    RuleFeeFormula.Text += "{0}";
                    return;
                }

                int maxVarNum = (formula.Contains("{0}") ? 0 : -1);
                RG.MatchCollection vars = FEE_FORMULA_VAR.Matches(formula);

                foreach (RG.Match var in vars)
                    maxVarNum = Math.Max(Convert.ToInt32(var.Groups[1].Value), maxVarNum);

                RuleFeeFormula.Text += "{" + (maxVarNum + 1) + "}";
            }
            catch { }
        }

        private async void ConfirmRuleAddingButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                PopupControl.OpenPopup(ActivityPopup);

                Rule rule = new Rule(RuleNameInput.Text, RuleScopeInput.Text, RuleFeeFormula.Text);
                await DI.Services.Resolve<GroupController>().AddRule(rule, SourceGroup);
                await UpdateGroupInfo();

                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded(true);
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
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
            if (PopupControl.OpenedPopupsCount != 0)
                PopupControl.CloseTopPopupAndHideKeyboardIfNeeded();
            else
                (App.Current.MainPage as MasterDetailPage).Detail = new GroupListPage();

            return true;
        }
    }
}