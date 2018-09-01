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
            catch (System.IO.DirectoryNotFoundException)
            {
                MessageBox.Show("Error: No such directory!");
                return;
            }
            MessageBox.Show("Successfully moved " + filesMoved.ToString() + " files!");
        }
        public void startDelete(string sourceDirectory, string filetype)
        {
            int filesDeleted = 0;
            DirectoryInfo sourceDir = new DirectoryInfo(@sourceDirectory);
            FileInfo[] filesToDelete = sourceDir.GetFiles("*" + filetype);

            try
            {
                foreach (FileInfo fileToDelete in filesToDelete)
                {
                    File.Delete(sourceDirectory + fileToDelete.ToString());

                    filesDeleted++;
                }
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                MessageBox.Show("Error: No such directory!");
                return;
            }
            MessageBox.Show("Successfully moved " + filesDeleted.ToString() + " files!");
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
                startDelete(textbox1.Text, textbox2.Text);
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
