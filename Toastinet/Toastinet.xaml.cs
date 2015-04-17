using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

namespace Toastinet
{
    /// <summary>
    /// Toastinet is a tool developed by Guillaume DEMICHELI shared on CodePlex via Nuget for free
    /// </summary>
    [ContentProperty("ToastContent")]
    public partial class Toastinet : INotifyPropertyChanged
    {
        #region Private variables
        private TimeSpan _interval = new TimeSpan(0, 0, 3);
        private bool _isLoaded;
        Queue<string> _queue = new Queue<string>();
        #endregion

        #region Event closed
        public delegate void ClosedEventHandler(object sender, VisualStateChangedEventArgs e);
        public event ClosedEventHandler Closed;
        public event ClosedEventHandler Closing;

        private void OnCurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            if (e.NewState.Name.EndsWith("Closed"))
                if (Closed != null)
                    Closed(this, e);
        }

        private void OnCurrentStateChanging(object sender, VisualStateChangedEventArgs e)
        {
            if (e.NewState.Name.EndsWith("Closed"))
                if (Closing != null)
                    Closing(this, e);
        }
        #endregion

        #region ToastContent
        public object ToastContent
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public new static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("ToastContent", typeof(object), typeof(Toastinet), new PropertyMetadata(null, OnContentChanged));

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var toast = (Toastinet)d;
            if (e.NewValue != null)
                toast.DefaultContent.Visibility = Visibility.Collapsed;
            else
                toast.DefaultContent.Visibility = Visibility.Visible;
        }
        #endregion

        #region TextHAlignment (Horizontal alignment) (Default: Stretch)
        public HorizontalAlignment TextHAlignment
        {
            get { return (HorizontalAlignment)GetValue(TextHAlignmentProperty); }
            set { SetValue(TextHAlignmentProperty, value); }
        }

        public static readonly DependencyProperty TextHAlignmentProperty =
            DependencyProperty.Register("TextHAlignment", typeof(HorizontalAlignment), typeof(Toastinet), new PropertyMetadata(HorizontalAlignment.Stretch));
        #endregion

        #region Message
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(Toastinet), new PropertyMetadata(string.Empty, OnTextChanged));

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                var toast = (Toastinet)d;

                if (e.NewValue == null || string.IsNullOrEmpty(e.NewValue.ToString()) ||
                    (e.OldValue != null && toast.Queued && toast._queue.Contains(e.OldValue.ToString())))
                {
                    if (e.NewValue == null || string.IsNullOrEmpty(e.NewValue.ToString()))
                        if (toast.Queued && toast._queue.Any())
                        {
                            toast.Message = toast._queue.Dequeue();
                        }
                    return;
                }

                if (e.OldValue != null && toast.Queued &&
                    !toast.GetCurrentState(toast.GetValidAnimation() + "Group").Name.Contains("Closed"))
                {
                    toast._queue.Enqueue(e.NewValue.ToString());
                    toast.Message = e.OldValue.ToString();
                    return;
                }

                VisualStateManager.GoToState(toast, toast.GetValidAnimation() + "Opened", true);

                var timer = new DispatcherTimer { Interval = toast._interval };
                timer.Tick += (s, t) =>
                {
                    VisualStateManager.GoToState(toast, toast.GetValidAnimation() + "Closed", true);
                    timer.Stop();
                };
                timer.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception occured during OnTextChanged operation. Details: " + ex.Message);
            }
        }

        public VisualState GetCurrentState(string stateGroupName)
        {
            var list = (IList<VisualStateGroup>)VisualStateManager.GetVisualStateGroups(VisualTreeHelper.GetChild(this, 0) as FrameworkElement);

            var stateGroup1 = list.FirstOrDefault(v => v.Name == stateGroupName);

            if (stateGroup1 != null) return stateGroup1.CurrentState;

            return null;
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

        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(Toastinet), new PropertyMetadata(TextWrapping.NoWrap));
        #endregion

        #region ReversedHeight
        public int ReversedHeight
        {
            get
            {
                return -(int)LayoutRoot.ActualHeight;
            }
        }
        #endregion

        #region Title
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Toastinet), new PropertyMetadata(string.Empty, OnTitleChanged));

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var toast = (Toastinet)d;
            if (e.NewValue != null && !string.IsNullOrEmpty((string)e.NewValue))
                toast.TitleVisibility = Visibility.Visible;
            else
                toast.TitleVisibility = Visibility.Collapsed;
        }

        internal Visibility TitleVisibility
        {
            get { return (Visibility)GetValue(TitleVisibilityProperty); }
            set { SetValue(TitleVisibilityProperty, value); }
        }

        public static readonly DependencyProperty TitleVisibilityProperty =
            DependencyProperty.Register("TitleVisibility", typeof(Visibility), typeof(Toastinet), new PropertyMetadata(System.Windows.Visibility.Collapsed));
        #endregion

        #region Image
        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(Toastinet), new PropertyMetadata(null, OnImageChanged));

        private static void OnImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var toast = (Toastinet)d;
            if (e.NewValue != null)
                toast.LogoVisibility = Visibility.Visible;
            else
                toast.LogoVisibility = Visibility.Collapsed;
        }

        internal Visibility LogoVisibility
        {
            get { return (Visibility)GetValue(LogoVisibilityProperty); }
            set { SetValue(LogoVisibilityProperty, value); }
        }

        public static readonly DependencyProperty LogoVisibilityProperty =
            DependencyProperty.Register("LogoVisibility", typeof(Visibility), typeof(Toastinet), new PropertyMetadata(System.Windows.Visibility.Collapsed));
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
                toast.NotifyChanged();
                var projection = toast.LayoutRoot.Projection as PlaneProjection;
                if (projection != null && animType != AnimationType.Rotation && animType != AnimationType.Vertical)
                {
                    projection.GlobalOffsetX = toast.WidthToOpened;
                }
            }
        }

        #endregion

        #region Clipped
        public bool Clipped
        {
            get { return (bool)GetValue(ClippedProperty); }
            set { SetValue(ClippedProperty, value); }
        }

        public static readonly DependencyProperty ClippedProperty =
            DependencyProperty.Register("Clipped", typeof(bool), typeof(Toastinet), new PropertyMetadata(false));
        #endregion

        #region Queued
        public bool Queued
        {
            get { return (bool)GetValue(QueuedProperty); }
            set { SetValue(QueuedProperty, value); }
        }

        public static readonly DependencyProperty QueuedProperty =
            DependencyProperty.Register("Queued", typeof(bool), typeof(Toastinet), new PropertyMetadata(false));
        #endregion

        public int WidthToClosed
        {
            get
            {
                var width = (int)LayoutRoot.ActualWidth;
                if (AnimationType == AnimationType.LeftToLeft ||
                    AnimationType == AnimationType.RightToLeft)
                    width = -(int)LayoutRoot.ActualWidth;

                return width;
            }
        }

        public int WidthToOpened
        {
            get
            {
                var width = (int)LayoutRoot.ActualWidth;
                if (AnimationType == AnimationType.LeftToRight ||
                    AnimationType == AnimationType.LeftToLeft)
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

            // Default Padding
            Padding = new Thickness(10);

            Loaded += (s, e) =>
            {
                _isLoaded = true;
                NotifyChanged();

                VisualStateManager.GoToState(this, GetValidAnimation() + "Closed", false);
            };
        }

        private AnimationType GetValidAnimation()
        {
            var anim = AnimationType;
            if (anim == AnimationType.RightToLeft || anim == AnimationType.LeftToLeft || anim == AnimationType.RightToRight)
                anim = AnimationType.LeftToRight;

            return anim;
        }

        /// <summary>
        /// Use to clip the toast content to its bounds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFirstContainerChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                ToastMsg.Width = LayoutRoot.ActualWidth - 10 - e.NewSize.Width - Padding.Left - Padding.Right;
            }
            catch
            {
                ToastMsg.Width = Width - 20;
            }

            if (Clipped)
                Clip = new RectangleGeometry
                {
                    Rect = new Rect(0, 0, ActualWidth, ActualHeight + 10)
                };
            else
                Clip = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SbCompleted(object sender, EventArgs e)
        {
            // Force the property to be changed even if the user don't change the message value
            // It's done in this callback to avoid text disappear (set to empty) before the closing animation is completed
            // Not sure it's a good way to do it (it was done with the CoerceValue in WPF)
            Message = string.Empty;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            NotifyChanged();
        }

        private void NotifyChanged()
        {
            if (PropertyChanged == null) return;

            PropertyChanged(this, new PropertyChangedEventArgs("WidthToClosed"));
            PropertyChanged(this, new PropertyChangedEventArgs("WidthToOpened"));
            PropertyChanged(this, new PropertyChangedEventArgs("ReversedHeight"));
        }
    }
}
