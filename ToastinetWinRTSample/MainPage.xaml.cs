using System.ComponentModel;
using ToastinetWinRT;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace ToastinetWinRTSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : INotifyPropertyChanged
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
            this.DataContext = this;
        }

        private void OnBasicToast(object sender, RoutedEventArgs e)
        {
            FirstToastMsg = "This is a basic bound toast";
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
            this.Toast3.Message = "This toast goes from Left To Left";
        }

        private void OnR2R(object sender, RoutedEventArgs e)
        {
            this.Toast3.AnimationType = AnimationType.RightToRight;
            this.Toast3.Foreground = new SolidColorBrush(Colors.Black);
            this.Toast3.Background = new SolidColorBrush(Color.FromArgb(255, 155, 89, 182));
            this.Toast3.Message = "This toast goes from Right To Right";
        }

        private void OnL2R(object sender, RoutedEventArgs e)
        {
            this.Toast3.AnimationType = AnimationType.LeftToRight;
            this.Toast3.Foreground = new SolidColorBrush(Color.FromArgb(255, 155, 89, 182));
            this.Toast3.Background = new SolidColorBrush(Color.FromArgb(255, 236, 240, 241));
            this.Toast3.Message = "This toast goes from Left To Right";
        }

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
