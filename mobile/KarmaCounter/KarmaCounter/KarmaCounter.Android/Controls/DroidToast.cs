using Xamarin.Forms;

using Android.Widget;

using KarmaCounter.Controls;
using KarmaCounter.Droid.Controls;

[assembly: Dependency(typeof(DroidToast))]
namespace KarmaCounter.Droid.Controls
{
    public class DroidToast : IToast
    {
        public void Show(string message, bool longDuration) =>
            Toast.MakeText(Android.App.Application.Context, message, longDuration ? ToastLength.Long : ToastLength.Short).Show();
    }
}