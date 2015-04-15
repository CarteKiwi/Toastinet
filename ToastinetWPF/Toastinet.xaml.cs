using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

namespace ToastinetWPF
{
    /// <summary>
    /// Toastinet is an UserControl developed by Guillaume DEMICHELI shared on CodePlex via Nuget for Windows Phone
    /// </summary>
    [ContentProperty("ToastContent")]
    public partial class Toastinet : INotifyPropertyChanged
    {
        #region Private variables
        private TimeSpan _interval = new TimeSpan(0, 0, 3);
        private bool _isLoaded;
        Queue<string> _queue = new Queue<string>();
        private bool IsFullyLoaded { get; set; }
        #endregion

        #region Owner

        private static FrameworkElement _previousOwner;
        public FrameworkElement Owner
        {
            get { return (FrameworkElement)GetValue(OwnerProperty); }
            set { SetValue(OwnerProperty, value); }
        }

        public static readonly DependencyProperty OwnerProperty =
            DependencyProperty.Register("Owner", typeof(FrameworkElement), typeof(Toastinet), new PropertyMetadata(null, OnOwnerChanged));

        private static void OnOwnerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var toast = (Toastinet)d;

            if (toast.Owner != null)
            {
                _previousOwner = e.OldValue as FrameworkElement;

                ChangeOwner(toast, e.OldValue as FrameworkElement);
            }
        }

        private static void ChangeOwner(Toastinet toast, FrameworkElement parent)
        {
            // Remove toast from parent
            if (parent != null)
            {
                IEnumerable<UIElement> parentContainer = VisualTreeHelperUtil.GetControlsDecendant<Grid>(parent);
                var grid = parentContainer.FirstOrDefault() as System.Windows.Controls.Grid;
                if (grid != null)
                    grid.Children.Remove(toast);
                else
                {
                    var panel = parentContainer.FirstOrDefault() as StackPanel;
                    if (panel != null)
                        panel.Children.Remove(toast);
                }
            }

            // Set ZIndex of the dynamic toast
            Canvas.SetZIndex(toast, 99999);

            // Try to find First Grid
            IEnumerable<UIElement> container = VisualTreeHelperUtil.GetControlsDecendant<Grid>(toast.Owner);
            if (container.FirstOrDefault() != null)
            {
                ((Grid)container.First()).Children.Insert(0, toast);
            }
            else
            {
                //no grid, is there a stackpanel
                container = VisualTreeHelperUtil.GetControlsDecendant<StackPanel>(toast.Owner);
                if (container.FirstOrDefault() != null)
                {
                    ((Grid)container.First()).Children.Insert(0, toast);
                }
                else
                {
                    throw new Exception("Unable to find window container of type Grid or StackPanel");
                }
            }

            VisualStateManager.GoToState(toast, toast.GetValidAnimation() + "Opened", true);
            VisualStateManager.GoToState(toast, toast.GetValidAnimation() + "Closed", true);
        }
        #endregion

        #region Loaded event
        public event RoutedEventHandler LLoaded = delegate { };
        #endregion

        #region Event closed
        public delegate void ClosedEventHandler(object sender, VisualStateChangedEventArgs e);
        public event ClosedEventHandler Closed;
        public event ClosedEventHandler Closing;

        private void OnCurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            if (IsFullyLoaded && e.NewState.Name.EndsWith("Closed"))
                if (Closed != null)
                    Closed(this, e);

