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
                MessageBox.Show("Error: Directory not found");
                return;
            }
            MessageBox.Show("Successfully moved " + filesMoved.ToString() + " files");
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
                MessageBox.Show("Error: Directory not found");
                return;
            }
            MessageBox.Show("Successfully deleted " + filesDeleted.ToString() + " files");
        }
        public void startCreateFiles(string directory, string filetype, string numOfFiles, string sizeInMB)
        {
            try
            {
                int sizeInBytes = Int32.Parse(sizeInMB) * 1000000;
                int filesMade = 0;
                for (int i = 0; i < Int32.Parse(numOfFiles); i++)
                {
                    System.IO.File.WriteAllBytes(directory + "file" + i.ToString() + filetype, new byte[sizeInBytes]);
                    filesMade++;
                }
                MessageBox.Show("Successfully made " + filesMade.ToString() + " files");
            }
            catch (Exception)
            {
                MessageBox.Show("Error!");
                return;
            }
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
                startCreateFiles(textbox1.Text, textbox2.Text, textbox3.Text, textbox4.Text);
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
            } else if (comboBox1.SelectedIndex == delete)
            {
                textbox1.Text = "Directory to delete files from (e.g., C:\\1\\)";
                textbox2.Text = "Filetype to delete (e.g., .png)";

                hideControl(textbox3);
                hideControl(textbox4);
            } else if (comboBox1.SelectedIndex == createFiles)
            {
                showControl(textbox3);
                showControl(textbox4);

                textbox1.Text = "Directory to create files (e.g., C:\\1\\)";
                textbox2.Text = "Filetype to create (e.g., .png)";
                textbox3.Text = "Number of files to create (must be integer)";
                textbox4.Text = "Size to make files (in MB, must be integer)";
            }
        }
    }
}
