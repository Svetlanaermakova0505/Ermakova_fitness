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
    /// Логика взаимодействия для registracia.xaml
    /// </summary>
    public partial class registracia : Window
    {
        public registracia()
        {
            InitializeComponent();
        }
        Entities1 entities = new Entities1();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ( classUser.user.role != "администратор" )
            {
                MessageBox.Show("У вас нет прав!");
                return;
            }

            try
            {
                if (loginTxt.Text == "" || passwordTxt.Text == "" || cmb.SelectedIndex == -1)
                {
                    MessageBox.Show("Заполните все поля!");
                }
                else
                {
                    Users user = new Users();

                    if (cmb.SelectedItem != null)
                    {
                        user.role = cmb.SelectionBoxItem.ToString();
                    }
                    user.login = loginTxt.Text;
                    user.password = passwordTxt.Text;

                    entities.Users.Add(user);
                    entities.SaveChanges();

                    MessageBox.Show("Успешно зарегистрирован " + cmb.SelectionBoxItem.ToString() + ": " + loginTxt.Text);
                }
            }
            catch
            {
                MessageBox.Show("Ошибка! Заполните все поля корректно!");
                return;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var window = new Abonimenti();
            this.Close();
            window.ShowDialog();
        }
    }
}
