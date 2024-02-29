using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common.CommandTrees.ExpressionBuilder;
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
using Exel = Microsoft.Office.Interop.Excel;

namespace Ermakova
{
    /// <summary>
    /// Логика взаимодействия для Statistika.xaml
    /// </summary>
    public partial class Statistika : Window
    {
        Entities1 entities = new Entities1();
        public Statistika()
        {
            InitializeComponent();

            foreach (var item in entities.Visits)
            {
                lst.Items.Add(item);
            }

            foreach (var item in entities.Season_tickets)
            {
                cmb_id_tick.Items.Add(item.Id);
            }

            cmb_in_out.Items.Add("Вход");
            cmb_in_out.Items.Add("Выход");

            //s

            var kolvo = from i in entities.Season_tickets
                        where i.inOut == "Вход" 
                        select i;
            txtKolvo.Text = kolvo.Count().ToString();

            foreach (var item in entities.Types_season_tickets)
            {
                cmbTypeTick.Items.Add(item);
            }

            cmbTime.Items.Add("За сегодня");
            cmbTime.Items.Add("За весь период");

            kolvoVisitsAll.Text = lst.Items.Count.ToString();
        }

        private void lst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = lst.SelectedItem as Visits;
            if (selectedItem != null)
            {

                dateV.Text = selectedItem.date.ToString();
                timeV.Text = selectedItem.time.ToString();

                cmb_id_tick.SelectedItem = selectedItem.id_ticket;
                cmb_in_out.SelectedItem = selectedItem.inOutType;
                                               
            }
            else
            {
                dateV.Text = null;
                timeV.Text = null;
                cmb_id_tick.SelectedItem = null;
                cmb_in_out.SelectedItem = null;
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Abonimenti window = new Abonimenti();
            this.Close();
            window.ShowDialog();
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            lst.SelectedItem = null;
            dateV.Text = null;
            timeV.Text = null;
            cmb_id_tick.SelectedItem = null;
            cmb_in_out.SelectedItem = null;
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

            var item = lst.SelectedItem as Visits;
            if (
                dateV.Text == "" ||
                timeV.Text == "" ||
                cmb_id_tick.SelectedItem == null ||
                cmb_in_out.SelectedItem == null
                )
            {
                MessageBox.Show("Заполните поля", "Ошибка", MessageBoxButton.OK);
            }
            else
            {
                if (item == null)
                {
                    item = new Visits();
                    entities.Visits.Add(item);
                    lst.Items.Add(item);
                }

                try
                {
                    item.date = dateV.SelectedDate.Value;
                    item.time = TimeSpan.Parse(timeV.Text);
                    item.id_ticket = int.Parse(cmb_id_tick.SelectionBoxItem.ToString());

                    string s = cmb_in_out.SelectionBoxItem.ToString();
                    item.inOutType = s;

                }
                catch
                {
                    MessageBox.Show("Некорректные данные!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);

                }

                entities.SaveChanges();
                lst.Items.Refresh();
                kolvoVisitsAll.Text = lst.Items.Count.ToString();
                MessageBox.Show("Готово", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (classUser.user.role == "администратор")
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
                var deletedItem = lst.SelectedItem as Visits;
                if (deletedItem != null)
                {
                    try
                    {
                        entities.Visits.Remove(deletedItem);
                        entities.SaveChanges();

                        lst.Items.Remove(deletedItem);
                        lst.Items.Refresh();

                        lst.SelectedIndex = -1;
                        dateV.Text = null;
                        timeV.Text = null;
                        cmb_id_tick.SelectedItem = null;
                        cmb_in_out.SelectedItem = null;

                        kolvoVisitsAll.Text = lst.Items.Count.ToString();

                        MessageBox.Show("Удаление прошло успешно", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Эти данные используют другие таблицы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

        
        private void statistic_Click(object sender, RoutedEventArgs e)
        {

            lst.Items.Clear();

            string T = "";
            int typeTk = 0;

            if (cmbTime.SelectedItem != null)
            {
                T = cmbTime.SelectionBoxItem.ToString();
            }
            else
            {
                T = "";
            }

            if (cmbTypeTick.SelectedItem != null)
            {
                typeTk = (cmbTypeTick.SelectedItem as Types_season_tickets).Id;
            }
            else
            {
                typeTk = 0;
            }


            var spisokType = from i in entities.Season_tickets
                             where i.id_ticket_type == typeTk
                             select i;


            if (T == "За сегодня")
            {
                
                var dateNow = DateTime.Now.ToShortDateString();

                if (typeTk == 0)
                {
                    foreach (var item in entities.Visits)
                    {
                        if (item.date.ToString().Contains(dateNow))
                        {
                            lst.Items.Add(item);
                        }
                    }
                }
                else
                {
                    foreach (var item in entities.Visits)
                    {
                        if (item.date.ToString().Contains(dateNow))
                        {
                            foreach (var item2 in spisokType)
                            {
                                if (item2.Id == item.id_ticket)
                                {
                                    lst.Items.Add(item);
                                }
                            }
                        }
                    }
                }
            }
            else
            {

                if (typeTk == 0)
                {
                    foreach (var item in entities.Visits)
                    {
                        lst.Items.Add(item);
                    }
                }
                else
                {
                    foreach (var item in entities.Visits)
                    {
                        foreach (var item2 in spisokType)
                        {
                            if (item2.Id == item.id_ticket)
                            {
                                lst.Items.Add(item);
                            }
                        }
                    }

                }

            }

            kolvoVisits.Text = lst.Items.Count.ToString();
        }

        private void sbros_Click(object sender, RoutedEventArgs e)
        {
            lst.Items.Clear();
            kolvoVisits.Text = null;
            cmbTime.SelectedItem = null;
            cmbTypeTick.SelectedItem = null;
            foreach (var item in entities.Visits)
            {
                lst.Items.Add(item);
            }
        }

        private void export_btn_Click(object sender, RoutedEventArgs e)
        {
            if (classUser.user.role == "администратор" || classUser.user.role == "менеджер")
            {

            }
            else
            {
                MessageBox.Show("У вас нет прав!");
                return;
            }

            string[,] items_data = new string[lst.Items.Count, 5];

            for (int i = 0; i < lst.Items.Count; i++)
            {
                string srtoka = lst.Items[i].ToString();
                srtoka = srtoka.Replace("Запись №", "");
                srtoka = srtoka.Replace("-", " ");
                var stroka_ = srtoka.Split(' ');
                
                for (int j = 0; j < stroka_.Length; j++)
                {
                    items_data[i, j] = stroka_[j];
                }
                

            }

            Exel.Application exApp = new Exel.Application();
            exApp.Workbooks.Add();
            Exel.Worksheet wsh = (Exel.Worksheet)exApp.ActiveSheet;

            wsh.Cells[1, 1] = $"Запись №";
            wsh.Cells[1, 2] = $"id абонемента";
            wsh.Cells[1, 3] = $"Вход/выход";
            wsh.Cells[1, 4] = $"Дата";
            wsh.Cells[1, 5] = $"Время";

            int maxrow = lst.Items.Count;
            int maxcol = 5;

            for (int row = 0; row < maxrow; row++)
            {
                for (int col = 0; col < maxcol; col++)
                {
                    wsh.Cells[row + 2, col + 1] = $"{items_data[row, col]}";
                }
            }

            exApp.Visible = true;
        }
    }
}
