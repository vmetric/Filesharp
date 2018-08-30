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

        public void startMove(string sourceDirectory, string destDirectory, string filetype)
        {
            int filesMoved = 0;

            DirectoryInfo sourceDir = new DirectoryInfo(@sourceDirectory);
            FileInfo[] filesToMove = sourceDir.GetFiles("*" + filetype);

            try
            {
                foreach (FileInfo fileToMove in filesToMove)
                {
                    File.Move(sourceDirectory + fileToMove.ToString(), destDirectory + fileToMove.ToString());

                    filesMoved++;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error!");
                return;
                //throw;
            }
            MessageBox.Show("Successfully moved " + filesMoved.ToString() + " files!");
        }

        private void button_Execute_Click(object sender, RoutedEventArgs e)
        {
            int operationToExecute = comboBox1.SelectedIndex;

            if (operationToExecute == move)
            {
                startMove(textbox1.Text, textbox2.Text, textbox3.Text);
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

        private void comboBox1SelChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void cb1_dropDownClosed(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == move)
            {
                textbox1.Text = "Source directory (e.g., C:\\1\\)";
                textbox2.Text = "Destination directory (e.g., C:\\2\\";
                textbox3.Text = "Filetype to move (e.g., .png)";

                hideControl(textbox4);
            }
            if (comboBox1.SelectedIndex == delete)
            {
                textbox1.Text = "Directory to delete files from (e.g., C:\\1\\)";
                textbox2.Text = "Filetype to delete (e.g., .png)";

                hideControl(textbox3);
                hideControl(textbox4);
            }
        }
    }
}
