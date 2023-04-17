using System;
using System.Windows;

namespace OpenSilverLeakExample
{
    public partial class LeakWindow : IId
    {
        private string _log = "";
        public string CreatedStackTrace;

        private static int _counter;
        public LeakWindow()
        {
            InitializeComponent();

            CreatedStackTrace = Environment.StackTrace;

            Id = "LeakWindow " + _counter++;

            //Global event!!!
            Application.Current.Exit += Application_Exit;
        }

        public string Id { get; }

        ~LeakWindow()
        {
            Console.WriteLine("LeakWindow has been collected");
        }

        private void Application_Exit(object sender, System.EventArgs e)
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

