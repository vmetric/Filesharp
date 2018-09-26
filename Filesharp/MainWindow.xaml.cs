using System;
using System.Windows;
using System.Windows.Controls;

namespace Filesharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary> 
    /// (internal) TODO:
    /// 1) Add actual number counts to progress indication
    /// 2) Utilize progress bar
    /// 3) Add ability to cancel an operation (maybe a button or exit on close window?)
    /// 4) Add confirmation dialogue after an operation finishes (for Move, Delete, and Sort)
    /// 5) Maybe a speed indicator w/ an ETA?
    /// 
    /// (internal) BUGS:
    /// 1) *MASSIVE* Memory usage when deleting large amounts of files 
    /// (experienced when attempting to delete more than 50k files totalling over 30GB of space)
    /// 2) Program does not completely exit on window close. Maybe a missed setting somewhere?

    public partial class MainWindow : Window
    {
        // Declaration of class objects
        Operations.Sort SortingMethods = new Operations.Sort();
        Operations.Move MoveMethods = new Operations.Move();
        Operations.Delete DeleteMethods = new Operations.Delete();
        Operations.Create_Files CreateMethods = new Operations.Create_Files();

        // Operations index
        const int move = 0;
        const int delete = 1;
        const int createFiles = 2;
        const int sort = 3;
        bool isRecursive = false;

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

        // When "execute" button is clicked, runs the appropriate method based on what is selected in the comboBox.
        private void button_Execute_Click(object sender, RoutedEventArgs e)
        {
            int operationToExecute = comboBox1.SelectedIndex;

            if (operationToExecute == move)
            {
                MoveMethods.startMove(textbox1.Text, textbox2.Text, textbox3.Text, isRecursive);
            }
            else if (operationToExecute == delete)
            {
                DeleteMethods.startDelete(textbox1.Text, textbox2.Text, isRecursive);
            }
            else if (operationToExecute == createFiles)
            {
                CreateMethods.startCreateFiles(textbox1.Text, textbox2.Text, textbox3.Text, textbox4.Text);
            }
            else if (operationToExecute == sort)
            {
                SortingMethods.startSort(textbox1.Text, textbox2.Text, isRecursive);
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
                showControl(checkbox_Recursive);

                textbox1.Text = "Source directory (e.g., C:\\1\\)";
                textbox2.Text = "Destination directory (e.g., C:\\2\\";
                textbox3.Text = "Filetype to move (e.g., .png)";
            } else if (comboBox1.SelectedIndex == delete)
            {
                hideControl(textbox3);
                hideControl(textbox4);
                showControl(checkbox_Recursive);

                textbox1.Text = "Directory to delete files from (e.g., C:\\1\\)";
                textbox2.Text = "Filetype to delete (e.g., .png)";
            } else if (comboBox1.SelectedIndex == createFiles)
            {
                hideControl(checkbox_Recursive);
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
                showControl(checkbox_Recursive);

                textbox1.Text = "Directory containing unsorted files (e.g., C:\\1\\)";
                textbox2.Text = "Directory to place sorted files (e.g., C:\\2\\)";
            }
            else
            {
                MessageBox.Show("Error: In cb1_dropDownClosed, comboBox1.SelectedIndex does not equal any potential operations");
            }
        }

        // When the Recursive checkbox is clicked, set isRecursive to false if unchecked, or true if checked.
        private void checkbox_Recursive_Click(object sender, RoutedEventArgs e)
        {
            isRecursive = checkbox_Recursive.IsChecked.HasValue ? checkbox_Recursive.IsChecked.Value : false;
        }
    }
}
