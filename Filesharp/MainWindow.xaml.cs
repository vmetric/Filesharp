using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Filesharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Operations index
        const int move = 0;
        const int delete = 1;
        const int createFiles = 2;
        const int sort = 3;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void hideControl(Control control)
        {
            control.Visibility = Visibility.Hidden;
        }
        public void showControl(Control control)
        {
            control.Visibility = Visibility.Visible;
        }

        public void startMove(string sourceDir, string destDir, string filetype, string recursive)
        {
            if (recursive == "True" || recursive == "true")
            {
                // NOTE TO SELF: directory.getfiles SHOULD WORK!
                // Look into it!
                string[] filesToMove = Directory.GetFiles(sourceDir, "*" + filetype);
                try
                {
                    MessageBox.Show(filesToMove[1]);
                }
                catch (Exception)
                {
                    MessageBox.Show("Error!");
                    throw;
                }


              //  var filesToMove = Directory.EnumerateFiles(sourceDir, "*.*")
           // .Where(s => s.EndsWith(filetype, StringComparison.OrdinalIgnoreCase));

               // string[] filesArray = filesToMove.ToArray<string>();
               // MessageBox.Show(filesToMove.ToString());
            }
        }

        private void button_Execute_Click(object sender, RoutedEventArgs e)
        {
            int operationToExecute = comboBox1.SelectedIndex;

            if (operationToExecute == move)
            {
                startMove(textbox1.Text, textbox2.Text, textbox3.Text, textbox4.Text);
            }
            else if (operationToExecute == delete)
            {
                // delete()
            }
            else if (operationToExecute == createFiles)
            {
                // createFiles()
            }
            else
            {
                // sort()
            }
        }
    }
}
