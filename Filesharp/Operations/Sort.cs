using System;
using System.Threading;
using System.IO;
using System.Windows;
using System.Collections.Generic;

namespace Filesharp.Operations
{
    public class Sort

        /// Need to add:
        /// .svg
    {
        // Int to keep track of running Sort operations.
        int sortOpsRunning = 0;
        int filesToSort = 0;
        int filesSorted = 0;
        Operation_is_running opSort = new Operation_is_running();

        public void startSort(string dirToSortFrom, string dirToSortTo, bool isRecursive)
        {
            // Look into Dictionary for optimization

            DirectoryInfo sourceDir = new DirectoryInfo(dirToSortFrom);
            DirectoryInfo[] subDirs = sourceDir.GetDirectories();

            if (sortOpsRunning == 0)
            {
                opSort.Open($"Sorting from {dirToSortFrom} to {dirToSortTo}", "Sorting files, please wait", "Sort", sortOpsRunning);
            }
            else if (sortOpsRunning > 0 && !isRecursive)
            {
                opSort.Open($"Sorting from {dirToSortFrom} to {dirToSortTo}", "Sorting files, please wait", "Sort", sortOpsRunning);
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
                    startSort(dirToSortFrom + dir.ToString() + "\\", dirToSortTo, isRecursive);
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
            opSort.Dispatcher.BeginInvoke((Action)delegate { threadSort.Start(); });
        }       
        public void sortPictures(string sourceDirectory, string destinationDirectory)
        {
            // Declarations
            int picsToSort = 0;
            int picsSorted = 0;
            string[] pictureFiletypes = { ".jpg", ".jpeg", ".png", ".gif", ".tiff", ".bmp" };
            string picDir = destinationDirectory + "Pictures\\";
            List<FileInfo> picturesToSort = new List<FileInfo>();
            DirectoryInfo sourceDir = new DirectoryInfo(sourceDirectory);

            // Create pictures directory
            Directory.CreateDirectory(picDir);

            // Gather pictures to move
            for (int i = 0; i < pictureFiletypes.Length; i++)
            {
                FileInfo[] filesToAdd = sourceDir.GetFiles("*" + pictureFiletypes[i]);
                foreach (FileInfo file in filesToAdd)
                {
                    picturesToSort.Add(file);
                    picsToSort++;
                    filesToSort++;
                }
            }

            // Sort pictures
            foreach (FileInfo picToSort in picturesToSort)
            {
                File.Move(sourceDir.ToString() + "\\" + picToSort.ToString(), picDir + picToSort.ToString());
                picsSorted++;
                filesSorted++;
                opSort.UpdateProgress(filesSorted, filesToSort);
            }
        }
        public void sortDocuments(string sourceDirectory, string destinationDirectory)
        {
            // Declarations
            int docsToSort = 0;
            int docsSorted = 0;
            string[] documentFiletypes = { ".txt", ".doc", ".docx", ".xml", ".xlsx", ".pdf", ".xls", ".rtf", ".ppt", ".pptx" };
            string docDir = destinationDirectory + "Documents\\";
            DirectoryInfo sourceDir = new DirectoryInfo(sourceDirectory);
            List<FileInfo> documentsToSort = new List<FileInfo>();

            // Create documents directory
            Directory.CreateDirectory(docDir);

            // Gather documents to move
            for (int i = 0; i < documentFiletypes.Length; i++)
            {
                FileInfo[] filesToAdd = sourceDir.GetFiles("*" + documentFiletypes[i]);
                foreach (FileInfo file in filesToAdd)
                {
                    documentsToSort.Add(file);
                    docsToSort++;
                    filesToSort++;
                }
            }

            // Sort documents
            foreach (FileInfo docToSort in documentsToSort)
            {
                File.Move(sourceDir + docToSort.ToString(), docDir + docToSort.ToString());
                docsSorted++;
                filesSorted++;
                opSort.UpdateProgress(filesSorted, filesToSort);
            }
        }
        public void sortVideos(string sourceDirectory, string destinationDirectory)
        {
            // Declarations
            int vidsToSort = 0;
            int vidsSorted = 0;
            string[] videoFiletypes = { ".mp4", ".mov", ".wmv", ".avi" };
            string vidDir = destinationDirectory + "Videos\\";
            DirectoryInfo sourceDir = new DirectoryInfo(sourceDirectory);
            List<FileInfo> videosToSort = new List<FileInfo>();

            // Create video directory
            Directory.CreateDirectory(vidDir);

            // Gather videos to sort
            for (int i = 0; i < videoFiletypes.Length; i++)
            {
                FileInfo[] filesToAdd = sourceDir.GetFiles("*" + videoFiletypes[i]);
                foreach (FileInfo file in filesToAdd)
                {
                    videosToSort.Add(file);
                    vidsToSort++;
                    filesToSort++;
                }
            }

            // Sort videos
            foreach (FileInfo vidToSort in videosToSort)
            {
                File.Move(sourceDir.ToString() + "\\" + vidToSort.ToString(), vidDir + vidToSort.ToString());
                vidsSorted++;
                filesSorted++;
                opSort.UpdateProgress(filesSorted, filesToSort);
            }
        }
        public void sortAudio(string sourceDirectory, string destinationDirectory)
        {
            // Declarations
            int audToSort = 0;
            int audSorted = 0;
            string[] audioFiletypes = { ".mp3", ".wav", ".aac" };
            string audDir = destinationDirectory + "Audio\\";
            List<FileInfo> audioToSort = new List<FileInfo>();
            DirectoryInfo sourceDir = new DirectoryInfo(sourceDirectory);

            // Create audio directory
            Directory.CreateDirectory(audDir);

            // Gather audio files to move
            for (int i = 0; i < audioFiletypes.Length; i++)
            {
                FileInfo[] filesToAdd = sourceDir.GetFiles("*" + audioFiletypes[i]);
                foreach (FileInfo file in filesToAdd)
                {
                    audioToSort.Add(file);
                    audToSort++;
                    filesToSort++;
                }
            }

            // Sort audio
            foreach (FileInfo audioFile in audioToSort)
            {
                File.Move(sourceDirectory + audioFile.ToString(), audDir + audioFile.ToString());
                audSorted++;
                filesSorted++;
                opSort.UpdateProgress(filesSorted, filesToSort);
            }
        }
    }
}


