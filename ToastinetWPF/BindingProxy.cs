using System.ComponentModel;
using System.Windows;

namespace ToastinetWPF
{
    public class BindingProxy : Freezable, INotifyPropertyChanged
    {
        #region Overrides of Freezable

        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        #endregion

        public object Data
        {
            get { return GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            var safe = this.PropertyChanged;
            if (safe != null)
            {
                safe(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
