using System.ComponentModel;

using Autofac;

using Newtonsoft.Json;

using Xamarin.Forms;

using KarmaCounter.Models;

namespace KarmaCounter.Pages
{
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void MasterDetailPage_Appearing(object sender, System.EventArgs e)
        {
            DisplayAlert("Session", JsonConvert.SerializeObject(DI.Services.Resolve<SessionWrapper>().CurrentUserSession), "OK");
        }
    }
}
