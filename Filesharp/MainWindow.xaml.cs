using System;
using System.Threading;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;

namespace Filesharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    /// Changes made since last commit to main:
    /// Implemented threading and progress indication for Move, Delete, Create Files, and Sort.
    /// 
    /// (internal) TODO:
    /// 1) Add actual number counts to progress indication
    /// 2) Utilize progress bar
    /// 
    /// (internal) ISSUES:
    /// 1) Currently, can only run one operation type at a time. I.e., cannot have two moves going on at once. You can have multiple operations of different types running, though.
    /// // POSSIBLE FIX: If (for example) the opMove declaration is moved inside the method, and then at the end of the method opMove.Dispatcher.BeginInvoke is used to start the thread?
    /// // Then, make declarative name and title be based on input (i.e., "opMove.txtFromE:\1\toE:\2\")
    /// 2) On closing of main window, program is still running in background (other windows being hidden not closed is culprit?)

    public partial class MainWindow : Window
    {
        // Operations index
        const int move = 0;
        const int delete = 1;
        const int createFiles = 2;
        const int sort = 3;

        // Operation is running windows declaration
        Operation_is_running opMove = new Operation_is_running();
        Operation_is_running opDel = new Operation_is_running();
        Operation_is_running opCreate = new Operation_is_running();
        Operation_is_running opSort = new Operation_is_running();

        // idk what this does exactly but it's important
        public MainWindow()
        {
            InitializeComponent();
        }

        // Hides a given control.
        public void hideControl(Control control)
        {
            control.Visibility = Visibility.Hidden;
        }

        // Shows a given control.
        public void showControl(Control control)
        {
            control.Visibility = Visibility.Visible;
        }

        // Moves files of a given filetype from a given source directory to a given destination directory.
        public void startMove(string sourceDirectory, string destDirectory, string filetype)
        {
            opMove.Open("Move", $"Moving all {filetype} files from {sourceDirectory} to {destDirectory}, please wait");
            Thread threadMove = new Thread(() =>
            {
                int filesMoved = 0;
                DirectoryInfo sourceDir = new DirectoryInfo(@sourceDirectory);
                FileInfo[] filesToMove = sourceDir.GetFiles("*" + filetype);
                MessageBox.Show($"ready to move {filesToMove.Length} files");
                try
                {
                    foreach (FileInfo fileToMove in filesToMove)
                    {
                        File.Move(sourceDirectory + fileToMove.ToString(), destDirectory + fileToMove.ToString());
                        filesMoved++;
                        opMove.UpdateProgress(filesMoved, filesToMove.Length);
                    }
                    opMove.UpdateText("Done!");
                    MessageBox.Show($"Successfully moved {filesMoved} files");
                    opMove.Minimize();
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show("Error: Directory not found");
                    return;
                }
            });
            threadMove.Start();
        }

        // Deletes files of a given filetype from a given directory.
        public void startDelete(string sourceDirectory, string filetype)
        {
            opDel.Open("Delete", $"Deleting all {filetype} files from {sourceDirectory}, please wait");
            Thread threadDelete = new Thread(() =>
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
                            opDel.UpdateProgress(filesDeleted, filesToDelete.Length);
                        }
                        opDel.UpdateText("Done!");
                        MessageBox.Show($"Successfully deleted {filesDeleted} files");
                        opDel.Minimize();
                    }
                    catch (DirectoryNotFoundException)
                    {
                        MessageBox.Show("Error: Directory not found");
                        return;
                    }
                });
            threadDelete.Start();
        }

        // Creates a given number of files of a given size and filetype in a given directory.
        public void startCreateFiles(string directory, string filetype, string numOfFiles, string sizeInMB)
        {
            opCreate.Open("Create", $"Creating {numOfFiles} {sizeInMB}MB {filetype} files in {directory}, please wait");
            Thread threadCreateFiles = new Thread(() =>
            {
                int sizeInBytes = Int32.Parse(sizeInMB) * 1000000;
                int filesMade = 0;
                try
                {

                    for (int i = 0; i < Int32.Parse(numOfFiles); i++)
                    {
                        File.WriteAllBytes(directory + "file" + i.ToString() + filetype, new byte[sizeInBytes]);
                        filesMade++;
                        opCreate.UpdateProgress(filesMade, Int32.Parse(numOfFiles));
                    }
                    opCreate.UpdateText("Done!");
                    MessageBox.Show($"Successfully made {filesMade} {sizeInMB}MB {filetype} files in {directory}");
                    opCreate.Minimize();
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show("Error: Directory not found");
                    return;
                }
            });
            threadCreateFiles.Start();
        }

        // Automagically sorts pictures, documents, videos, and audio from a given source directory into a given destination directory.
        public void startSort(string sourceDirectory, string destDirectory)
        {
            // Look into Dictionary for optimization
            // https://stackoverflow.com/questions/24917532/can-you-create-variables-in-a-loop-c-sharp
            // https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2?view=netframework-4.7.2

            opSort.Open("Sort", "Sorting files, please wait");

            Thread threadSort = new Thread(() =>
            {
                // Ints that will be used to track progress
                int filesSorted = 0;
                int filesToSort = 0;

                // Create filetype arrays for each category
                string[] pictureFiletypes = { ".jpg", ".jpeg", ".png", ".gif", ".tiff" };
                string[] documentFiletypes = { ".txt", ".doc", ".docx", ".xml", ".xlsx", ".pdf" };
                string[] videoFiletypes = { ".mp4", ".mov", ".wmv", ".avi" };
                string[] audioFiletypes = { ".mp3", ".wav", ".aac" };

                // Directory declaration and creation
                string picDir = destDirectory + "Pictures\\";
                string docDir = destDirectory + "Documents\\";
                string vidDir = destDirectory + "Videos\\";
                string audDir = destDirectory + "Audio\\";
                Directory.CreateDirectory(picDir);
                Directory.CreateDirectory(docDir);
                Directory.CreateDirectory(vidDir);
                Directory.CreateDirectory(audDir);
                DirectoryInfo sourceDir = new DirectoryInfo(@sourceDirectory);
                List<FileInfo> picturesToSort = new List<FileInfo>();
                List<FileInfo> documentsToSort = new List<FileInfo>();
                List<FileInfo> videosToSort = new List<FileInfo>();
                List<FileInfo> audioToSort = new List<FileInfo>();

                try
                {
                    // Gather pictures to move
                    for (int i = 0; i < pictureFiletypes.Length; i++)
                    {
                        FileInfo[] filesToAdd = sourceDir.GetFiles("*" + pictureFiletypes[i]);
                        foreach (FileInfo file in filesToAdd)
                        {
                            picturesToSort.Add(file);
                            filesToSort++;
                        }
                    }
                    
                    // Gather documents to move
                    for (int i = 0; i < documentFiletypes.Length; i++)
                    {
                        FileInfo[] filesToAdd = sourceDir.GetFiles("*" + documentFiletypes[i]);
                        foreach (FileInfo file in filesToAdd)
                        {
                            documentsToSort.Add(file);
                            filesToSort++;
                        }
                    }

                    // Gather videos to move
                    for (int i = 0; i < videoFiletypes.Length; i++)
                    {
                        FileInfo[] filesToAdd = sourceDir.GetFiles("*" + videoFiletypes[i]);
                        foreach (FileInfo file in filesToAdd)
                        {
                            videosToSort.Add(file);
                            filesToSort++;
                        }
                    }
                    // Gather audio files to move
                    for (int i = 0; i < audioFiletypes.Length; i++)
                    {
                        FileInfo[] filesToAdd = sourceDir.GetFiles("*" + audioFiletypes[i]);
                        foreach (FileInfo file in filesToAdd)
                        {
                            audioToSort.Add(file);
                            filesToSort++;
                        }
                    }
                    // Finish gathering file list(s): begin actual sorting

                    // Sort pictures
                    foreach (FileInfo picToSort in picturesToSort)
                    {
                        File.Move(sourceDir + picToSort.ToString(), picDir + picToSort.ToString());
                        filesSorted++;
                        opSort.UpdateProgress(filesSorted, filesToSort);
                    }

                    // Sort documents
                    foreach (FileInfo docToSort in documentsToSort)
                    {
                        File.Move(sourceDir + docToSort.ToString(), docDir + docToSort.ToString());
                        filesSorted++;
                        opSort.UpdateProgress(filesSorted, filesToSort);
                    }

                    // Sort videos
                    foreach (FileInfo vidToSort in videosToSort)
                    {
                        File.Move(sourceDir + vidToSort.ToString(), vidDir + vidToSort.ToString());
                        filesSorted++;
                        opSort.UpdateProgress(filesSorted, filesToSort);
                    }

                    // Sort audio
                    foreach (FileInfo audToSort in audioToSort)
                    {
                        File.Move(sourceDir + audToSort.ToString(), audDir + audToSort.ToString());
                        filesSorted++;
                        opSort.UpdateProgress(filesSorted, filesToSort);
                    }
                    opSort.UpdateText("Done!");
                    MessageBox.Show($"Successfully sorted {filesSorted} files!");
                    opSort.Minimize();
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show("Error: Directory not found");
                    return;
                }
            });
            threadSort.Start();
        }



        // When "execute" button is clicked, runs the appropriate method based on what is selected in the comboBox.
        private void button_Execute_Click(object sender, RoutedEventArgs e)
        {
            int operationToExecute = comboBox1.SelectedIndex;

            if (operationToExecute == move)
            {
                opMove.Dispatcher.BeginInvoke(new Action(() => startMove(textbox1.Text, textbox2.Text, textbox3.Text)));
            }
            else if (operationToExecute == delete)
            {
                opDel.Dispatcher.BeginInvoke(new Action(() => startDelete(textbox1.Text, textbox2.Text)));
            }
            else if (operationToExecute == createFiles)
            {
                opCreate.Dispatcher.BeginInvoke(new Action(() => startCreateFiles(textbox1.Text, textbox2.Text, textbox3.Text, textbox4.Text)));
            }
            else if (operationToExecute == sort)
            {
                opSort.Dispatcher.BeginInvoke(new Action(() => startSort(textbox1.Text, textbox2.Text)));
            }
            else
            {
                MessageBox.Show("Error: In button_Execute_Click, operationToExecute does not equal any potential operations");
            }
        }

        // Whenever the comboBox dropdown is closed, adjusted each textbox's text and visibility accordingly.
        private void cb1_dropDownClosed(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == move)
            {
                hideControl(textbox4);
                showControl(textbox3);

                textbox1.Text = "Source directory (e.g., C:\\1\\)";
                textbox2.Text = "Destination directory (e.g., C:\\2\\";
                textbox3.Text = "Filetype to move (e.g., .png)";
            } else if (comboBox1.SelectedIndex == delete)
            {
                hideControl(textbox3);
                hideControl(textbox4);

                textbox1.Text = "Directory to delete files from (e.g., C:\\1\\)";
                textbox2.Text = "Filetype to delete (e.g., .png)";
            } else if (comboBox1.SelectedIndex == createFiles)
            {
                showControl(textbox3);
                showControl(textbox4);

                textbox1.Text = "Directory to create files (e.g., C:\\1\\)";
                textbox2.Text = "Filetype to create (e.g., .png)";
                textbox3.Text = "Number of files to create (must be integer)";
                textbox4.Text = "Size to make files (in MB, must be integer)";
            } else if (comboBox1.SelectedIndex == sort)
            {
                hideControl(textbox3);
                hideControl(textbox4);

                textbox1.Text = "Directory containing unsorted files (e.g., C:\\1\\)";
                textbox2.Text = "Directory to place sorted files (e.g., C:\\2\\)";
            }
            else
            {
                MessageBox.Show("Error: In cb1_dropDownClosed, comboBox1.SelectedIndex does not equal any potential operations");
            }
        }
    }
}
