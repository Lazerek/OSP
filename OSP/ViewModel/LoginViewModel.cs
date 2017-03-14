using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using OSP.Interface;
using OSP.Model;
using INavigationService = OSP.Interface.INavigationService;

namespace OSP.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        #region Fields

        private readonly Interface.INavigationService navigationService;
        private User user=User.Commander;
        private SQLiteConnection con = new SQLiteConnection(connectionString);
        private bool login = false;

        private static string connectionString =
             @"Data Source=C:\Users\Lazerek\Desktop\OSP\OSP\OSPDatabase.db; Version=3";
        #endregion

        #region Properties

        public string Login { get; set; }
        public string Password { get; set; }
        

        #endregion

        #region ICommand
        public ICommand LoginCommand { get; private set; }
        
        #endregion


        #region Constructors

        public LoginViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            LoginCommand = new RelayCommand<PasswordBox>(CheckLogin);
        }
        #endregion

        #region Methods

        private void LoginToApp()
        {
            if (user == User.Firefighter)
            {
                Messenger.Default.Send(new Message(NotificationToken.FirefighterLogin));
            }
            else if(user == User.Commander)
            {
                Messenger.Default.Send(new Message(NotificationToken.CommanderLogin));
            }
            navigationService.NavigatoToMain();

            
        }

       
        private void CheckLogin(PasswordBox password)
        {
            Password = password.Password.ToString();
            try
            {
                using (con = new SQLiteConnection(connectionString))
                {
                    con.Open();
                    SQLiteCommand sql_cmd;
                    sql_cmd = con.CreateCommand();
                    sql_cmd.CommandText = "SELECT * FROM Login";
                    sql_cmd.CommandType = CommandType.Text;
                    SQLiteDataReader r = sql_cmd.ExecuteReader();
                    while (r.Read())
                    {
                        if (r["Login"].ToString().Equals(Login) && r["Haslo"].ToString().Equals(Password))
                        {
                            login = true;
                            if (r["Typ"].ToString().Equals("Commander"))
                                user = User.Commander;
                            else if (r["typ"].ToString().Equals("Firefighter"))
                                user = User.Firefighter;
                            break;
                        }
                    }
                    r.Close();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd wczytwania bazy " + ex);
            }
            if (login)
            {
                LoginToApp();
                
            }
            else
            {
                MessageBox.Show("Błędne hasło lub/i login");
            }
        }
        #endregion
    }
}