using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using OSP.Interface;
using OSP.Model;
using Action = OSP.Model.Action;
using RelayCommand = GalaSoft.MvvmLight.Command.RelayCommand;

namespace OSP.ViewModel
{
    public class ActionMenuViewModel : ViewModelBase
    {
        #region Fields

        private TypeOfWindow windowType;
        private string windowTitle;
        private readonly INavigationService navigationService;
        private ObservableCollection<Action> actions = new ObservableCollection<Action>();
        private Action selectedAction;
        private SQLiteConnection con = new SQLiteConnection(connectionString);
        private ObservableCollection<ActionType> actionTypes = new ObservableCollection<ActionType>();
        private User user;
        private Visibility visibility = Visibility.Hidden;

       
        private static string connectionString =
            @"Data Source=|DataDirectory|\OSPDatabase.db; Version=3";
        

        #endregion

        #region Properties

        public Visibility Visibility
        {
            get { return visibility;}
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

        public ObservableCollection<Action> Actions
        {
            get { return actions; }
            set
            {
                actions = value;
                RaisePropertyChanged();
            }
        }

        public Action SelectedAction
        {
            get { return selectedAction; }
            set
            {
                selectedAction = value;
                RaisePropertyChanged();
            }
        }



        #endregion

        #region ICommands

        public ICommand AddActionCommand { get; private set; }
        public ICommand EditActionCommand { get; private set; }
        public ICommand RemoveActionCommand { get; private set; }

        #endregion

        #region Constructors

        public ActionMenuViewModel(INavigationService navigationService)
        {
            RegisterMessages();

            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            AppDomain.CurrentDomain.SetData("DataDirectory", path);

            this.navigationService = navigationService;

            AddActionCommand = new RelayCommand(NewAction);
            EditActionCommand = new RelayCommand(EditAction);
            RemoveActionCommand = new RelayCommand(RemoveAction);


            windowType = TypeOfWindow.Actions;
            windowTitle = Helpers.GetDescription(windowType);

            GetActionTypes();
            using (con)
            {
                try
                {
                    con.Open();
                    LoadActions();
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd połączenia z bazą");
                }

            }

            selectedAction = Actions.ElementAt(0);
        }

        #endregion

        #region Methods

        private void LoadActions()
        {
            SQLiteCommand sql_cmd;
            sql_cmd = con.CreateCommand();
            sql_cmd.CommandText = "SELECT * FROM Akcja";
            sql_cmd.CommandType = CommandType.Text;
            SQLiteDataReader r = sql_cmd.ExecuteReader();
            ActionType actionType = new ActionType();
            while (r.Read())
            {
                int actionTypeId = Int32.Parse(r["IdTypuAkcji"].ToString());
                
                foreach (ActionType at in actionTypes)
                {
                    Debug.WriteLine(at.Id + " " + at.Type);
                    if (at.Id == actionTypeId)
                    {
                        actionType = at;
                    }
                }

                Actions.Add(new Action(Int32.Parse(r["IdAkcji"].ToString()), Int32.Parse(r["NumerMeldunku"].ToString()),
                    r["DataAkcji"].ToString(),
                    Int32.Parse(r["CzasTrwania"].ToString()), r["MiejsceAkcji"].ToString(),
                    Int32.Parse(r["LiczbaKilometrow"].ToString()), actionType));
                
            }
        }



        private void NewAction()
        {
            int id = 0;
            using (con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (
                        SQLiteCommand command = new SQLiteCommand("SELECT IdAkcji FROM Akcja ORDER BY IdAkcji DESC", con)
                    )
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        id = Int32.Parse(reader["IdAkcji"].ToString()) + 1;
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd połączenia z bazą: " + ex);
                }
            }
            Messenger.Default.Send(new Message(NotificationToken.AskCars));
            Messenger.Default.Send(new Message(NotificationToken.AskFirefighters));
            Messenger.Default.Send(new Message(NotificationToken.AskActionTypes));
            Messenger.Default.Send(new Message<Action>(NotificationToken.NewAction, new Action(id)));
            Messenger.Default.Send(new Message(NotificationToken.LoadAction));
            navigationService.NavigateToAction();
        }

