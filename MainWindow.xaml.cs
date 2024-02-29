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

namespace Ermakova
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Entities1 entities = new Entities1();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_enter_Click(object sender, RoutedEventArgs e)
        {
            string login = login_box.Text.Trim();
            string password = password_box.Password.Trim();

            Users user = new Users();
            try
            {
                user = entities.Users.Where(p => p.login == login && p.password == password).First();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            int userCount = entities.Users.Where(p => p.login == login && p.password == password).Count();

            if (userCount > 0)
            {
                MessageBox.Show("Вы подлкючились как " + user.role);

                classUser.user = user;

                Abonimenti window = new Abonimenti();
                this.Close();
                window.ShowDialog();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль, попробуйте еще раз.");
            }
        }

        private void Button_registration_Click(object sender, RoutedEventArgs e)
        {
            var window = new registracia();
            this.Close();
            window.ShowDialog();
        }

        private void Button_gost_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("Вы подлкючились как гость.");


            var window = new Abonimenti();
            this.Close();
            window.ShowDialog();
        }
    }
}
