﻿using AG = Android.Graphics;

using Xamarin.Forms;

using CustomControls;

namespace KarmaCounter.Controls.Popups
{
    public class PhotoPopup : Popup
    {
        public const string POPUP_BACKGROUND_COLOR = "InactiveTransperentColor";

        public const string INNER_FRAME_CLASS = "PopupInnerFrame";
        public const string OUTER_FRAME_CLASS = "PopupOuterFrame";
        public const string POPUP_BACKGROUND_CLASS = "PopupBackgroud";

        private string imageSource = "";
        public string ImageSource { get { return imageSource; } set { imageSource = value; UpdatePhotoSource(); } }

        private AG.Bitmap firstSource { get; set; }
        public AG.Bitmap FirstSource { get => firstSource; set { firstSource = value; UpdatePhotoSource(); } }

        protected Img Image { get; set; }

        public PhotoPopup() : base()
        {
            Img image = new Img();
            image.HorizontalOptions = LayoutOptions.FillAndExpand;
            image.VerticalOptions = LayoutOptions.CenterAndExpand;
            image.BorderOnClick = false;
            image.Border = false;
            image.Margin = 0;
            Image = image;

            StackLayout layout = new StackLayout();
            layout.Children.Add(image);
            ContentWrapper = layout;

            ClickableFrame innerFrame = new ClickableFrame();
            innerFrame.Style = (Style)Application.Current.Resources[INNER_FRAME_CLASS];
            innerFrame.HorizontalOptions = LayoutOptions.FillAndExpand;
            innerFrame.VerticalOptions = LayoutOptions.FillAndExpand;
            innerFrame.Padding = 0;
            innerFrame.Content = layout;
            InnerFrame = innerFrame;

            ClickableFrame outerFrame = new ClickableFrame();
            outerFrame.Style = (Style)Application.Current.Resources[OUTER_FRAME_CLASS];
            outerFrame.HorizontalOptions = LayoutOptions.FillAndExpand;
            outerFrame.VerticalOptions = LayoutOptions.FillAndExpand;
            outerFrame.Padding = 0;
            outerFrame.Content = innerFrame;
            OuterFrame = outerFrame;

            UpdatePhotoSource();

            base.Style = (Style)Application.Current.Resources[POPUP_BACKGROUND_CLASS];
            BackgroundColor = (Color)Application.Current.Resources[POPUP_BACKGROUND_COLOR];
            IsVisible = false;
            Closable = true;
            Content = outerFrame;
            OnPopupBodyClicked = (e, sender) => true;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (Image != null)
            {
                Image.WidthRequest = width;
                Image.HeightRequest = width;
            }
        }

        public void UpdatePhotoSource()
        {
            if (FirstSource != null)
                Image.FirstSource = FirstSource;
            else if (ImageSource != null)
                Image.Source = ImageSource;
        }
    }
}
