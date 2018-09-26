using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Filesharp_Rebuild
{
    class Operations
    {
        public void moveFiles(string sourceDirectory, string destinationDirectory, string filetype, bool recursive)
        {
            // Var declarations
            double filesMoved = 0.0;
            int runningMoveOps = 0;
            DirectoryInfo sourceDir = new DirectoryInfo(sourceDirectory);
            Progress moveOpProgress = new Progress();

            // Set values for the progress windows
            // This (hopefully) allows for multiple progress windows to be open without interfering with each other
            moveOpProgress.Name = $"Move #{runningMoveOps}";
            moveOpProgress.Title = $"Move #{runningMoveOps}";
            moveOpProgress.Show();

            // First, move all files out of the source directory
            foreach(var file in sourceDir.EnumerateFiles("*" + filetype))
            {
                file.MoveTo(destinationDirectory + file.ToString());
                filesMoved++;
                moveOpProgress.updateProgress(filesMoved);
            }

            // Next, create a new array and place all subdirs in it.
            // The array is declared here to create the array with all elements at creation.
            DirectoryInfo[] subDirs = sourceDir.GetDirectories();
            
            // If there are subdirectories, iterate through subDirs[], calling moveFiles on each one.
            if (subDirs.Length > 0)
            {
                foreach (DirectoryInfo dir in subDirs)
                {
                    moveFiles(dir.ToString(), destinationDirectory, filetype, recursive);
                }
            }
            else
            {
                moveOpProgress.Close();
            }
        }
    }
}
