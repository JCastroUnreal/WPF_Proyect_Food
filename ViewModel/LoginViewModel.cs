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

        // Interfaz implementada
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Propiedades de enlace
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));

                // Forzar la reevaluación del CanExecute
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));

                // Forzar la reevaluación del CanExecute
                LoginCommand.RaiseCanExecuteChanged();
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

        // Comandos
        public RelayCommand LoginCommand { get; }

        public LoginViewModel()
        {
            // Inicializar el comando
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
        }

        // Verifica si el login puede ejecutarse
        private bool CanExecuteLogin(object parameter)
        {
            // Solo permitir login si el usuario y la contraseña no están vacíos
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        // Ejecutar el login
        private void ExecuteLogin(object parameter)
        {
            if (IsValidUser(Username, Password))
            {
                // Si el login es correcto ejecutamos esta parte
                new MainView().Show();
                Application.Current.MainWindow.Close();

                LoginMessage = string.Empty;
            } else
            {
                // Si el login es incorrecto ejecutamos esta parte
                LoginMessage = "Invalid username or password";
            }
        }

        private bool IsValidUser(string username, string password)
        {
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
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (SqlException ex) { return false; }
            catch (Exception ex) { return false; }
        }
    }
}
