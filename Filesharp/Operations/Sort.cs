using System;
using System.Threading;
using System.IO;
using System.Windows;
using System.Collections.Generic;

namespace Filesharp.Operations
{
    public class Sort
    {
        double filesSorted = 0;
        // Int to keep track of running Sort operations.
        int sortOpsRunning = 0;
        Operation_is_running opSort = new Operation_is_running();

        public void startSort(string dirToSortFrom, string dirToSortTo, bool isRecursive)
        {
            // Look into Dictionary for optimization

            DirectoryInfo sourceDir = new DirectoryInfo(dirToSortFrom);
            DirectoryInfo[] subDirs = sourceDir.GetDirectories();
            Thread opSortThread = new Thread(() => opSort.Open($"Sorting from {dirToSortFrom} to {dirToSortTo}", "Sorting files, please wait", "Sort", sortOpsRunning));

            if (sortOpsRunning == 0)
            {
                opSortThread.Start();
            }
            else if (sortOpsRunning > 0 && !isRecursive)
            {
                opSortThread.Start();
            }
            else
            {
                // do nothing
            }
            sortOpsRunning++;
            
            if (isRecursive)
            {
                foreach (DirectoryInfo dir in subDirs)
                {
                    //Thread sortThread = new Thread(() =>
                    //{
                        startSort(dirToSortFrom + dir.ToString() + "\\", dirToSortTo, isRecursive);
                    //});
                    //sortThread.Start();
                }
            }

            Thread threadSort = new Thread(() =>
            {
                try
                {
                    sortPictures(dirToSortFrom, dirToSortTo);
                    sortDocuments(dirToSortFrom, dirToSortTo);
                    sortVideos(dirToSortFrom, dirToSortTo);
                    sortAudio(dirToSortFrom, dirToSortTo);
                } catch (Exception exc)
                {
                    MessageBox.Show($"Error sorting: {exc}");
                    
                    return;
                }
                sortOpsRunning--;
                opSort.Exit();
            });
            //opSort.Dispatcher.BeginInvoke((Action)delegate {  });
            threadSort.Start();
        }       
        public void sortPictures(string sourceDirectory, string destinationDirectory)
        {
            // Declarations
            int picsSorted = 0;
            string[] pictureFiletypes = { ".jpg", ".jpeg", ".png", ".gif", ".tiff", ".bmp", ".svg" };
            string picDir = destinationDirectory + "Pictures\\";
            DirectoryInfo sourceDir = new DirectoryInfo(sourceDirectory);

            // Create pictures directory
            Directory.CreateDirectory(picDir);

            foreach (string filetype in pictureFiletypes)
            {
                foreach (var file in Directory.EnumerateFiles(@sourceDir.ToString(), "*" + filetype))
                {
                    FileInfo feel = new FileInfo(file);
                    feel.MoveTo(picDir + feel.Name);
                    picsSorted++;
                    filesSorted++;
                    opSort.UpdateFilesProcessed(filesSorted);
                }
            }
        }
        public void sortDocuments(string sourceDirectory, string destinationDirectory)
        {
            // Declarations
            int docsSorted = 0;
            string[] documentFiletypes = { ".txt", ".doc", ".docx", ".xml", ".xlsx", ".pdf", ".xls", ".rtf", ".ppt", ".pptx" };
            string docDir = destinationDirectory + "Documents\\";
            DirectoryInfo sourceDir = new DirectoryInfo(sourceDirectory);

            // Create documents directory
            Directory.CreateDirectory(docDir);

            foreach (string filetype in documentFiletypes)
            {
                foreach (var file in Directory.EnumerateFiles(@sourceDir.ToString(), "*" + filetype))
                {
                    FileInfo feel = new FileInfo(file);
                    feel.MoveTo(docDir + feel.Name);
                    docsSorted++;
                    filesSorted++;
                    opSort.UpdateFilesProcessed(filesSorted);
                }
            }
        }
        public void sortVideos(string sourceDirectory, string destinationDirectory)
        {
            // Declarations
            int vidsSorted = 0;
            string[] videoFiletypes = { ".mp4", ".mov", ".wmv", ".avi" };
            string vidDir = destinationDirectory + "Videos\\";
            DirectoryInfo sourceDir = new DirectoryInfo(sourceDirectory);
            List<FileInfo> videosToSort = new List<FileInfo>();

            // Create video directory
            Directory.CreateDirectory(vidDir);

            foreach (string filetype in videoFiletypes)
            {
                foreach (var file in Directory.EnumerateFiles(@sourceDir.ToString(), "*" + filetype))
                {
                    FileInfo feel = new FileInfo(file);
                    feel.MoveTo(vidDir + feel.Name);
                    vidsSorted++;
                    filesSorted++;
                    opSort.UpdateFilesProcessed(filesSorted);
                }
            }
        }
        public void sortAudio(string sourceDirectory, string destinationDirectory)
        {
            // Declarations
            int audSorted = 0;
            string[] audioFiletypes = { ".mp3", ".wav", ".aac" };
            string audDir = destinationDirectory + "Audio\\";
            List<FileInfo> audioToSort = new List<FileInfo>();
            DirectoryInfo sourceDir = new DirectoryInfo(sourceDirectory);

            // Create audio directory
            Directory.CreateDirectory(audDir);

            foreach (string filetype in audioFiletypes)
            {
                foreach (var file in Directory.EnumerateFiles(@sourceDir.ToString(), "*" + filetype))
                {
                    FileInfo feel = new FileInfo(file);
                    feel.MoveTo(audDir + feel.Name);
                    audSorted++;
                    filesSorted++;
                    opSort.UpdateFilesProcessed(filesSorted);
                }
            }
        }
    }
}


