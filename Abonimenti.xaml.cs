using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Логика взаимодействия для Abonimenti.xaml
    /// </summary>
    public partial class Abonimenti : Window
    {
        Entities1 entities = new Entities1();
        public Abonimenti()
        {
            InitializeComponent();

            dateNow.Text = DateTime.Now.ToString();

            foreach (var item in entities.Season_tickets)
            {
                lst.Items.Add(item);
            }
            foreach (var item in entities.Clients)
            {
                cmbKlienti.Items.Add(item);
            }
            foreach (var item in entities.Trainers)
            {
                cmbTrener.Items.Add(item);
            }
            foreach (var item in entities.Types_season_tickets)
            {
                cmbTip.Items.Add(item);
            }
        }

        private void lst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = lst.SelectedItem as Season_tickets;
            if (selectedItem != null)
            {
                

                if (selectedItem.freeze_status == "+")
                {
                    CheckBox.IsChecked = true;
                }
                else
                {
                    CheckBox.IsChecked = false;
                }


                txtId.Text = selectedItem.Id.ToString();
                

                dateStart.Text = selectedItem.date_start.ToString();
                dateEnd.Text = selectedItem.date_end.ToString();

                cmbTip.SelectedItem = (from var in entities.Types_season_tickets
                                        where var.Id == selectedItem.id_ticket_type
                                        select var
                                        ).Single<Types_season_tickets>();

                cmbKlienti.SelectedItem = (from var in entities.Clients
                                          where var.Id == selectedItem.id_client
                                          select var
                                        ).Single<Clients>();

                cmbTrener.SelectedItem = (from var in entities.Trainers
                                         where var.Id == selectedItem.id_trainer
                                         select var
                                        ).Single<Trainers>();


                if (selectedItem.freeze_status == "+"  && cmbTip.SelectedValue.ToString() == "одноразовый")
                {
                    MessageBox.Show("Абонимент уже использовн!");
                    CheckBox.IsEnabled = false;
                    cmbTip.IsEnabled = false;
                }
                else
                {


                    CheckBox.IsEnabled = true;
                    cmbTip.IsEnabled = true;

                    if (selectedItem.date_end <= DateTime.Now)
                    {
                        MessageBox.Show("Абонемент просрочен!");
                        selectedItem.freeze_status = "+";
                        CheckBox.IsEnabled = false;
                        lst.Items.Refresh();
                    }


                }
            }
            else
            {
                cmbTip.SelectedIndex = -1;
                cmbKlienti.SelectedIndex = -1;
                cmbTrener.SelectedIndex = -1;
                txtId.Text = "";
                dateEnd.SelectedDate = null;
                dateStart.SelectedDate = null;
                CheckBox.IsChecked = false;
            }
        }

        private void enter_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = lst.SelectedItem as Season_tickets;

            if (CheckBox.IsChecked == true)
            {
                MessageBox.Show("Нельзя отметить! Абонемент приостановлен!");
                return;
            }

            var vhod = from i in entities.Season_tickets
                       where i.id_client == selectedItem.id_client  && i.inOut == "Вход"
                       select i;

            if (selectedItem == null)
            {
                MessageBox.Show("Не выбран абонимент!");
            }
            else if (selectedItem.inOut == "Выход" && vhod.Count() == 0)
            {
                
                selectedItem.inOut = "Вход";

                var visit = new Visits();

                DateTime date = DateTime.Now;

                visit.date = DateTime.Parse(date.ToShortDateString());
                visit.time = TimeSpan.Parse(date.ToShortTimeString());

                visit.inOutType = "Вход";

                visit.id_ticket = selectedItem.Id;

                entities.Visits.Add(visit);
                entities.SaveChanges();

                MessageBox.Show("Вход отмечен!");
            }
            else
            {
                MessageBox.Show("Вход уже осуществлен!");
            }

        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBox.IsChecked == true)
            {
                MessageBox.Show("Нельзя отметить! Абонемент приостановлен!");
                return;
            }

            var selectedItem = lst.SelectedItem as Season_tickets;

            if (selectedItem == null)
            {
                MessageBox.Show("Не выбран абонимент!");
            }
            else if (selectedItem.inOut != "Выход")
            {
                selectedItem.inOut = "Выход";

                var visit = new Visits();

                DateTime date = DateTime.Now;

                visit.date = DateTime.Parse(date.ToShortDateString());
                visit.time = TimeSpan.Parse(date.ToShortTimeString());

                visit.inOutType = "Выход";

                visit.id_ticket = selectedItem.Id;

                entities.Visits.Add(visit);
                entities.SaveChanges();

                MessageBox.Show("Выход отмечен!");
            }
            else
            {
                MessageBox.Show("Выход уже осуществлен!");
            }
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

            var item = lst.SelectedItem as Season_tickets;
            if (
                dateStart.SelectedDate == null ||
                dateEnd.SelectedDate == null ||
                cmbKlienti.SelectedIndex == -1 ||
                cmbTip.SelectedIndex == -1 ||
                cmbTrener.SelectedIndex == -1
                )
            {
                MessageBox.Show("Заполните поля", "Ошибка", MessageBoxButton.OK);
            }
            else
            {
                if (item == null)
                {
                    item = new Season_tickets();
                    entities.Season_tickets.Add(item);
                    lst.Items.Add(item);
                }

                try
                {
                    item.date_start = dateStart.SelectedDate.Value;
                    item.date_end = dateEnd.SelectedDate.Value;

                    //item.freeze_status = txtStatus.Text;
                    if (CheckBox.IsChecked == true)
                    {
                        item.freeze_status = "+" ;
                    }
                    else
                    {
                        item.freeze_status = "-";
                    }


                    item.id_client = (cmbKlienti.SelectedItem as Clients).Id;
                    item.id_ticket_type = (cmbTip.SelectedItem as Types_season_tickets).Id;
                    item.id_trainer = (cmbTrener.SelectedItem as Trainers).Id;
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

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            lst.SelectedIndex = -1;

            cmbTip.SelectedIndex = -1;
            cmbKlienti.SelectedIndex = -1;
            cmbTrener.SelectedIndex = -1;
            txtId.Text = "";
            CheckBox.IsChecked = false;
            dateEnd.SelectedDate = null;
            dateStart.SelectedDate = null;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            lst.Items.Clear();
            if (txt_bx_poisk.Text != "")
            {
                var FIO = (from cl in entities.Clients
                           where
                           cl.last_name.ToLower().Contains(txt_bx_poisk.Text.ToLower())
                           select cl.Id
                           );


                foreach (var i in entities.Season_tickets)
                {
                    if(
                        i.Id.ToString().Contains(txt_bx_poisk.Text)
                        ||
                        i.freeze_status.Contains(txt_bx_poisk.Text)
                        ||
                        FIO.Contains<int>((int)i.id_client)
                       )
                        lst.Items.Add( i );
                }

                if (lst.Items.Count == 0)
                {
                    MessageBox.Show("Совпадений не найдено!");
                }

            }
            else
            {
                foreach (var row in entities.Season_tickets)
                {
                    lst.Items.Add(row);
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lst.Items.Clear();
            
            string cmb = cmbFilter.SelectedItem.ToString();
            int index = cmb.IndexOf(" ");
            cmb = cmb.Remove(0, index+1);

            if (cmb == "Все")
            {
                foreach (var row in entities.Season_tickets)
                {
                    lst.Items.Add(row);
                }
            }

            if (cmb == "Одноразовые аб.")
            {
                var type = (
                        from i in entities.Types_season_tickets
                        where i.time_type == "одноразовый"
                        select i
                        ).First<Types_season_tickets>();

                foreach (var row in entities.Season_tickets)
                {
                    if (row.id_ticket_type == type.Id)
                        lst.Items.Add(row);
                }
            }

            if (cmb == "Детские аб.")
            {
                var type = (
                        from i in entities.Types_season_tickets
                        where i.type == "детский"
                        select i
                        ).First<Types_season_tickets>();

                foreach (var row in entities.Season_tickets)
                {
                    if (row.id_ticket_type == type.Id)
                        lst.Items.Add(row);
                }
            }

            if (cmb == "Приостановленные")
            {
                foreach (var row in entities.Season_tickets)
                {
                    if (row.freeze_status == "+")
                        lst.Items.Add(row);
                }
            }

            if (cmb == "Просроченные")
            {
                foreach (var row in entities.Season_tickets)
                {
                    if (row.date_end <= DateTime.Now)
                        lst.Items.Add(row);
                }
            }


        }

        private void clientsBtn_Click(object sender, RoutedEventArgs e)
        {
            var window = new WindowClients();
            this.Close();
            window.ShowDialog();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if(classUser.user.role != "администратор")
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
                var deletedItem = lst.SelectedItem as Season_tickets;
                if (deletedItem != null)
                {

                    try
                    {
                        entities.Season_tickets.Remove(deletedItem);
                        entities.SaveChanges();

                        lst.Items.Remove(deletedItem);
                        lst.Items.Refresh();

                        lst.SelectedIndex = -1;

                        cmbTip.SelectedIndex = -1;
                        cmbKlienti.SelectedIndex = -1;
                        cmbTrener.SelectedIndex = -1;
                        txtId.Text = "";
                        CheckBox.IsChecked = false;
                        dateEnd.SelectedDate = null;
                        dateStart.SelectedDate = null;

                        MessageBox.Show("Удаление прошло успешно", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Эти данные используют другие таблицы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }


                }
            }
        }

        private void stistika_Click(object sender, RoutedEventArgs e)
        {
            var window = new Statistika();
            this.Close();
            window.ShowDialog();
        }

        private void avtorizacia_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            this.Close();
            window.ShowDialog();
        }

        private void treneri_Click(object sender, RoutedEventArgs e)
        {
            var window = new WindowTreneri();
            this.Close();
            window.ShowDialog();
        }

        private void tipiBtn_Click(object sender, RoutedEventArgs e)
        {
            var window = new tipAbonementa();
            this.Close();
            window.ShowDialog();
        }

        private void registracia_Click(object sender, RoutedEventArgs e)
        {
            var window = new registracia();
            this.Close();
            window.ShowDialog();
        }
    }
}
