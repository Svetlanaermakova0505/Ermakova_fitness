using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для WindowClients.xaml
    /// </summary>
    public partial class WindowClients : Window
    {
        Entities1 entities = new Entities1();
        public WindowClients()
        {
            InitializeComponent();
            foreach (var item in entities.Clients)
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

        private void lst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = lst.SelectedItem as Clients;

            if (selectedItem != null)
            {
                txt_имя.Text = selectedItem.name;
                txt_фам.Text = selectedItem.last_name;
                txt_отч.Text = selectedItem.fathers_name;

                txt_возр.Text = selectedItem.age.ToString();
                txt_пол.Text = selectedItem.pol;
                txt_адр.Text = selectedItem.adres;
                txt_тел.Text = selectedItem.phone;

                txt_рек.Text = selectedItem.recomendation;

            }
            else
            {
                txt_имя.Text = null;
                txt_фам.Text = null;
                txt_отч.Text = null;

                txt_возр.Text = null;
                txt_пол.Text = null;
                txt_адр.Text = null;
                txt_тел.Text = null;

                txt_рек.Text = null;
            }
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            lst.SelectedIndex = -1;

            txt_имя.Text = null;
            txt_фам.Text = null;
            txt_отч.Text = null;
            txt_возр.Text = null;
            txt_пол.Text = null;
            txt_адр.Text = null;
            txt_тел.Text = null;
            txt_рек.Text = null;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (classUser.user.role == "администратор" || classUser.user.role == "менеджер")
            {

            }
            else
            {
                MessageBox.Show("У вас нет прав!");
                return;
            }

            var item = lst.SelectedItem as Clients;
            if (
                txt_имя.Text == "" ||
                txt_фам.Text == "" ||
                txt_отч.Text == "" ||
                txt_возр.Text == "" ||
                txt_пол.Text == "" ||
                txt_адр.Text == "" ||
                txt_тел.Text == "" ||
                txt_рек.Text == ""
                )
            {
                MessageBox.Show("Заполните поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (item == null)
                {
                    item = new Clients();
                    entities.Clients.Add(item);
                    lst.Items.Add(item);
                }


                try
                {
                    item.name = txt_имя.Text;
                    item.last_name = txt_фам.Text;
                    item.fathers_name = txt_отч.Text;
                    item.age = int.Parse(txt_возр.Text);

                    if (txt_пол.Text == "муж"|| txt_пол.Text == "жен")
                        item.pol = txt_пол.Text;
                    else
                    {
                        MessageBox.Show("Ошибка! Пол только \"муж\" или \"жен\".");
                        //throw new Exception("Ошибка! Пол только \"муж\" или \"жен\".");
                    }

                    item.adres = txt_адр.Text;
                    item.phone = txt_тел.Text;
                    item.recomendation = txt_рек.Text;

                }
                catch
                {
                    MessageBox.Show("Некорректные данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);

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
                var deletedItem = lst.SelectedItem as Clients;
                if (deletedItem != null)
                {

                    try
                    {
                        entities.Clients.Remove(deletedItem);
                        entities.SaveChanges();

                        lst.Items.Remove(deletedItem);
                        lst.Items.Refresh();


                        lst.SelectedIndex = -1;

                        txt_имя.Text = null;
                        txt_фам.Text = null;
                        txt_отч.Text = null;
                        txt_возр.Text = null;
                        txt_пол.Text = null;
                        txt_адр.Text = null;
                        txt_тел.Text = null;
                        txt_рек.Text = null;

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
