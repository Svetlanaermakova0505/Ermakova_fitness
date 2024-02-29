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

namespace Ermakova
{
    /// <summary>
    /// Логика взаимодействия для tipAbonementa.xaml
    /// </summary>
    public partial class tipAbonementa : Window
    {
        Entities1 entities = new Entities1();
        public tipAbonementa()
        {
            InitializeComponent();
            foreach (var item in entities.Types_season_tickets)
            {
                lst.Items.Add(item);
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            var window = new Abonimenti();
            this.Close();
            window.ShowDialog();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (classUser.user.role == "администратор")
            {

            }
            else
            {
                MessageBox.Show("У вас нет прав!");
                return;
            }

            var item = lst.SelectedItem as Types_season_tickets;
            if (
                txt_возраст.Text == "" ||
                txt_срок.Text == "" ||
                txt_тип.Text == "" ||
                txt_цена.Text == "" 
                )
            {
                MessageBox.Show("Заполните поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (item == null)
                {
                    item = new Types_season_tickets();
                    entities.Types_season_tickets.Add(item);
                    lst.Items.Add(item);
                }

                try
                {
                    item.type = txt_тип.Text;
                    item.age_group = txt_возраст.Text;
                    item.time_type = txt_срок.Text;
                    item.price = decimal.Parse(txt_цена.Text);

                    entities.SaveChanges();
                    lst.Items.Refresh();
                    MessageBox.Show("Готово", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                    MessageBox.Show("Некорректные данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);

                }

                
            }
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            txt_тип.Text = string.Empty;
            txt_возраст.Text = string.Empty;
            txt_срок.Text = string.Empty;
            txt_цена.Text = string.Empty;

            lst.SelectedIndex = -1;

        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (classUser.user.role == "администратор" )
            {

            }
            else
            {
                MessageBox.Show("У вас нет прав!");
                return;
            }

            var resalt = MessageBox.Show("Удалить?", "Удаление", MessageBoxButton.YesNo);
            if (resalt == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                var deletedItem = lst.SelectedItem as Types_season_tickets;
                if (deletedItem != null)
                {

                    try
                    {
                        entities.Types_season_tickets.Remove(deletedItem);
                        entities.SaveChanges();

                        lst.Items.Remove(deletedItem);
                        lst.Items.Refresh();

                        txt_тип.Text = string.Empty;
                        txt_возраст.Text = string.Empty;
                        txt_срок.Text = string.Empty;
                        txt_цена.Text = string.Empty;

                        lst.SelectedIndex = -1;


                        MessageBox.Show("Удаление прошло успешно", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Эти данные используют другие таблицы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }


                }
            }
        }

        private void lst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = lst.SelectedItem as Types_season_tickets;

            if (selectedItem != null)
            {
                txt_тип.Text = selectedItem.type;
                txt_возраст.Text = selectedItem.age_group;
                txt_срок.Text = selectedItem.time_type;
                txt_цена.Text = selectedItem.price.ToString();

            }
            else
            {
                txt_тип.Text = string.Empty;
                txt_возраст.Text = string.Empty;
                txt_срок.Text = string.Empty;
                txt_цена.Text = string.Empty;
            }
        }
    }
}
