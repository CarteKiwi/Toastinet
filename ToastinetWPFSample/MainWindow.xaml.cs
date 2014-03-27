﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
