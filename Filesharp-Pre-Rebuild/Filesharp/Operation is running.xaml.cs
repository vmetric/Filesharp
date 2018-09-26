using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Data;


namespace Filesharp
{
    public partial class Operation_is_running : Window
    {
        // idk what this does exactly but it's important
        public Operation_is_running()
        {
            InitializeComponent();
        }

        public void Open(string title, string textblock1Text, string op, int opCount)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Title = title;
                this.textblock1.Text = textblock1Text;
                this.Name = $"{op}{opCount}";
                this.Show();
            });
        }

        public void Minimize()
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Hide();
            });
        }

        public void Exit()
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

        public void UpdateFilesProcessed(double filesProcessed)
        {
            this.Dispatcher.Invoke(() =>
            {
                textblock_Progress.Text = $"Process file count: {filesProcessed}";
            });
        }
    }
}