            if (_isLoaded && e.OldState == null && !IsFullyLoaded && PropertyChanged != null)
            {
                IsFullyLoaded = true;
                if (LLoaded != null)
                    LLoaded(this, new RoutedEventArgs());
            }
        }

        private void OnCurrentStateChanging(object sender, VisualStateChangedEventArgs e)
        {
            if (IsFullyLoaded && e.OldState != null && e.NewState.Name.EndsWith("Closed"))
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
            DependencyProperty.Register("Message", typeof(string), typeof(Toastinet), new PropertyMetadata(String.Empty, OnTextChanged, OnTextSet));

        // This is the CoerceValue method, always called even if the value does not change
        private static object OnTextSet(DependencyObject d, object baseValue)
        {
            try
            {
                var toast = (Toastinet)d;

                if (baseValue == null || string.IsNullOrEmpty(baseValue.ToString()))
                {
                    return baseValue;
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
                MessageBox.Show(ex.Message);
            }

            return baseValue;
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //try
            //{
            //    var toast = (Toastinet)d;

            //    if (e.NewValue == null || String.IsNullOrEmpty(e.NewValue.ToString()) ||
            //        (e.OldValue != null && toast.Queued && toast._queue.Contains(e.OldValue.ToString())))
            //    {
            //        if (e.NewValue == null || String.IsNullOrEmpty(e.NewValue.ToString()))
            //            if (toast.Queued && toast._queue.Any())
            //            {
            //                toast.Message = toast._queue.Dequeue();
            //            }
            //        return;
            //    }

            //    if (e.OldValue != null && toast.Queued && !toast.GetCurrentState(toast.GetValidAnimation() + "Group").Name.Contains("Closed"))
            //    {
            //        toast._queue.Enqueue(e.NewValue.ToString());
            //        toast.Message = e.OldValue.ToString();
            //        return;
            //    }

            //    VisualStateManager.GoToState(toast, toast.GetValidAnimation() + "Opened", true);

            //    var timer = new DispatcherTimer { Interval = toast._interval };
            //    timer.Tick += (s, t) =>
            //    {
            //        VisualStateManager.GoToState(toast, toast.GetValidAnimation() + "Closed", true);
            //        timer.Stop();
            //    };
            //    timer.Start();
            //}
            //catch { }
        }

        public VisualState GetCurrentState(string stateGroupName)
        {
            var list = (IList<VisualStateGroup>)VisualStateManager.GetVisualStateGroups(LayoutRoot);

            if (list == null) return null;

            var stateGroup1 = list.FirstOrDefault(v => v.Name == stateGroupName);

            return stateGroup1 != null ? stateGroup1.CurrentState : null;
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

        #region Height & ReversedHeight
        public int ReversedHeight
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
            }
        }

        public new static readonly DependencyProperty HeightProperty =
            DependencyProperty.Register("Height", typeof(int), typeof(Toastinet), new PropertyMetadata(30));

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

            if (toast.PropertyChanged != null)
            {
                toast. NotifyChanged();
                var tg = toast.MainGrid.RenderTransform as TransformGroup;
                if (tg != null)
                {
                    var translation = tg.Children[1] as TranslateTransform;
                    if (translation != null)
                    {
                        translation.X = toast.WidthToOpened;
                    }
                }
            }
        }

        #endregion

        #region Font
        public new FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public new static readonly DependencyProperty FontFamilyProperty =
            DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(Toastinet), new PropertyMetadata(new FontFamily("Segoe UI")));
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
        /// <summary>
        /// TODO : Not working right now. Almost done.
        /// </summary>
        private bool Queued
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

            Loaded += (s, e) =>
            {
                if (PropertyChanged != null)
                {
                    _isLoaded = true;
                    NotifyChanged();
                    VisualStateManager.GoToState(this, GetValidAnimation() + "Closed", false);
                }

                VisualStateManager.GoToState(this, GetValidAnimation() + "Closed", false);
            };
        }

        public void Show(string message = null)
        {
            if (!IsFullyLoaded)
                LLoaded += (s, e) => Show(message);
            else
                Message = message ?? Message;
        }

        private AnimationType GetValidAnimation()
        {
            var anim = AnimationType;
            if (anim == AnimationType.RightToLeft || anim == AnimationType.LeftToLeft || anim == AnimationType.RightToRight)
                anim = AnimationType.LeftToRight;

            return anim;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            NotifyChanged();
            AdjustBodyContainerWidth();
            ClipIt();
        }

        /// <summary>
        /// Adjust the width of the default text message (allow the text to wrap)
        /// </summary>
        private void AdjustBodyContainerWidth()
        {
            try
            {
                ToastMsg.Width = LayoutRoot.ActualWidth - 20 - HeaderContainer.ActualWidth;
            }
            catch (Exception ex)
            {
                ToastMsg.Width = LayoutRoot.ActualWidth;
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Create a clip within the Toast bounds
        /// </summary>
        private void ClipIt()
        {
            if (Clipped)
                LayoutRoot.Clip = new RectangleGeometry
                {
                    Rect = new Rect(0, 0, LayoutRoot.ActualWidth, LayoutRoot.ActualHeight + 10)
                };
            else
                LayoutRoot.Clip = null;
        }

        /// <summary>
        /// Raise property changed to notify view that values have changed
        /// </summary>
        private void NotifyChanged()
        {
            if (PropertyChanged == null) return;

            PropertyChanged(this, new PropertyChangedEventArgs("WidthToClosed"));
            PropertyChanged(this, new PropertyChangedEventArgs("WidthToOpened"));
            PropertyChanged(this, new PropertyChangedEventArgs("ReversedHeight"));
            PropertyChanged(this, new PropertyChangedEventArgs("AnimationType"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
