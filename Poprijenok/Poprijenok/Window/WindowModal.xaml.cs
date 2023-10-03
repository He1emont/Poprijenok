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
using System.Windows.Shapes;

namespace Poprijenok.Antuh
{
    /// <summary>
    /// Логика взаимодействия для WindowModal.xaml
    /// </summary>
    public partial class WindowModal : Window
    {
        public WindowModal(int pr)
        {
            InitializeComponent();

            priorety.Text = pr.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            helper.flag = true;
            helper.prioritet = Convert.ToInt32(priorety.Text);
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
