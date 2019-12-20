using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using KarmaCounter.Models;

namespace KarmaCounter.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebPayPage : ContentPage
    {
        private LiqpayPayment Payment { get; set; }

        public WebPayPage(LiqpayPayment payment)
        {
            Payment = payment;

            InitializeComponent();

            Browser.Navigated += Browser_Navigated;
        }

        private void ContentPage_Appearing(object sender, System.EventArgs e) => LoadPayButton();

        private void LoadPayButton()
        {
            HtmlWebViewSource html = new HtmlWebViewSource()
            {
                Html =
                "<html><style>" +
                            "#form { margin-top : 40vh; }" +
                            "#loader {" +
                                "border: 1.5vh solid #67A93D;" +
                                "border-left : 1.5vh solid #FFFFFF;" +
                                "border-radius: 50%;" +
                                "width: 10vh;" +
                                "height: 10vh;" +
                                "animation: spin 2s linear infinite;" +
                            "}" +
                            "@keyframes spin {" +
                                "0% { transform: rotate(0deg); }" +
                                "100% { transform: rotate(360deg); }" +
                            "}" +
                        "</style>" +
                    "<body>" +
                        "<form id='form' method='POST' action='https://www.liqpay.ua/api/3/checkout' accept-charset='utf-8'>" +
                            $"<input type='hidden' name='data' value='{Payment.Base64}'/>" +
                            $"<input type='hidden' name='signature' value='{Payment.Signature}'/>" +
                            "<center>" +
                                "<div id='loader'></div>" +
                                "<img id='pay_button' onclick='document.getElementById(\"loader\").hidden = false; document.getElementById(\"pay_button\").hidden = true; document.getElementById(\"form\").submit()' src='https://static.liqpay.ua/buttons/p1ru.radius.png'/>" +
                            "</center>" +
                        "</form>" +
                        "<script>" +
                            "window.onload = function() {" +
                                "document.getElementById('loader').hidden = true;" +
                                "document.getElementById('pay_button').hidden = false;" +
                            "}" +
                        "</script>" +
                    "</body>" +
                "</html>"
            };
            Browser.Source = html;
        }

        private void Browser_Navigated(object sender, WebNavigatedEventArgs e)
        {
            if (e.Url.Contains("gotrips"))//It's default redirect page. change if liqpay account changed.
                App.Current.MainPage = new MainPage();
        }

        protected override bool OnBackButtonPressed()
        {
            if (Browser.Source is UrlWebViewSource)
                LoadPayButton();
            else
                App.Current.MainPage = new MainPage();

            return true;
        }
    }
}