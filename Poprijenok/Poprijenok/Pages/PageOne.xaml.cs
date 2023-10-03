using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using Poprijenok.Antuh.Model;

namespace Poprijenok.Antuh
{
    /// <summary>
    /// Логика взаимодействия для PageOne.xaml
    /// </summary>
    public partial class PageOne : Page
    {
        private int start = 0;
        private int fullCount = 0;
        public PageOne(Frame frame)
        {
            InitializeComponent();
            Load();

        }
        public void Load()
        {
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            agentGrid.ItemsSource = helper.GetContext().Agent.OrderBy(Agent => Agent.ID).Skip(start * 10).Take(10).ToList();
            fullCount = helper.GetContext().Agent.Count();
            full.Text = fullCount.ToString();
            int ost = fullCount % 10;
            int pag = (fullCount - ost) / 10;
            if (ost > 0) pag++;
            pagin.Children.Clear();
            for (int i = 0; i < pag; i++)
            {
                Button myButton = new Button();
                myButton.Height = 30;
                myButton.Content = i + 1;
                myButton.Width = 20;
                myButton.HorizontalAlignment = HorizontalAlignment.Center;
                myButton.Tag = i;
                myButton.Click += new RoutedEventHandler(paginButto_Click); ;
                pagin.Children.Add(myButton);
            }
            turnButton();
        }
        private void turnButton()
        {
            if (start == 0) { back.IsEnabled = false; }
            else { back.IsEnabled = true; };
            if ((start + 1) * 10 > fullCount) { forward.IsEnabled = false; }
            else { forward.IsEnabled = true; };
        }



        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            if (agentGrid.SelectedItems.Count > 0)
            {
                int prt = 0;
                foreach (Agent agent in agentGrid.SelectedItems)
                {
                    if (agent.Priority > prt) prt = agent.Priority;
                }
                WindowModal dlg = new WindowModal(prt);
                helper.prioritet = prt;
                helper.flag = false;
                dlg.ShowDialog();
                if (helper.flag)
                {
                    foreach (Agent agent in agentGrid.SelectedItems)
                    {
                        agent.Priority = helper.prioritet;
                        helper.GetContext().Entry(agent).State = EntityState.Modified;
                    }
                    helper.GetContext().SaveChanges();
                    Load();
                }
            }
        }

