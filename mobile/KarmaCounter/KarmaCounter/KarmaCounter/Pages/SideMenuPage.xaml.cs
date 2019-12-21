using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KarmaCounter.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SideMenuPage : ContentPage
    {
        public SideMenuPage()
        {
            InitializeComponent();
        }

        private void WatchGroupList_Clicked(object sender, EventArgs e) =>
            MainPageChangeDetail<GroupListPage>();

        private void WatchProfile_Clicked(object sender, EventArgs e) =>
            MainPageChangeDetail<UserPage>();

        private void WatchMyGroupsStatistics_Clicked(object sender, EventArgs e) =>
            MainPageChangeDetail<MyGroupsStats>();

        private void MainPageChangeDetail<T>() where T : Page, new()
        {
            MasterDetailPage mainPage = (App.Current.MainPage as MasterDetailPage);

            mainPage.Detail = new T();
            mainPage.IsPresented = false;
        }
    }
}