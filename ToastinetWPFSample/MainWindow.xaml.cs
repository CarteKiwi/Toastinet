﻿using System;
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
            DataContext = this;
        }

        private void OnBasicToast(object sender, RoutedEventArgs e)
        {
            Message = "Data binding & queue : " + DateTime.Now.Second;
        }

        private void OnBasicToast2(object sender, RoutedEventArgs e)
        {
            Toast2.Message = "This is a basic toast with message and without logo. You can set a large text (height is auto !)";
        }

        private void OnL2L(object sender, RoutedEventArgs e)
        {
            Toast3.AnimationType = AnimationType.LeftToLeft;
            Toast3.Foreground = new SolidColorBrush(Colors.Black);
            Toast3.Background = new SolidColorBrush(Color.FromArgb(255, 230, 126, 34));
            Toast3.Message = "This is a toast from left to left";
        }

        private void OnR2R(object sender, RoutedEventArgs e)
        {
            Toast3.AnimationType = AnimationType.RightToRight;
            Toast3.Foreground = new SolidColorBrush(Colors.Black);
            Toast3.Background = new SolidColorBrush(Color.FromArgb(255, 155, 89, 182));
            Toast3.Message = "This is a toast from right to right";
        }

        private void OnL2R(object sender, RoutedEventArgs e)
        {
            Toast3.AnimationType = AnimationType.LeftToRight;
            Toast3.Foreground = new SolidColorBrush(Color.FromArgb(255, 155, 89, 182));
            Toast3.Background = new SolidColorBrush(Color.FromArgb(255, 236, 240, 241));
            Toast3.Message = "This is a toast from left to right";
        }

        private void OnEventToast(object sender, RoutedEventArgs e)
        {
            ToastEventBtn.Content = "== toast animating ==";
            ToastEventBtn.IsEnabled = false;
            Toast2.Message = "This toast is listening for closing/closed events";
            Toast2.Closing += ToastOnClosing;
            Toast2.Closed += ToastOnClosed;
        }

        private void ToastOnClosing(object sender, VisualStateChangedEventArgs visualStateChangedEventArgs)
        {
            Toast2.Closing -= ToastOnClosing;
            ToastEventBtn.Content = "toast closing...";
        }

        private void ToastOnClosed(object sender, VisualStateChangedEventArgs visualStateChangedEventArgs)
        {
            Toast2.Closed -= ToastOnClosed;
            ToastEventBtn.Content = "toast event";
            ToastEventBtn.IsEnabled = true;
        }

        private void OnNotStretch(object sender, RoutedEventArgs e)
        {
            Toast4.Message = "This toast is not full width";
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
