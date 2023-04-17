using System;
using System.Windows;

namespace OpenSilverLeakExample
{
    public partial class NoLeakWindow : IId
    {
        private string _log = "";

        private static int _counter;

        public NoLeakWindow()
        {
            InitializeComponent();

            Id = "NoLeakWindow " + _counter++;

            this.Unloaded += NoLeakWindow_Loaded;
        }

        public string Id { get; }

        ~NoLeakWindow()
        {
            Console.WriteLine("NoLeakWindow has been collected");
        }

        private void NoLeakWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _log += "Do some huge work!";
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

