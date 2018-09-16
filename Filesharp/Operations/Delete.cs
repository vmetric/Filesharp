using System;
using System.Threading;
using System.IO;
using System.Windows;

namespace Filesharp.Operations
{
    class Delete
    {
        int deleteOpsRunning = 0;

        // Deletes files of a given filetype from a given directory.
        public void startDelete(string sourceDirectory, string filetype, bool isRecursive)
        {
            Operation_is_running opDelete = new Operation_is_running();
            DirectoryInfo sourceDir = new DirectoryInfo(sourceDirectory);
            DirectoryInfo[] subDirs = sourceDir.GetDirectories();

            if (deleteOpsRunning == 0)
            {
                opDelete.Open("Delete", $"Deleting all {filetype} files from {sourceDirectory}, please wait", "Delete", deleteOpsRunning);
            }
            else if (deleteOpsRunning > 0 && !isRecursive)
            {
                opDelete.Open("Delete", $"Deleting all {filetype} files from {sourceDirectory}, please wait", "Delete", deleteOpsRunning);
            }
            else
            {
                // do nothing
            }
            deleteOpsRunning++;

            if (isRecursive)
            {
                foreach (DirectoryInfo dir in subDirs)
                {
                    startDelete(sourceDirectory + dir.ToString() + "\\", filetype, isRecursive);
                }
            }

            Thread threadDelete = new Thread(() =>
            {
                int filesDeleted = 0;
                FileInfo[] filesToDelete = sourceDir.GetFiles("*" + filetype);
                try
                {
                    foreach (FileInfo fileToDelete in filesToDelete)
                    {
                        File.Delete(sourceDirectory + fileToDelete.ToString());
                        filesDeleted++;
                        opDelete.UpdateProgress(filesDeleted, filesToDelete.Length);
                    }
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

    }
}
