using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Filesharp_Rebuild
{
    class Operations
    { 
        /// <summary>
        /// Look into arraylist for gathering array of files
        /// </summary>

        public void moveFiles(string sourceDirectory, string destinationDirectory, string filetype, bool recursive)
        {
            // Var declarations
            double filesMoved = 0.0;
            int runningMoveOps = 0;
            DirectoryInfo sourceDir = new DirectoryInfo(sourceDirectory);
            Progress moveOpProgress = new Progress();

            // Set values for the progress windows
            // This (hopefully) allows for multiple progress windows to be open without interfering with each other
            moveOpProgress.Name = $"Move{runningMoveOps}";
            moveOpProgress.Title = $"Move{runningMoveOps}";
            moveOpProgress.Show();

            // First, move all files out of the source directory
            foreach(var file in sourceDir.EnumerateFiles("*" + filetype))
            {
                file.MoveTo(Path.Combine(destinationDirectory, file.ToString()));
                filesMoved++;
                moveOpProgress.updateProgress(filesMoved);
            }

            // Next, create a new array and place all subdirs in it.
            // The array is declared here to create the array with all elements at creation.
            DirectoryInfo[] subDirs = sourceDir.GetDirectories();
            
            // Checks if recursion is true. If it is:
            // Check if there are subdirectories, iterate through subDirs[], calling moveFiles on each one.
            if (recursive)
            {
                if (subDirs.Length > 0)
                {
                    foreach (DirectoryInfo dir in subDirs)
                    {
                        moveFiles(Path.Combine(sourceDirectory.ToString(), dir.ToString()), destinationDirectory, filetype, recursive);
                    }
                }
                else
                {
                    moveOpProgress.Close();
                }
            }
            else
            {
                moveOpProgress.Close();
            }

        }
        public void deleteFiles(string sourceDirectory, string filetype, bool recursive)
        {
            // Var declarations
            double filesDeleted = 0.0;
            int runningDeleteOps = 0;
            DirectoryInfo sourceDir = new DirectoryInfo(sourceDirectory);
            Progress deleteOpProgress = new Progress();

            // Set values for the progress windows
            // This (hopefully) allows for multiple progress windows to be open without interfering with each other
            deleteOpProgress.Name = $"Delete{runningDeleteOps}";
            deleteOpProgress.Title = $"Delete{runningDeleteOps}";
            deleteOpProgress.Show();

            // First, delete all files from the source directory
            foreach (var file in sourceDir.EnumerateFiles("*" + filetype))
            {
                file.Delete();
                filesDeleted++;
                deleteOpProgress.updateProgress(filesDeleted);
            }

            // Next, create a new array and place all subdirs in it.
            // The array is declared here to create the array with all elements at creation.
            DirectoryInfo[] subDirs = sourceDir.GetDirectories();

            // Check if recursion is true. If it is:
            // Check if there are subdirectories, iterate through subDirs[], calling deleteFiles on each one.
            if (recursive)
            {
                if (subDirs.Length > 0)
                {
                    foreach (DirectoryInfo dir in subDirs)
                    {
                        deleteFiles(Path.Combine(sourceDirectory.ToString(), dir.ToString()), filetype, recursive);
                    }
                    deleteOpProgress.Close();
                }
                else
                {
                    deleteOpProgress.Close();
                }
            }
            else
            {
                deleteOpProgress.Close();
            }
        }
        public void createFiles(string directory, string filetype, string filesizeInGB, string numOfFilesToCreate)
        {
            // Var declarations
            double filesCreated = 0.0;
            int runningCreateOps = 0;
            uint filecount = 0;
            ulong filesizeInBytes = 0;

            try
            {
                filecount = UInt32.Parse(numOfFilesToCreate);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error converting numOfFilesToCreate to UInt32: {ex}");
                return;
            }
            try
            {
                 filesizeInBytes = Convert.ToUInt64(filecount * Math.Pow(10, 9));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error converting filesizeInGB to Double: {ex}");
            }
            DirectoryInfo dir = new DirectoryInfo(directory);
            Progress createOpProgress = new Progress();

            // Set values for the progress windows
            // This (hopefully) allows for multiple progress windows to be open without interfering with each other
            createOpProgress.Name = $"Create{runningCreateOps}";
            createOpProgress.Title = $"Create{runningCreateOps}";
            createOpProgress.Show();
            runningCreateOps++;

            // Create files
            for (uint i = 0; i < filecount; i++)
            {
                File.Create(Path.Combine(directory, $"file{i}"));
                File.WriteAllBytes(Path.Combine(directory, $"file{i}", filetype), new byte[filesizeInBytes]);
                filesCreated++;
            }
             createOpProgress.Close();
        }
        //public void sortFiles(string sourceDirectory, string destinationDirectory, string intensity, bool recursive)
        //{
        //    // Var declarations
        //    double filesSorted = 0.0;
        //    int runningSortOps = 0;
        //    DirectoryInfo sourceDir = new DirectoryInfo(sourceDirectory);
        //    Progress sortOpProgress = new Progress();

        //    // Set values for the progress windows
        //    // This (hopefully) allows for multiple progress windows to be open without interfering with each other
        //    sortOpProgress.Name = $"Move #{runningSortOps}";
        //    sortOpProgress.Title = $"Move #{runningSortOps}";
        //    sortOpProgress.Show();

        //    // First, move all files out of the source directory
        //    foreach (var file in sourceDir.EnumerateFiles("*" + filetype))
        //    {
        //        file.MoveTo(Path.Combine(destinationDirectory, file.ToString()));
        //        filesMoved++;
        //        moveOpProgress.updateProgress(filesMoved);
        //    }

        //    // Next, create a new array and place all subdirs in it.
        //    // The array is declared here to create the array with all elements at creation.
        //    DirectoryInfo[] subDirs = sourceDir.GetDirectories();

        //    // If there are subdirectories, iterate through subDirs[], calling moveFiles on each one.
        //    if (subDirs.Length > 0)
        //    {
        //        foreach (DirectoryInfo dir in subDirs)
        //        {
        //            moveFiles(dir.ToString(), destinationDirectory, filetype, recursive);
        //        }
        //    }
        //    else
        //    {
        //        moveOpProgress.Close();
        //    }
        //}

    }
}