    private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (agentGrid.SelectedItem != null)
            {
                Agent selectedAgent = (Agent)agentGrid.SelectedItem;
                // Получение контекста базы данных.
                var context = helper.GetContext();
                // Нахождение всех записе в таблице ProductSale, связанные с удаляемым агентом.
                var relatedSales = context.ProductSale.Where(sale => sale.AgentID == selectedAgent.ID).ToList();
                // Удаление найденной записи из таблицы ProductSale.
                context.ProductSale.RemoveRange(relatedSales);
                // Затем удаление записи из таблицы Agent.
                context.Agent.Remove(selectedAgent);
                // Сохрание изменения в базе данных.
                context.SaveChanges();
                MessageBox.Show("Удаление прошло успешно");
                Load();
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageTwo(null));
        }

        private void revButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void paginButto_Click(object sender, RoutedEventArgs e)
        {
            start = Convert.ToInt32(((Button)sender).Tag.ToString());
            Load();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            start--;
            Load();
        }

        private void forward_Click(object sender, RoutedEventArgs e)
        {
            start++;
            Load();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                string selectedValue = ((ComboBoxItem)comboBox.SelectedItem).Content.ToString();

                if (selectedValue == "Без сортировки")
                {
                    agentGrid.ItemsSource = helper.GetContext().Agent.OrderBy(Agent => Agent.ID).Skip(start * 10).Take(10).ToList();
                }
                else if (selectedValue == "По возрастанию")
                {
                    agentGrid.ItemsSource = helper.GetContext().Agent.OrderBy(agent => agent.Title).Skip(start * 10).Take(10).ToList();
                }
                else if (selectedValue == "По убыванию")
                {
                    agentGrid.ItemsSource = helper.GetContext().Agent.OrderByDescending(agent => agent.Title).Skip(start * 10).Take(10).ToList();
                }
                else if (selectedValue == "По возрастанию приоритета")
                {
                    agentGrid.ItemsSource = helper.GetContext().Agent.OrderBy(agent => agent.Priority).Skip(start * 10).Take(10).ToList();
                }
                else if (selectedValue == "По убыванию приоритета")
                {
                    agentGrid.ItemsSource = helper.GetContext().Agent.OrderByDescending(agent => agent.Priority).Skip(start * 10).Take(10).ToList();
                }
            }
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                string selectedValue = ((ComboBoxItem)comboBox.SelectedItem).Content.ToString();

                if (selectedValue == "---")
                {
                    agentGrid.ItemsSource = helper.GetContext().Agent.Where(agent => agent.AgentType.Title == "").ToList();
                }
                else if (selectedValue == "ЗАО")
                {
                    agentGrid.ItemsSource = helper.GetContext().Agent.Where(agent => agent.AgentType.Title == "ЗАО").ToList();
                }
                else if (selectedValue == "МКК")
                {
                    agentGrid.ItemsSource = helper.GetContext().Agent.Where(agent => agent.AgentType.Title == "МКК").ToList();
                }
                else if (selectedValue == "МФО")
                {
                    agentGrid.ItemsSource = helper.GetContext().Agent.Where(agent => agent.AgentType.Title == "МФО").ToList();
                }
                else if (selectedValue == "ОАО")
                {
                    agentGrid.ItemsSource = helper.GetContext().Agent.Where(agent => agent.AgentType.Title == "ОАО").ToList();
                }
                else if (selectedValue == "ООО")
                {
                    agentGrid.ItemsSource = helper.GetContext().Agent.Where(agent => agent.AgentType.Title == "ООО").ToList();
                }
                else if (selectedValue == "ПАО")
                {
                    agentGrid.ItemsSource = helper.GetContext().Agent.Where(agent => agent.AgentType.Title == "ПАО").ToList();
                }
            }
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = searchTextBox.Text.ToLower(); // Получаем текст из TextBox и преобразуем в нижний регистр для регистронезависимого поиска
            // Фильтруем данные и отображаем только записи, которые содержат введенный текст в названии
            agentGrid.ItemsSource = helper.GetContext().Agent
                .Where(agent => agent.Title.ToLower().Contains(searchText))
                .OrderBy(agent => agent.ID)
                .Skip(start * 10)
                .Take(10)
                .ToList();
        }
        private void agentGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (agentGrid.SelectedItems.Count > 0)
            {
                Agent agent = agentGrid.SelectedItems[0] as Agent;

                if (agent != null)
                {
                    PageTwo pageTwo = new PageTwo(agent);

                    this.NavigationService.Navigate(pageTwo);
                }
            }
        }

        private void agentGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            deleteButton.IsEnabled = agentGrid.SelectedItem != null;
        }

        private void agentGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            Agent agent = (Agent)e.Row.DataContext;
            int sum = 0;
            double fsum = 0;
            foreach (ProductSale ps in agent.ProductSale)
            {
                List<ProductMaterial> mtr = new List<ProductMaterial> { };
                mtr = helper.GetContext().ProductMaterial.Where(ProductMaterial => ProductMaterial.ProductID == ps.ProductID).ToList();
                foreach (ProductMaterial mt in mtr)
                {
                    double f = decimal.ToDouble(mt.Material.Cost);
                    fsum += f * (double)mt.Count;
                }
                fsum = fsum * ps.ProductCount;
                if (ps.SaleDate.AddDays(365).CompareTo(DateTime.Today) > 0)
                    sum += ps.ProductCount;
            }
            agent.Sale = sum;
            agent.Percent = 0;
            if (fsum >= 10000 && fsum < 50000) agent.Percent = 5;
            if (fsum >= 50000 && fsum < 150000) agent.Percent = 10;
            if (fsum >= 150000 && fsum < 500000) agent.Percent = 20;
            if (fsum >= 500000) agent.Percent = 25;
            if (agent.Percent == 25)
            {
                SolidColorBrush hb = new SolidColorBrush(Colors.LightGreen);
                e.Row.Background = hb;
            };

        }
    }
}