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
    /// NOTE!!!!
    /// Need to debug recursiveness of sort function.

    public partial class MainWindow : Window
    {
        // Operations index
        const int move = 0;
        const int delete = 1;
        const int createFiles = 2;
        const int sort = 3;

        // Ints to keep track of how many of each operation are currently open
        int moveOpsRunning = 0;
        int deleteOpsRunning = 0;
        int createFilesOpsRunning = 0;
        int sortOpsRunning = 0;

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
            Operation_is_running opMove = new Operation_is_running();
            opMove.Name = "opMove" + moveOpsRunning;
            moveOpsRunning++;
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
                    moveOpsRunning--;
                    opMove.Exit();
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show("Error: Directory not found");
                    return;
                }
            });
            //opMove.Dispatcher.BeginInvoke(new Action(() => ));
            opMove.Dispatcher.BeginInvoke(new Action(() => threadMove.Start()));
        }

        // Deletes files of a given filetype from a given directory.
        public void startDelete(string sourceDirectory, string filetype)
        {
            Operation_is_running opDelete = new Operation_is_running();
            opDelete.Name = "opDelete" + deleteOpsRunning;
            deleteOpsRunning++;
            opDelete.Open("Delete", $"Deleting all {filetype} files from {sourceDirectory}, please wait");
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
                            opDelete.UpdateProgress(filesDeleted, filesToDelete.Length);
                        }
                        opDelete.UpdateText("Done!");
                        MessageBox.Show($"Successfully deleted {filesDeleted} files");
                        deleteOpsRunning--;
                        opDelete.Exit();
                    }
                    catch (DirectoryNotFoundException)
                    {
                        MessageBox.Show("Error: Directory not found");
                        return;
                    }                    
                });
            opDelete.Dispatcher.BeginInvoke(new Action(() => threadDelete.Start()));
        }

        // Creates a given number of files of a given size and filetype in a given directory.
        public void startCreateFiles(string directory, string filetype, string numOfFiles, string sizeInMB)
        {
            Operation_is_running opCreate = new Operation_is_running();
            opCreate.Name = "opCreate" + createFilesOpsRunning;
            createFilesOpsRunning++;
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
                    createFilesOpsRunning--;
                    opCreate.Exit();
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show("Error: Directory not found");
                    return;
                }
            });
            opCreate.Dispatcher.BeginInvoke(new Action(() => threadCreateFiles.Start()));

        }

        // Automagically sorts pictures, documents, videos, and audio from a given source directory into a given destination directory.
        public void startSort(string directory, string destDirectory)
        {
            // Look into Dictionary for optimization
            // https://stackoverflow.com/questions/24917532/can-you-create-variables-in-a-loop-c-sharp
            // https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2?view=netframework-4.7.2
            Operation_is_running opSort = new Operation_is_running();
            opSort.Name = "opSort" + sortOpsRunning;
            opSort.Open("Sort", "Sorting files, please wait");
            sortOpsRunning++;

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
                DirectoryInfo dir = new DirectoryInfo(@directory);
                DirectoryInfo[] dirsInSource = dir.GetDirectories();
                List<FileInfo> picturesToSort = new List<FileInfo>();
                List<FileInfo> documentsToSort = new List<FileInfo>();
                List<FileInfo> videosToSort = new List<FileInfo>();
                List<FileInfo> audioToSort = new List<FileInfo>();

                try
                {
                    foreach (DirectoryInfo dur in dirsInSource)
                    {
                        // Gather pictures to move
                        for (int i = 0; i < pictureFiletypes.Length; i++)
                        {
                            FileInfo[] filesToAdd = dur.GetFiles("*" + pictureFiletypes[i]);
                            foreach (FileInfo file in filesToAdd)
                            {
                                picturesToSort.Add(file);
                                filesToSort++;
                            }
                        }

                        // Gather documents to move
                        for (int i = 0; i < documentFiletypes.Length; i++)
                        {
                            FileInfo[] filesToAdd = dur.GetFiles("*" + documentFiletypes[i]);
                            foreach (FileInfo file in filesToAdd)
                            {
                                documentsToSort.Add(file);
                                filesToSort++;
                            }
                        }

                        // Gather videos to move
                        for (int i = 0; i < videoFiletypes.Length; i++)
                        {
                            FileInfo[] filesToAdd = dur.GetFiles("*" + videoFiletypes[i]);
                            foreach (FileInfo file in filesToAdd)
                            {
                                videosToSort.Add(file);
                                filesToSort++;
                            }
                        }
                        // Gather audio files to move
                        for (int i = 0; i < audioFiletypes.Length; i++)
                        {
                            FileInfo[] filesToAdd = dur.GetFiles("*" + audioFiletypes[i]);
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
                            File.Move(dur + picToSort.ToString(), picDir + picToSort.ToString());
                            filesSorted++;
                            opSort.UpdateProgress(filesSorted, filesToSort);
                        }

                        // Sort documents
                        foreach (FileInfo docToSort in documentsToSort)
                        {
                            File.Move(dur + docToSort.ToString(), docDir + docToSort.ToString());
                            filesSorted++;
                            opSort.UpdateProgress(filesSorted, filesToSort);
                        }

                        // Sort videos
                        foreach (FileInfo vidToSort in videosToSort)
                        {
                            File.Move(dur + vidToSort.ToString(), vidDir + vidToSort.ToString());
                            filesSorted++;
                            opSort.UpdateProgress(filesSorted, filesToSort);
                        }

                        // Sort audio
                        foreach (FileInfo audToSort in audioToSort)
                        {
                            File.Move(dur + audToSort.ToString(), audDir + audToSort.ToString());
                            filesSorted++;
                            opSort.UpdateProgress(filesSorted, filesToSort);
                        }
                    }
                    opSort.UpdateText("Done!");
                    MessageBox.Show($"Successfully sorted {filesSorted} files!");
                    sortOpsRunning--;
                    opSort.Exit();
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show("Error: Directory not found");
                    return;
                }
            });
            opSort.Dispatcher.BeginInvoke(new Action(() => threadSort.Start()));
        }

        // When "execute" button is clicked, runs the appropriate method based on what is selected in the comboBox.
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
            else if (operationToExecute == sort)
            {
                startSort(textbox1.Text, textbox2.Text);
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
