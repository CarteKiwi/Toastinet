using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Toastinet;
using ToastinetSample.Resources;

namespace ToastinetSample
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnBasicToast(object sender, RoutedEventArgs e)
        {
            this.Toast.Message = "This is a basic toast";
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
            this.Toast3.Message = "This is a toast";
        }

        private void OnR2R(object sender, RoutedEventArgs e)
        {
            this.Toast3.AnimationType = AnimationType.RightToRight;
            this.Toast3.Foreground = new SolidColorBrush(Colors.Black);
            this.Toast3.Background = new SolidColorBrush(Color.FromArgb(255, 155, 89, 182));
            this.Toast3.Message = "This is a toast";
        }

        private void OnL2R(object sender, RoutedEventArgs e)
        {
            this.Toast3.AnimationType = AnimationType.LeftToRight;
            this.Toast3.Foreground = new SolidColorBrush(Color.FromArgb(255, 155, 89, 182));
            this.Toast3.Background = new SolidColorBrush(Color.FromArgb(255, 236, 240, 241));
            this.Toast3.Message = "This is a toast";
        }
    }
}