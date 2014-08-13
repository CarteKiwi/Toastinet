using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace ToastinetSL
{
    /// <summary>
    /// Toastinet is an UserControl developed by Guillaume DEMICHELI shared on CodePlex via Nuget for Windows Phone
    /// </summary>
    public partial class Toastinet : INotifyPropertyChanged
    {
        #region Private variables
        private TimeSpan _interval = new TimeSpan(0, 0, 3);
        private bool _isLoaded;
        #endregion

        #region Event closed
        public delegate void ClosedEventHandler(object sender, VisualStateChangedEventArgs e);
        public event ClosedEventHandler Closed;
        public event ClosedEventHandler Closing;

        private void OnCurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            if (e.NewState.Name.EndsWith("Closed"))
                if (Closed != null)
                    Closed(sender, e);
        }

        private void OnCurrentStateChanging(object sender, VisualStateChangedEventArgs e)
        {
            if (e.NewState.Name.EndsWith("Closed"))
                if (Closing != null)
                    Closing(sender, e);
        }
        #endregion

        #region ShowLogo (Default: true)
        /// <summary>
        /// Show the image bound to the control (default is True)
        /// </summary>
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

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(Toastinet), new PropertyMetadata(String.Empty, OnTextChanged));

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (e.NewValue == null || String.IsNullOrEmpty(e.NewValue.ToString()))
                    return;

                var toast = (Toastinet)d;

                VisualStateManager.GoToState(toast, toast.GetValidAnimation() + "Opened", true);

                var timer = new DispatcherTimer { Interval = toast._interval };
                timer.Tick += (s, t) =>
                {
                    VisualStateManager.GoToState(toast, toast.GetValidAnimation() + "Closed", true);
                    timer.Stop();
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

        public int InvertedHeight
        {
            get
            {
                return -(int)GetValue(HeightProperty);
            }
        }

        public new int Height
        {
            get { return (int)GetValue(HeightProperty); }
            set
            {
                SetValue(HeightProperty, value);
                //PropertyChanged(this, new PropertyChangedEventArgs("InvertedHeight"));
            }
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

        #region AnimationType
        /// <summary>
        /// Define the animation type: Vertical or Rotation
        /// </summary>
        public AnimationType AnimationType
        {
            get { return (AnimationType)GetValue(AnimationTypeProperty); }
            set { SetValue(AnimationTypeProperty, value); }
        }

        public static readonly DependencyProperty AnimationTypeProperty =
            DependencyProperty.Register("AnimationType", typeof(AnimationType), typeof(Toastinet), new PropertyMetadata(AnimationType.Rotation, OnAnimationTypeChanged));

        private static void OnAnimationTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var toast = (Toastinet)d;
            AnimationType animType;

            if (!Enum.TryParse(e.NewValue.ToString(), out animType))
                toast.AnimationType = AnimationType.Rotation;
            else
            {
                toast.AnimationType = animType;
            }

            if (toast._isLoaded)
            {
                toast.PropertyChanged(d, new PropertyChangedEventArgs("WidthToClosed"));
                toast.PropertyChanged(d, new PropertyChangedEventArgs("WidthToOpened"));
                var projection = toast.MainGrid.Projection as PlaneProjection;
                if (projection != null)
                {
                    projection.GlobalOffsetX = toast.WidthToOpened;
                }
            }
        }

        #endregion

        public int WidthToClosed
        {
            get
            {
                var width = (int)LayoutRoot.ActualWidth;
                if (this.AnimationType == AnimationType.LeftToLeft ||
                    this.AnimationType == AnimationType.RightToLeft)
                    width = -(int)LayoutRoot.ActualWidth;

                return width;
            }
        }

        public int WidthToOpened
        {
            get
            {
                var width = (int)LayoutRoot.ActualWidth;
                if (this.AnimationType == AnimationType.LeftToRight ||
                    this.AnimationType == AnimationType.LeftToLeft)
                    width = -(int)LayoutRoot.ActualWidth;

                return width;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Toastinet()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                _isLoaded = true;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("WidthToClosed"));
                    PropertyChanged(this, new PropertyChangedEventArgs("WidthToOpened"));
                }

                VisualStateManager.GoToState(this, GetValidAnimation() + "Closed", false);
            };
        }

        private AnimationType GetValidAnimation()
        {
            var anim = this.AnimationType;
            if (anim == AnimationType.RightToLeft || anim == AnimationType.LeftToLeft || anim == AnimationType.RightToRight)
                anim = AnimationType.LeftToRight;

            return anim;
        }

        private void OnFirstContainerChanged(object sender, SizeChangedEventArgs e)
        {
            ToastMsg.Width = LayoutRoot.ActualWidth - 10 - e.NewSize.Width;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SbCompleted(object sender, EventArgs e)
        {
            // Force the property to be changed even if the user don't change the message value
            // It's done in this callback to avoid text disappear (set to empty) before the closing animation is completed
            // Not sure it's a good way to do it (it was done with the CoerceValue in WPF)
            this.Message = String.Empty;
        }
    }
}
