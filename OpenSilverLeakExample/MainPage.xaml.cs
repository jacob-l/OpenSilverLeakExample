using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace OpenSilverLeakExample
{
    public partial class MainPage : Page
    {
        private List<WeakReference> _weakReferences = new List<WeakReference>();

        public MainPage()
        {
            this.InitializeComponent();

            OpenSilver.Interop.ExecuteJavaScriptVoid("console.clear()");
            // Enter construction logic here...
        }
        
        public void ShowLeakingWindow(object sender, EventArgs args)
        {
            var window = new LeakWindow();
            _weakReferences.Add(new WeakReference(window));
            window.Show();
        }

        public void ShowNotLeakingWindow(object sender, EventArgs args)
        {
            var window = new NoLeakWindow();
            _weakReferences.Add(new WeakReference(window));
            window.Show();
        }

        public void CallGarbageCollector(object sender, EventArgs args)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            TB.Text = string.Join(Environment.NewLine,
                _weakReferences.Where(r => r.IsAlive).Select(r => r.Target).OfType<IId>()
                    .Select(el => el.Id + " Is Alive"));
        }
        
        public void RunMeasurements(object sender, EventArgs args)
        {
            OpenSilver.Interop.ExecuteJavaScriptVoid(@"
                async function measureMemory() {
                  const memorySample = await performance.measureUserAgentSpecificMemory();
                  $0();
                  console.log(memorySample);
                }

                if (crossOriginIsolated) {
                  measureMemory();
                }
            ", (Action)(() =>
            {
                var memoryKb = GC.GetTotalMemory(false) / 1024;
                Console.WriteLine($"Memory - {memoryKb} Kb.");
            }));
        }

    }
}
