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


namespace pr15_PCForYourself.Views
{
    public partial class SaveBuildDialog : Window
    {
        public string BuildName { get; private set; }
        public string Author { get; private set; }

        public SaveBuildDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            BuildName = BuildNameBox.Text;
            Author = AuthorBox.Text;
            if (string.IsNullOrWhiteSpace(BuildName) || string.IsNullOrWhiteSpace(Author))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;
        }
    }
}
