using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_Proyect_Food.View;

namespace WPF_Proyect_Food.ViewModel
{
    class LoginViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _password;
        private string _loginMessage;
        private string connectionString = "Server = localhost\\SQLEXPRESS;Database=JI_FOOD;Trusted_Connection=True;";

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
                ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
            }
        }

        public string LoginMessage
        {
            get => _loginMessage;
            set
            {
                _loginMessage = value;
                OnPropertyChanged(nameof(LoginMessage));
            }
        }

        public ICommand LoginCommand { get; }


        public event PropertyChangedEventHandler? PropertyChanged;

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login, CanLogin);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool CanLogin(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        private void Login(object parameter)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Console.WriteLine("CONSULTA");
                connection.Open();
                string query = "SELECT * FROM [JI_FOOD].[dbo].[Users] WHERE Nick = @Username AND Password = @Password";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", _username);
                command.Parameters.AddWithValue("@Password", _password);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    // Cuando encontramos al usuario
                    while (reader.Read())
                    {
                        new MainView().Show();

                        LoginMessage = "BIENVENIDO";

                        Application.Current.MainWindow.Close();
                    }
                }
                else
                {
                    LoginMessage = "ERROR EN EL USUARIO O CONTRASEÑA";
                    // Cuando el login falla
                }
            }

        }
    }
}
