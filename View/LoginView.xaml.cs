using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using WPF_Proyect_Food.ViewModel;

namespace WPF_Proyect_Food.View
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnMinimice_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OnLoginClick(object sender, RoutedEventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPassword.Password;

            if (IsValidUser(username, password))
            {
                new MainView().Show();

                txtLoginMessage.Text = "BIENVENIDO";

                Application.Current.MainWindow.Close();

            }
            else
            {
                txtLoginMessage.Text = "ERROR EN EL USUARIO O CONTRASEÑA";
            }
        }

        private bool IsValidUser(string username, string password)
        {
            string connectionString = "Server = localhost\\SQLEXPRESS;Database=JI_FOOD;Trusted_Connection=True;";
            string query = "SELECT * FROM [JI_FOOD].[dbo].[Users] WHERE Nick = @Username AND Password = @Password";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        int userCount = Convert.ToInt32(result);
                        return userCount > 0;
                    } else
                    {
                        return false;
                    }

                }
            }
            catch (SqlException ex) { return false; }
            catch (Exception ex) { return false; }


        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            var viewModel = (LoginViewModel)this.DataContext;

            if (viewModel != null)
            {
                viewModel.Password = passwordBox.Password;
            }
        }
    }
}
