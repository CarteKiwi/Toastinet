using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ToastinetWPF;

namespace ToastinetWPFSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void OnBasicToast(object sender, RoutedEventArgs e)
        {
            this.Message = "This is a basic bound toast";
        }

        private void OnBasicToast2(object sender, RoutedEventArgs e)
        {
            this.Toast2.Message = "This is a basic toast with message and without logo. You can set a large text (you have to set height indeed)";
        }

        private void OnL2L(object sender, RoutedEventArgs e)
        {
            this.Toast3.AnimationType = AnimationType.LeftToLeft;
            this.Toast3.Foreground = new SolidColorBrush(Colors.Black);
            this.Toast3.Background = new SolidColorBrush(Color.FromArgb(255, 230, 126, 34));
            this.Toast3.Message = "This is a toast from left to left";
        }

        private void OnR2R(object sender, RoutedEventArgs e)
        {
            this.Toast3.AnimationType = AnimationType.RightToRight;
            this.Toast3.Foreground = new SolidColorBrush(Colors.Black);
            this.Toast3.Background = new SolidColorBrush(Color.FromArgb(255, 155, 89, 182));
            this.Toast3.Message = "This is a toast from right to right";
        }

        private void OnL2R(object sender, RoutedEventArgs e)
        {
            this.Toast3.AnimationType = AnimationType.LeftToRight;
            this.Toast3.Foreground = new SolidColorBrush(Color.FromArgb(255, 155, 89, 182));
            this.Toast3.Background = new SolidColorBrush(Color.FromArgb(255, 236, 240, 241));
            this.Toast3.Message = "This is a toast from left to right";
        }

        private void OnEventToast(object sender, RoutedEventArgs e)
        {
            ToastEventBtn.Content = "== toast animating ==";
            ToastEventBtn.IsEnabled = false;
            this.Toast2.Message = "This toast is listening for closing/closed events";
            this.Toast2.Closing += ToastOnClosing;
            this.Toast2.Closed += ToastOnClosed;
        }

        private void ToastOnClosing(object sender, VisualStateChangedEventArgs visualStateChangedEventArgs)
        {
            this.Toast2.Closing -= ToastOnClosing;
            ToastEventBtn.Content = "toast closing...";
        }

        private void ToastOnClosed(object sender, VisualStateChangedEventArgs visualStateChangedEventArgs)
        {
            this.Toast2.Closed -= ToastOnClosed;
            ToastEventBtn.Content = "toast event";
            ToastEventBtn.IsEnabled = true;
        }

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnWindow(object sender, RoutedEventArgs e)
        {
            var stack = new StackPanel { Margin = new Thickness(50) };
            stack.Children.Add(new TextBlock
            {
                Text = "Window is dynamically built. When content is rendered, a new toast is created.",
                MaxWidth = 250,
                TextWrapping = TextWrapping.Wrap
            });

            var g = new Grid();
            g.Children.Add(stack);

            var w = new Window
            {
                Name = "ToastinetChild",
                Content = g,
                SizeToContent = SizeToContent.WidthAndHeight,
                Owner = this
            };

            w.ContentRendered += OnRuntime;
            w.Show();
        }

        private Toastinet globalToast;
        private void OnRuntime(object sender, EventArgs e)
        {
            if (globalToast == null)
            {
                // Initialize a new toast
                globalToast = new Toastinet
                {
                    Owner = sender as FrameworkElement,
                    Name = "Toast5",
                    Duration = 1,
                    Title = "Toastinet Runtime",
                    AnimationType = AnimationType.Vertical,
                    VerticalAlignment = VerticalAlignment.Top,
                    Message = ""
                };
                //globalToast.Closed += (a, b) =>
                //{
                //    (globalToast.Owner as Window).Close();
                //    (globalToast.Owner as Window).ContentRendered -= OnRuntime;
                //};
            }

            globalToast.Owner = sender as FrameworkElement;
            globalToast.Show("test");


        }
    }
}
