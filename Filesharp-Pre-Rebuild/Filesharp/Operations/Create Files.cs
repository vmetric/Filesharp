using System;
using System.Threading;
using System.IO;
using System.Windows;

namespace Filesharp.Operations
{
    class Create_Files
    {
        // Int to keep track of running Create Files operations.
        int createFilesOpsRunning = 0;

        // Creates a given number of files of a given size and filetype in a given directory.
        public void startCreateFiles(string directory, string filetype, string numOfFiles, string sizeInMB)
        {
            Operation_is_running opCreate = new Operation_is_running();

            opCreate.Open("Create", $"Creating {numOfFiles} {sizeInMB}MB {filetype} files in {directory}, please wait", "Creating_files", createFilesOpsRunning);
            createFilesOpsRunning++;

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
    }
}
