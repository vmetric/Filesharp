﻿using System;
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
    public partial class MainWindow : Window
    {
        // Operations index
        const int move = 0;
        const int delete = 1;
        const int createFiles = 2;
        const int sort = 3;

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
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Error: Directory not found");
                return;
            }
            MessageBox.Show($"Successfully moved {filesMoved} files");
        }

        // Deletes files of a given filetype from a given directory.
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
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Error: Directory not found");
                return;
            }
            MessageBox.Show($"Successfully deleted {filesDeleted} files");
        }

        // Creates a given number of files of a given size and filetype in a given directory.
        public void startCreateFiles(string directory, string filetype, string numOfFiles, string sizeInMB)
        {
            try
            {
                int sizeInBytes = Int32.Parse(sizeInMB) * 1000000;
                int filesMade = 0;
                for (int i = 0; i < Int32.Parse(numOfFiles); i++)
                {
                    File.WriteAllBytes(directory + "file" + i.ToString() + filetype, new byte[sizeInBytes]);
                    filesMade++;
                }
                MessageBox.Show($"Successfully made {filesMade} files");
            }
            catch (Exception)
            {
                MessageBox.Show("Error!");
                return;
            }
        }

        // Automagically sorts pictures, documents, videos, and audio from a given source directory into a given destination directory.
        public void startSort(string sourceDirectory, string destDirectory)
        {
            // Look into Dictionary for optimization
            // https://stackoverflow.com/questions/24917532/can-you-create-variables-in-a-loop-c-sharp
            // https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2?view=netframework-4.7.2

            int filesSorted = 0;
            int filesToSort = 0;
            string[] pictureFiletypes = { ".jpg", ".jpeg", ".png", ".gif", ".tiff" };
            string[] documentFiletypes = { ".txt", ".doc", ".docx", ".xml", ".xlsx", ".pdf" };
            string[] videoFiletypes = { ".mp4", ".mov", ".wmv", ".avi" };
            string[] audioFiletypes = { ".mp3", ".wav", ".aac" };
            string picDir = destDirectory + "Pictures\\";
            string docDir = destDirectory + "Documents\\";
            string vidDir = destDirectory + "Videos\\";
            string audDir = destDirectory + "Audio\\";
            Directory.CreateDirectory(picDir);
            Directory.CreateDirectory(docDir);
            Directory.CreateDirectory(vidDir);
            Directory.CreateDirectory(audDir);
            DirectoryInfo sourceDir = new DirectoryInfo(@sourceDirectory);

            try
            {
                // Gather pictures to move
                List<FileInfo> picturesToMove = sourceDir.GetFiles("*" + pictureFiletypes[0]).ToList();
                for (int i = 1; i < pictureFiletypes.Length; i++)
                {
                    FileInfo[] filesToAdd = sourceDir.GetFiles("*" + pictureFiletypes[i]);
                    foreach (FileInfo file in filesToAdd)
                    {
                        picturesToMove.Add(file);
                        filesToSort++;
                    }
                }

                // Gather documents to move
                List<FileInfo> documentsToSort = sourceDir.GetFiles("*" + documentFiletypes[0]).ToList();
                for (int i = 1; i < documentFiletypes.Length; i++)
                {
                    FileInfo[] filesToAdd = sourceDir.GetFiles("*" + documentFiletypes[i]);
                    foreach (FileInfo file in filesToAdd)
                    {
                        documentsToSort.Add(file);
                        filesToSort++;
                    }
                }

                // Gather videos to move
                List<FileInfo> videosToSort = sourceDir.GetFiles("*" + videoFiletypes[0]).ToList();
                for (int i = 1; i < videoFiletypes.Length; i++)
                {
                    FileInfo[] filesToAdd = sourceDir.GetFiles("*" + videoFiletypes[i]);
                    foreach (FileInfo file in filesToAdd)
                    {
                        videosToSort.Add(file);
                        filesToSort++;
                    }
                }
                // Gather audio files to move
                List<FileInfo> audioToSort = sourceDir.GetFiles("*" + audioFiletypes[0]).ToList();
                for (int i = 1; i < audioFiletypes.Length; i++)
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
                foreach (FileInfo picToSort in picturesToMove)
                {
                    File.Move(sourceDir + picToSort.ToString(), picDir + picToSort.ToString());
                    filesSorted++;
                }
                
                // Sort documents
                foreach (FileInfo docToSort in documentsToSort)
                {
                    File.Move(sourceDir + docToSort.ToString(), docDir + docToSort.ToString());
                    filesSorted++;
                }

                // Sort videos
                foreach (FileInfo vidToSort in videosToSort)
                {
                    File.Move(sourceDir + vidToSort.ToString(), vidDir + vidToSort.ToString());
                    filesSorted++;
                }

                // Sort audio
                foreach (FileInfo audToSort in audioToSort)
                {
                    File.Move(sourceDir + audToSort.ToString(), audDir + audToSort.ToString());
                    filesSorted++;
                }
                MessageBox.Show($"Successfully sorted {filesSorted} files!");
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Error: Directory not found");
                return;
            }
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
        }

        // Whenever the comboBox dropdown is closed, adjusted each textbox's text and visibility accordingly.
        private void cb1_dropDownClosed(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == move)
            {
                hideControl(textbox4);

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
        }
    }
}