        private void AddAction(Action action)
        {
            //Dodanie Akcji do ObservableCollection<Action>
            Actions.Add(action);
            using (con = new SQLiteConnection(connectionString)) //Stworzenie nowego połączenia SQL
            {
                con.Open(); //Otworzenie tego połączenia
                try
                {
                    using (SQLiteCommand insertSQL = con.CreateCommand()) //Utworzenie polecenie SQL
                    {
                        insertSQL.CommandText =
                            "INSERT INTO Akcja(IdAkcji, NumerMeldunku, DataAkcji, CzasTrwania, MiejsceAkcji, LiczbaKilometrow, IdTypuAkcji) " +
                            "VALUES (@id,@number,@date,@time,@place,@km,@idtype)"; //Dodanie tekstu naszego polecenia
                        //Dodanie parametrów naszej akcji do naszego polecenia SQL
                        insertSQL.Parameters.Add(new SQLiteParameter("@id", SqlDbType.Int) {Value = action.Id});
                        insertSQL.Parameters.Add(new SQLiteParameter("@number", SqlDbType.Int)
                        {
                            Value = action.NumberOfAction
                        });
                        insertSQL.Parameters.AddWithValue("@date", action.DateOfAction);
                        insertSQL.Parameters.Add(new SQLiteParameter("@time", SqlDbType.Int)
                        {
                            Value = action.MinutesOfAction
                        });
                        insertSQL.Parameters.AddWithValue("@place", action.PlaceOfAction);
                        insertSQL.Parameters.Add(new SQLiteParameter("@km", SqlDbType.Int) {Value = action.NumberOfKM});
                        insertSQL.Parameters.Add(new SQLiteParameter("@idtype", SqlDbType.Int)
                        {
                            Value = action.ActionType.Id
                        });
                        insertSQL.ExecuteNonQuery(); //Wywołanie polecenia SQL
                    }
                    con.Close(); //Zamknięcie połączenia 
                }
                catch (Exception ex) //Wyłapywanie wyjatków i błędów połączeń oraz zapytań SQL
                {
                    MessageBox.Show("Błąd połączenia z bazą: " + ex);
                }
            }
        }

        private void EditAction()
        {
            Messenger.Default.Send(new Message(NotificationToken.AskCars));
            Messenger.Default.Send(new Message(NotificationToken.AskFirefighters));
            Messenger.Default.Send(new Message(NotificationToken.AskActionTypes));
            Messenger.Default.Send(new Message<Action>(NotificationToken.EditAction, selectedAction));
            Messenger.Default.Send(new Message(NotificationToken.LoadAction));
            navigationService.NavigateToAction();
        }

        private void ChangeAction(Action action)
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                if (Actions[i].Id == action.Id)
                {
                    Actions[i] = action.Clone();
                }
            }
            CollectionViewSource.GetDefaultView(this.Actions).Refresh();


            using (con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SQLiteCommand command = new SQLiteCommand(con))
                    {
                        command.CommandText =
                            "UPDATE Akcja SET IdAkcji = @id, NumerMeldunku = @number, DataAkcji = @date, CzasTrwania = @time, MiejsceAkcji = @place, LiczbaKilometrow = @km, IdTypuAkcji = @idtype WHERE IdAkcji='" + action.Id.ToString() + "'";
                        command.Parameters.AddWithValue("id", action.Id);
                        command.Parameters.AddWithValue("number", action.NumberOfAction);
                        command.Parameters.AddWithValue("date", action.DateOfAction);
                        command.Parameters.AddWithValue("time", action.MinutesOfAction);
                        command.Parameters.AddWithValue("place", action.PlaceOfAction);
                        command.Parameters.Add(new SQLiteParameter("@km", SqlDbType.Int) { Value = action.NumberOfKM });
                        command.Parameters.Add(new SQLiteParameter("@idtype", SqlDbType.Int) { Value = action.ActionType.Id});
                        command.ExecuteNonQuery();
                    }
                    con.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd połączenia z bazą: " + ex);
                }
            }
        }

        private void RemoveAction()
        {

            using (con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    SQLiteCommand command = new SQLiteCommand(con);
                    command.CommandText = "DELETE FROM Akcja WHERE IdAkcji='" + SelectedAction.Id + "'";
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
            Actions.Remove(SelectedAction);
        }

        private void RegisterMessages()
        {
            
                Messenger.Default.Register<Message<Action>>(this, (message) =>
                {
                    if (message.Token == NotificationToken.ChangedAction)
                    {
                        ChangeAction(message.Content);
                    }
                    else if (message.Token == NotificationToken.AddAction)
                    {
                        AddAction(message.Content);
                    }
                });
                Messenger.Default.Register<Message<ObservableCollection<ActionType>>>(this, (message) =>
                {
                    if (message.Token == NotificationToken.SendActionTypes)
                    {
                        actionTypes = message.Content;
                    }
                });
            Messenger.Default.Register<Message>(this, (message) =>
            {
                if (message.Token==NotificationToken.FirefighterLogin)
                {
                    user = User.Firefighter;
                    visibility = Visibility.Hidden;
                }
                else if (message.Token == NotificationToken.CommanderLogin)
                {
                    user = User.Commander;
                    visibility = Visibility.Visible;
                }
                if (message.Token == NotificationToken.AskAction)
                {
                    Messenger.Default.Send(new Message<ObservableCollection<Action>>(NotificationToken.SendActions, actions));
                }
            });
            

            
        }

        private void GetActionTypes()
        {
            Messenger.Default.Send(new Message(NotificationToken.AskActionTypes));
        }
        #endregion


    }
}