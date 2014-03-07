using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Toastinet
{
    /// <summary>
    /// Toastinet is an UserControl developed by Guillaume DEMICHELI shared on CodePlex via Nuget for Windows Phone
    /// </summary>
    public partial class Toastinet
    {
        #region Private variables
        private TimeSpan _interval = new TimeSpan(0, 0, 3);
        #endregion

        #region ShowLogo (Default: true)
        public bool ShowLogo
        {
            get { return (bool)GetValue(ShowLogoProperty); }
            set { SetValue(ShowLogoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowLogo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowLogoProperty =
            DependencyProperty.Register("ShowLogo", typeof(bool), typeof(Toastinet), new PropertyMetadata(true));
        #endregion

        #region Background (Default: ARGB = 255, 52, 73, 94)
        public new SolidColorBrush Background
        {
            get { return (SolidColorBrush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public new static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(SolidColorBrush), typeof(Toastinet), new PropertyMetadata(new SolidColorBrush { Color = new Color { A = 255, R = 52, G = 73, B = 94 }, Opacity = .9 }));
        #endregion

        #region TextHAlignment (Horizontal alignment) (Default: Stretch)
        public HorizontalAlignment TextHAlignment
        {
            get { return (HorizontalAlignment)GetValue(TextHAlignmentProperty); }
            set { SetValue(TextHAlignmentProperty, value); }
        }

        public static readonly DependencyProperty TextHAlignmentProperty =
            DependencyProperty.Register("TextHAlignment", typeof(HorizontalAlignment), typeof(Toastinet), new PropertyMetadata(System.Windows.HorizontalAlignment.Stretch));
        #endregion

        #region Message
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(Toastinet), new PropertyMetadata(String.Empty, OnTextChanged));

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (e.NewValue == null || String.IsNullOrEmpty(e.NewValue.ToString()))
                    return;

                var toast = (Toastinet)d;

                if (toast.ToastMsg != null)
                    toast.ToastMsg.Text = e.NewValue.ToString();

                VisualStateManager.GoToState(toast, "Opened", true);

                var timer = new DispatcherTimer { Interval = toast._interval };
                timer.Tick += (s, t) =>
                {
                    VisualStateManager.GoToState(toast, "Closed", true);
                    timer.Stop();
                    toast.Message = String.Empty;
                };
                timer.Start();
            }
            catch { }
        }
        #endregion

        #region Duration (Default: 3s)
        /// <summary>
        /// Displaying duration of the toast (in sec)
        /// </summary>
        public int Duration
        {
            get { return (int)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(int), typeof(Toastinet), new PropertyMetadata(3, OnDurationChanged));

        private static void OnDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Toastinet;
            int interval;
            Int32.TryParse(e.NewValue.ToString(), out interval);
            if (control != null) control._interval = new TimeSpan(0, 0, interval);
        }
        #endregion

        #region TextWrapping
        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextWrapping.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(Toastinet), new PropertyMetadata(TextWrapping.NoWrap));
        #endregion

        #region Height
        public int InvertedHeight { get { return -Height; } }
        public new int Height
        {
            get { return (int)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        public new static readonly DependencyProperty HeightProperty =
            DependencyProperty.Register("Height", typeof(int), typeof(Toastinet), new PropertyMetadata(50));

        #endregion

        #region Title
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Toastinet), new PropertyMetadata("AppName"));
        #endregion

        #region Image
        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(string), typeof(Toastinet), new PropertyMetadata("/Toastinet;component/Assets/tile.png"));
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public Toastinet()
        {
            InitializeComponent();

            VisualStateManager.GoToState(this, "Closed", true);
        }

        private void OnFirstContainerChanged(object sender, SizeChangedEventArgs e)
        {
            ToastMsg.Width = 480 - 10 - e.NewSize.Width;
        }
    }
}
