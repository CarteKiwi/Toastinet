using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using ToastinetSL;

namespace ToastinetSLSample
{
    public partial class MainPage : INotifyPropertyChanged
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

        public MainPage()
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
            this.Toast2.Message = "This toast is listening for closed event";
            this.Toast2.Closed += ToastOnClosed;
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
    }
}
