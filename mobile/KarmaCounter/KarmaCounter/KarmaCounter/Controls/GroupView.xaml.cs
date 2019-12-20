using CustomControls;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using KarmaCounter.Models;

namespace KarmaCounter.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupView : ClickableContentView
    {
        private static readonly BindableProperty SourceGroupProperty =
            BindableProperty.Create("SourceGroup", typeof(Group), typeof(GroupView));

        private static readonly BindableProperty TitleProperty =
            BindableProperty.Create("Title", typeof(string), typeof(GroupView));

        private static readonly BindableProperty AccessIndicatorColorProperty =
            BindableProperty.Create("AccessIndicatorColor", typeof(Color), typeof(GroupView));

        private static readonly BindableProperty SpreadIndicatorColorProperty =
            BindableProperty.Create("SpreadIndicatorColor", typeof(Color), typeof(GroupView));

        public Group SourceGroup
        {
            get => (Group)GetValue(SourceGroupProperty);
            set
            {
                if (value == null)
                    return;

                SetValue(SourceGroupProperty, value);
                OnPropertyChanged("SourceGroup");
                OnPropertyChanged("Title");
                OnPropertyChanged("AccessIndicatorColor");
                OnPropertyChanged("SpreadIndicatorColor");
            }
        }

        private Color publicGroupIndicatorColor;
        public Color PublicGroupIndicatorColor
        {
            get => publicGroupIndicatorColor;
            set {
                publicGroupIndicatorColor = value;
                OnPropertyChanged("AccessIndicatorColor");
            }
        }

        private Color privateGroupIndicatorColor;
        public Color PrivateGroupIndicatorColor
        {
            get => privateGroupIndicatorColor;
            set
            {
                privateGroupIndicatorColor = value;
                OnPropertyChanged("AccessIndicatorColor");
            }
        }

        private Color localGroupIndicatorColor;
        public Color LocalGroupIndicatorColor
        {
            get => localGroupIndicatorColor;
            set
            {
                localGroupIndicatorColor = value;
                OnPropertyChanged("SpreadIndicatorColor");
            }
        }

        private Color globalGroupIndicatorColor;
        public Color GlobalGroupIndicatorColor
        {
            get => globalGroupIndicatorColor;
            set
            {
                globalGroupIndicatorColor = value;
                OnPropertyChanged("SpreadIndicatorColor");
            }
        }

        public string Title => string.Format("{0} by {1}", SourceGroup?.Name, SourceGroup?.Owner.Login);

        public Color AccessIndicatorColor => (SourceGroup?.IsPublic).GetValueOrDefault() ? PublicGroupIndicatorColor : PrivateGroupIndicatorColor;
        public Color SpreadIndicatorColor => (SourceGroup?.IsLocal).GetValueOrDefault() ? LocalGroupIndicatorColor : GlobalGroupIndicatorColor;

        public GroupView()
        {
            InitializeComponent();
        }
    }
}