using System;
using System.Threading;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Filesharp
{
    class Move
    {
        int moveOpsRunning = 0;

        public void startMove(string sourceDirectory, string destDirectory, string filetype, bool isRecursive)
        {
            Operation_is_running opMove = new Operation_is_running();
            DirectoryInfo sourceDir = new DirectoryInfo(sourceDirectory);
            DirectoryInfo[] subDirs = sourceDir.GetDirectories();
            
            if (moveOpsRunning == 0)
            {
                opMove.Open("Move", $"Moving all {filetype} files from {sourceDirectory} to {destDirectory}, please wait", "Move", moveOpsRunning);
            } else if (moveOpsRunning > 0 && !isRecursive)
            {
                opMove.Open("Move", $"Moving all {filetype} files from {sourceDirectory} to {destDirectory}, please wait", "Move", moveOpsRunning);
            }
            else
            {
                // do nothing
            }
            moveOpsRunning++;

            if (isRecursive)
            {
                foreach (DirectoryInfo dir in subDirs)
                {
                    startMove(sourceDirectory + dir.ToString() + "\\", destDirectory, filetype, isRecursive);
                }
            }

            Thread threadMove = new Thread(() =>
            {
                int filesMoved = 0;
                FileInfo[] filesToMove = sourceDir.GetFiles("*" + filetype);
                try
                {
                    foreach (FileInfo fileToMove in filesToMove)
                    {
                        File.Move(sourceDirectory + fileToMove.ToString(), destDirectory + fileToMove.ToString());
                        filesMoved++;
                        opMove.UpdateProgress(filesMoved, filesToMove.Length);
                    }
                    opMove.UpdateText("Done!");
                    moveOpsRunning--;
                    opMove.Exit();
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show("Error: Directory not found");
                    return;
                }
            });
            opMove.Dispatcher.BeginInvoke(new Action(() => threadMove.Start()));
        }
    }
}
