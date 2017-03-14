using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
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
    public class ActionTypeMenuViewModel : ViewModelBase
    {
        #region Fields
        private TypeOfWindow windowType;
        private string windowTitle;
        private readonly INavigationService navigationService;
        private ObservableCollection<ActionType> actionTypes = new ObservableCollection<ActionType>();
        private ActionType selectedActionType;
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

        public ObservableCollection<ActionType> ActionTypes
        {
            get { return actionTypes; }
            set
            {
                actionTypes = value;
                RaisePropertyChanged();
            }
        }

        public ActionType SelectedActionType
        {
            get { return selectedActionType; }
            set
            {
                selectedActionType = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ICommands
        public ICommand AddActionTypeCommand { get; private set; }
        public ICommand EditActionTypeCommand { get; private set; }
        public ICommand RemoveActionTypeCommand { get; private set; }
        #endregion

        #region Constructors

        public ActionTypeMenuViewModel(INavigationService navigationService)
        {
            RegisterMessages();

            this.navigationService = navigationService;

            windowType = TypeOfWindow.ActionTypes;
            windowTitle = Helpers.GetDescription(windowType);

            AddActionTypeCommand = new RelayCommand(NewActionType);
            EditActionTypeCommand = new RelayCommand(EditActionType);
            RemoveActionTypeCommand = new RelayCommand(RemoveActionType);

            using (con)
            {
                try
                {
                    con.Open();
                    LoadActionTypes();
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

            selectedActionType = ActionTypes.ElementAt(0);
        }

        #endregion

        #region Methods
        private void LoadActionTypes()
        {

            SQLiteCommand sql_cmd;
            sql_cmd = con.CreateCommand();
            sql_cmd.CommandText = "SELECT * FROM TypAkcji";
            sql_cmd.CommandType = CommandType.Text;
            SQLiteDataReader r = sql_cmd.ExecuteReader();
            while (r.Read())
            {
                ActionTypes.Add(new ActionType(Int32.Parse(r["IdTypuAkcji"].ToString()), r["NazwaTypuAkcji"].ToString()));
            }
        }
        private void NewActionType()
        {
            int id = 0;
            using (con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SQLiteCommand command = new SQLiteCommand("SELECT IdTypuAkcji FROM TypAkcji ORDER BY IdTypuAkcji DESC", con))
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        id = Int32.Parse(reader["IdTypuAkcji"].ToString())+1;
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd połączenia z bazą: " + ex);
                }
            }
            Messenger.Default.Send(new Message<ActionType>(NotificationToken.NewActionType, new ActionType(id)));
            navigationService.NavigateToActionType();
        }
        private void AddActionType(ActionType actionType)
        {
            ActionTypes.Add(actionType);
            using (con = new SQLiteConnection(connectionString))
            {
                con.Open();
                try
                {
                    using (SQLiteCommand insertSQL = con.CreateCommand())
                    {
                        insertSQL.CommandText = "INSERT INTO TypAkcji(IdTypuAkcji, NazwaTypuAkcji) VALUES (@id,@name)";
                        insertSQL.Parameters.Add(new SQLiteParameter("@id", SqlDbType.Int) { Value = actionType.Id });
                        insertSQL.Parameters.AddWithValue("@name", actionType.Type);
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
        private void EditActionType()
        {
            Messenger.Default.Send(new Message<ActionType>(NotificationToken.EditActionType, selectedActionType));
            navigationService.NavigateToActionType();
        }

        private void ChangeActionType(ActionType actionType)
        {
            for (int i = 0; i < ActionTypes.Count; i++)
            {
                if (ActionTypes[i].Id == actionType.Id)
                {
                    ActionTypes[i] = actionType.Clone();
                }
            }
            CollectionViewSource.GetDefaultView(this.ActionTypes).Refresh();


            using (con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SQLiteCommand command = new SQLiteCommand(con))
                    {
                        command.CommandText =
                            "UPDATE TypAkcji SET IdTypuAkcji = @id, NazwaTypuAkcji = @actionType WHERE IdTypuAkcji='" + actionType.Id.ToString() + "'";
                        command.Parameters.AddWithValue("id", actionType.Id);
                        command.Parameters.AddWithValue("actionType", actionType.Type);
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

        private void RemoveActionType()
        {
            //Stworzenie nowego połączenia SQL
            using (con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open(); //otworzenie połączenia SQL
                    SQLiteCommand command = new SQLiteCommand(con); //Stworzenie komendy SQL
                    command.CommandText = "DELETE FROM TypAkcji WHERE IdTypuAkcji='" + selectedActionType.Id + "'"; //Wpisanie odpowiedniej treści polecenia, aby usunąć odpowiedni typ akcji
                    command.ExecuteNonQuery(); //Wykonanie polecenia
                }
                catch (Exception ex) //Łapanie wyjątków
                {
                    MessageBox.Show("Błąd połączenia z bazą: " + ex);
                }
                finally //Zamknięcie bazy po wykonaniu operacji
                {
                    con.Close();
                }
            }
            ActionTypes.Remove(selectedActionType); //Usunięcie typu akcji z kolekcji
        }

        private void RegisterMessages()
        {
            Messenger.Default.Register<Message<ActionType>>(this, (message) =>
            {
                if (message.Token == NotificationToken.ChangedActionType)
                {
                    ChangeActionType(message.Content);
                }
                else if (message.Token == NotificationToken.AddActionType)
                {
                    AddActionType(message.Content);
                }
            });
            Messenger.Default.Register<Message>(this, (message) =>
            {
                if (message.Token == NotificationToken.AskActionTypes)
                {
                    SendActionTypes();
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

        private void SendActionTypes()
        {
            Messenger.Default.Send(new Message<ObservableCollection<ActionType>>(NotificationToken.SendActionTypes, actionTypes));
        }

        #endregion
    }
}