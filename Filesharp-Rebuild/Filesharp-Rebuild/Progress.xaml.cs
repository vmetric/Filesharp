using System.Windows;


namespace Filesharp_Rebuild
{
    /// <summary>
    /// Interaction logic for Progress.xaml
    /// </summary>
    public partial class Progress : Window
    {
        public Progress()
        {
            InitializeComponent();
        }
        public void updateProgress(double filesProcessed)
        {
            labelProgress.Content = $"Files processed: {filesProcessed}";
        }
    }
}
