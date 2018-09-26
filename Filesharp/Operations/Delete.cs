using System;
using System.Threading;
using System.IO;
using System.Windows;
using System.Collections.Generic;

namespace Filesharp.Operations
{
    class Delete
    {
        // Int to keep track of running Delete operations.
        int deleteOpsRunning = 0;

        // Deletes files of a given filetype from a given directory.
        public void startDelete(string sourceDirectory, string filetype, bool isRecursive)
        {
            Operation_is_running opDelete = new Operation_is_running();
            DirectoryInfo sourceDir = new DirectoryInfo(sourceDirectory);
            DirectoryInfo[] subDirs = sourceDir.GetDirectories();
            Thread opDeleteThread = new Thread(() => opDelete.Open("Delete", $"Deleting all {filetype} files from {sourceDirectory}, please wait", "Delete", deleteOpsRunning));  

            if (deleteOpsRunning == 0)
            {
                opDeleteThread.Start();
            }
            else if (deleteOpsRunning > 0 && !isRecursive)
            {
                opDeleteThread.Start();
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
                var filesToDelete = Directory.EnumerateFiles(@sourceDir.ToString(), "*" + filetype);

                try
                {
                    foreach(var file in Directory.EnumerateFiles(@sourceDir.ToString(), "*" + filetype))
                    {
                        
                        File.Delete(file.ToString());
                        filesDeleted++;
                        opDelete.UpdateFilesProcessed(filesDeleted);
                    }
                    deleteOpsRunning--;
                    opDelete.Exit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            });
            opDelete.Dispatcher.BeginInvoke(new Action(() => threadDelete.Start()));
        }
    }
}
