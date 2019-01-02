using System;
using System.Windows;


namespace Filesharp_Rebuild
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Declarations
        Operations ops = new Operations();
        // Operations index
        const int move = 0;
        const int delete = 1;
        const int create = 2;
        const int sort = 3;

        // Methods
        public MainWindow()
        {
            InitializeComponent();
        }



        private void buttonExecuteClick(object sender, RoutedEventArgs e)
        {
            if (comboBox_Operations.SelectedIndex == move)
            {
                ops.moveFiles(textbox1.Text, textbox2.Text, textbox3.Text, (bool)checkbox_Recursive.IsChecked);
            }
            else if (comboBox_Operations.SelectedIndex == delete)
            {
                ops.deleteFiles(textbox1.Text, textbox2.Text, (bool)checkbox_Recursive.IsChecked);
            }
            else if (comboBox_Operations.SelectedIndex == create)
            {
                ops.createFiles(textbox1.Text, textbox2.Text, textbox3.Text, textbox4.Text);
            }
            else if (comboBox_Operations.SelectedIndex == sort)
            {
                //ops.sortFiles(textbox1.Text, textbox2.Text, textbox3.Text, (bool)checkbox_Recursive.IsChecked);
            }
            else
            {
                MessageBox.Show("Please select an operation to execute");
            }
        }

        private void comboBox_Operations_DropDownClosed(object sender, EventArgs e)
        {
            if (comboBox_Operations.SelectedIndex == move)
            {
                textbox1.Text = "Source directory";
                textbox2.Text = "Destination directory";
                textbox3.Text = "Filetype (e.g., .png)";
                textbox4.Text = "";
                textbox3.Visibility = Visibility.Visible;
                textbox4.Visibility = Visibility.Hidden;
                checkbox_Recursive.Visibility = Visibility.Visible;
            }
            else if (comboBox_Operations.SelectedIndex == delete)
            {
                textbox1.Text = "Source directory";
                textbox2.Text = "Filetype (e.g., .png)";
                textbox3.Text = "";
                textbox4.Text = "";
                textbox3.Visibility = Visibility.Hidden;
                textbox4.Visibility = Visibility.Hidden;
                checkbox_Recursive.Visibility = Visibility.Visible;
            }
            else if (comboBox_Operations.SelectedIndex == create)
            {
                textbox1.Text = "Directory";
                textbox2.Text = "File extension";
                textbox3.Text = "File size";
                textbox4.Text = "File count";
                textbox1.Visibility = Visibility.Visible;
                textbox2.Visibility = Visibility.Visible;
                textbox3.Visibility = Visibility.Visible;
                textbox4.Visibility = Visibility.Visible;
                checkbox_Recursive.Visibility = Visibility.Hidden;
            }
            else if (comboBox_Operations.SelectedIndex == sort)
            {
                textbox1.Text = "Directory to sort from";
                textbox2.Text = "Directory to sort to";
                textbox1.Visibility = Visibility.Visible;
                textbox2.Visibility = Visibility.Visible;
                textbox3.Visibility = Visibility.Hidden;
                textbox4.Visibility = Visibility.Hidden;
                checkbox_Recursive.Visibility = Visibility.Hidden;
            }
        }
    }
}
