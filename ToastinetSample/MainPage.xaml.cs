using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Toastinet;

namespace ToastinetSample
{
    public partial class MainPage : INotifyPropertyChanged
    {
        private string _firstToastMsg;
        public string FirstToastMsg
        {
            get { return _firstToastMsg; }
            set
            {
                _firstToastMsg = value;
                OnPropertyChanged("FirstToastMsg");
            }
        }

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void OnBasicToast(object sender, RoutedEventArgs e)
        {
            FirstToastMsg = "Data binding & queue : " + DateTime.Now.Second;
        }

        private void OnBasicToast2(object sender, RoutedEventArgs e)
        {
            Toast2.Message = "This is a basic toast with message and without logo. You can set a large text (you have to set height indeed)";
        }

        private void OnL2L(object sender, RoutedEventArgs e)
        {
            Toast3.AnimationType = AnimationType.LeftToLeft;
            Toast3.Foreground = new SolidColorBrush(Colors.Black);
            Toast3.Background = new SolidColorBrush(Color.FromArgb(255, 230, 126, 34));
            Toast3.Message = "This toast goes from Left To Left";
        }

        private void OnR2R(object sender, RoutedEventArgs e)
        {
            Toast3.AnimationType = AnimationType.RightToRight;
            Toast3.Foreground = new SolidColorBrush(Colors.Black);
            Toast3.Background = new SolidColorBrush(Color.FromArgb(255, 155, 89, 182));
            Toast3.Message = "This toast goes from Right To Right";
        }

        private void OnL2R(object sender, RoutedEventArgs e)
        {
            Toast3.AnimationType = AnimationType.LeftToRight;
            Toast3.Foreground = new SolidColorBrush(Color.FromArgb(255, 155, 89, 182));
            Toast3.Background = new SolidColorBrush(Color.FromArgb(255, 236, 240, 241));
            Toast3.Message = "This toast goes from Left To Right";
        }
        private void OnNotStretch(object sender, RoutedEventArgs e)
        {
            Toast4.Message = "This toast is not full width";
        }

        private void OnEventToast(object sender, RoutedEventArgs e)
        {
            ToastEventBtn.Content = "== toast animating ==";
            ToastEventBtn.IsEnabled = false;
            Toast2.Message = "This toast is listening for closed event";
            Toast2.Closed += ToastOnClosed;
        }

        private void ToastOnClosed(object sender, VisualStateChangedEventArgs visualStateChangedEventArgs)
        {
            Toast2.Closed -= ToastOnClosed;
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