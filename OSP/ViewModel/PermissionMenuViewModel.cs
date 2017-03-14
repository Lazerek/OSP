using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using OSP.Interface;
using OSP.Model;

namespace OSP.ViewModel
{
    public class PermissionMenuViewModel : ViewModelBase
    {
        #region Fields
        private TypeOfWindow windowType;
        private string windowTitle;
        private readonly INavigationService navigationService;
        private ObservableCollection<Permission> permissions = new ObservableCollection<Permission>();
        private Permission selectedPermission;
        private SQLiteConnection con = new SQLiteConnection(connectionString);
        private User user;
        private Visibility visibility = Visibility.Hidden;

        private static string connectionString =
                        @"Data Source=|DataDirectory|\OSPDatabase.db; Version=3";
        #endregion

        #region Properties
        public Visibility Visibility
        {
            get { return visibility; }
        }
        public TypeOfWindow WindowType
        {
            get { return windowType; }
            set
            {
                windowType = value;
                RaisePropertyChanged();
            }
        }

        public string WindowTitle
        {
            get { return windowTitle; }
            set
            {
                windowTitle = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Permission> Permissions
        {
            get { return permissions; }
            set
            {
                permissions = value;
                RaisePropertyChanged();
            }
        }

        public Permission SelectedPermission
        {
            get { return selectedPermission; }
            set
            {
                selectedPermission = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ICommands
        public ICommand AddPermissionCommand { get; private set; }
        public ICommand EditPermissionCommand { get; private set; }
        public ICommand RemovePermissionCommand { get; private set; }
        #endregion

        #region Constructors
        public PermissionMenuViewModel(INavigationService navigationService)
        {
            RegisterMessages();

            this.navigationService = navigationService;

            windowType = TypeOfWindow.Permissions;
            windowTitle = Helpers.GetDescription(windowType);

            AddPermissionCommand = new RelayCommand(NewPermission);
            EditPermissionCommand = new RelayCommand(EditPermission);
            RemovePermissionCommand = new RelayCommand(RemovePermission);

            using (con)
            {
                try
                {
                    con.Open();
                    LoadPermissions();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd połączenia z bazą");
                }
                finally
                {
                    con.Close();
                }
            }

            selectedPermission = Permissions.ElementAt(0);
        }
        #endregion

        #region Methods
        private void LoadPermissions()
        {
            //Stworzenie komendy SQLite
            SQLiteCommand sql_cmd;
            sql_cmd = con.CreateCommand();
            sql_cmd.CommandText = "SELECT * FROM Uprawnienia"; //Wpisanie treści komendy
            sql_cmd.CommandType = CommandType.Text;
            SQLiteDataReader r = sql_cmd.ExecuteReader(); //stworzenie i wywołanie Reader'a SQLite
            while (r.Read()) //Czytanie zawartości tabeli
            {
                Permissions.Add(new Permission(Int32.Parse(r["IdUprawnienia"].ToString()), r["NazwaUprawnienia"].ToString())); //Dodawanie uprawnień do kolekcji
            }
        }

        private void NewPermission()
        {
            int id = 0;
            using (con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SQLiteCommand command = new SQLiteCommand("SELECT IdUprawnienia FROM Uprawnienia ORDER BY IdUprawnienia DESC", con))
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        id = Int32.Parse(reader["IdUprawnienia"].ToString()) + 1;
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd połączenia z bazą: " + ex);
                }
            }

            Messenger.Default.Send(new Message<Permission>(NotificationToken.NewPermission, new Permission(id)));
            navigationService.NavigateToPermission();
           
        }

        private void AddPermission(Permission permission)
        {
            Permissions.Add(permission);
            using (con = new SQLiteConnection(connectionString))
            {
                con.Open();
                try
                {
                    using (SQLiteCommand insertSQL = con.CreateCommand())
                    {
                        insertSQL.CommandText = "INSERT INTO Uprawnienia(IdUprawnienia, NazwaUprawnienia) VALUES (@id,@name)";
                        insertSQL.Parameters.Add(new SQLiteParameter("@id", SqlDbType.Int) { Value = permission.Id });
                        insertSQL.Parameters.AddWithValue("@name", permission.Name);
                        insertSQL.ExecuteNonQuery();
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd połączenia z bazą: " + ex);
                }
            }
        }

        private void EditPermission()
        {
            Messenger.Default.Send(new Message<Permission>(NotificationToken.EditPermission, selectedPermission));
            navigationService.NavigateToPermission();
        }

        private void ChangePermission(Permission permission)
        {
            for (int i = 0; i < Permissions.Count; i++)
            {
                if (Permissions[i].Id == permission.Id)
                {
                    Permissions[i] = permission.Clone();
                }
            }
            CollectionViewSource.GetDefaultView(this.Permissions).Refresh();


            using (con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SQLiteCommand command = new SQLiteCommand(con))
                    {
                        command.CommandText =
                            "UPDATE Uprawnienia SET IdUprawnienia = @id, NazwaUprawnienia = @permissionName WHERE IdUprawnienia='" + permission.Id.ToString() + "'";
                        command.Parameters.AddWithValue("id", permission.Id);
                        command.Parameters.AddWithValue("permissionName", permission.Name);
                        command.ExecuteNonQuery();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd połączenia z bazą: " + ex);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void RemovePermission()
        {

            using (con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    SQLiteCommand command = new SQLiteCommand(con);
                    command.CommandText = "DELETE FROM Uprawnienia WHERE IdUprawnienia='" + SelectedPermission.Id + "'";
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd połączenia z bazą: " + ex);
                }
                finally
                {
                    con.Close();
                }
            }
            Permissions.Remove(selectedPermission);
        }

        private void RegisterMessages()
        {
            Messenger.Default.Register<Message<Permission>>(this, (message) =>
            {
                if (message.Token == NotificationToken.ChangedPermission)
                {
                    ChangePermission(message.Content);
                }
                else if (message.Token == NotificationToken.AddPermission)
                {
                    AddPermission(message.Content);
                }
            });
            Messenger.Default.Register<Message>(this, (message) =>
            {
                if (message.Token == NotificationToken.AskPermissions)
                {
                    SendPermissions();
                }
            });
            Messenger.Default.Register<Message>(this, (message) =>
            {
                if (message.Token == NotificationToken.FirefighterLogin)
                {
                    user = User.Firefighter;
                    visibility = Visibility.Hidden;
                }
                else if (message.Token == NotificationToken.CommanderLogin)
                {
                    user = User.Commander;
                    visibility = Visibility.Visible;
                }
            });
        }

        private void SendPermissions()
        {
            Messenger.Default.Send(new Message<ObservableCollection<Permission>>(NotificationToken.SendPermissions, permissions));
        }
        #endregion
    }
}