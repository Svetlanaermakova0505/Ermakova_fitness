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
    /// Логика взаимодействия для WindowTreneri.xaml
    /// </summary>
    public partial class WindowTreneri : Window
    {
        Entities1 entities = new Entities1();
        public WindowTreneri()
        {
            InitializeComponent();
            foreach (var item in entities.Trainers)
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

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            lst.SelectedIndex = -1;

            txt_имя.Text = null;
            txt_фам.Text = null;
            txt_отч.Text = null;
            txt_опыт.Text = null;
            txt_адр.Text = null;
            txt_тел.Text = null;
        }

        private void lst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = lst.SelectedItem as Trainers;

            if (selectedItem != null)
            {
                txt_имя.Text = selectedItem.name;
                txt_фам.Text = selectedItem.last_name;
                txt_отч.Text = selectedItem.fathers_name;
                txt_опыт.Text = selectedItem.work_experience.ToString();
                txt_адр.Text = selectedItem.adres;
                txt_тел.Text = selectedItem.phone;

            }
            else
            {
                txt_имя.Text = null;
                txt_фам.Text = null;
                txt_отч.Text = null;
                txt_опыт.Text = null;
                txt_адр.Text = null;
                txt_тел.Text = null;
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (classUser.user.role == "администратор" )
            {

            }
            else
            {
                MessageBox.Show("У вас нет прав!");
                return;
            }

            var item = lst.SelectedItem as Trainers;
            if (
                txt_имя.Text == null ||
                txt_фам.Text == null ||
                txt_отч.Text == null ||
                txt_опыт.Text == null ||
                txt_адр.Text == null ||
                txt_тел.Text == null
                )
            {
                MessageBox.Show("Заполните поля", "Ошибка", MessageBoxButton.OK);
            }
            else
            {
                if (item == null)
                {
                    item = new Trainers();
                    entities.Trainers.Add(item);
                    lst.Items.Add(item);
                }

                try
                {
                    item.name = txt_имя.Text;
                    item.last_name = txt_фам.Text;
                    item.fathers_name = txt_отч.Text;
                    item.adres = txt_адр.Text;
                    item.phone = txt_тел.Text;
                    item.work_experience = double.Parse(txt_опыт.Text);
                }
                catch
                {
                    MessageBox.Show("Некорректные данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                entities.SaveChanges();
                lst.Items.Refresh();
                MessageBox.Show("Готово", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
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
                var deletedItem = lst.SelectedItem as Trainers;
                if (deletedItem != null)
                {

                    try
                    {
                        entities.Trainers.Remove(deletedItem);
                        entities.SaveChanges();

                        lst.Items.Remove(deletedItem);
                        lst.Items.Refresh();


                        lst.SelectedIndex = -1;

                        txt_имя.Text = null;
                        txt_фам.Text = null;
                        txt_отч.Text = null;
                        txt_опыт.Text = null;
                        txt_адр.Text = null;
                        txt_тел.Text = null;

                        MessageBox.Show("Удаление прошло успешно", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Эти данные используют другие таблицы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }


                }
            }
        }
    }
}
