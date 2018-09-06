using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Filesharp
{
    /// <summary>
    /// Interaction logic for Operation_is_running.xaml
    /// </summary>
    /// 


    public partial class Operation_is_running : Window
    {
        // idk what this does exactly but it's important
        public Operation_is_running()
        {
            InitializeComponent();
        }

        public void Open(string title, string textblock1Text)
        {
            this.Title = title;
            this.textblock1.Text = textblock1Text;
            this.Show();
        }

        public void Minimize()
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Hide();
            });
        }

        public void UpdateText(string newText)
        {
            this.Dispatcher.Invoke(() =>
            {
                textblock1.Text = newText;
            });
        }
        
        public void UpdateProgress(int done, int toBeDone)
        {
            this.Dispatcher.Invoke(() =>
            {
                textblock_Progress.Text = $"{ Math.Round(Convert.ToDouble(done) / Convert.ToDouble(toBeDone) * 100, 2)} percent complete";
            });
        }
        /*
        dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
        dispatcherTimer.Interval = new TimeSpan(0,0,1);
        dispatcherTimer.Start();
        */
    }
}
