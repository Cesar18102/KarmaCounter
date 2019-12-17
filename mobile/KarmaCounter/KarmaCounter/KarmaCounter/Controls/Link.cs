using System;

using CustomControls;

using Android.Views;

using Xamarin.Forms;

namespace KarmaCounter.Controls
{
    public class Link : ClickableLabel
    {
        public string Url { get; set; }

        public Link() { OnClick += Link_OnClick; }

        private bool Link_OnClick(MotionEvent ME, IClickable sender)
        {
            if (ME.Action != MotionEventActions.Down)
                return false;

            if (Url == null || !Uri.IsWellFormedUriString(Url, UriKind.Absolute))
                return false;

            Device.OpenUri(new Uri(Url));
            return true;
        }
    }
}
