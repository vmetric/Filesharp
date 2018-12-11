using System;
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
    }
}
