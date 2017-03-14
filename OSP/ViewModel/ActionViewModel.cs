using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using OSP.Model;
using Action = OSP.Model.Action;

namespace OSP.ViewModel
{
    /// <summary>
    /// Klasa obsługująca okno akcji
    /// </summary>
    public class ActionViewModel : ViewModelBase
    {
        #region Fields
        private Action action;
        private TypeOfWindow windowType;
        private string windowTitle;
        private string buttonText;
        private ActionType actionType;
        private SQLiteConnection con = new SQLiteConnection(connectionString);
        private List<ListModel> firefighterList = new List<ListModel>();
        private List<ListModel> carList = new List<ListModel>();
        private ObservableCollection<Firefighter> firefighters;
        private ObservableCollection<Car> cars;
        private List<int> doneFirefighter = new List<int>();
        private List<int> doneCar = new List<int>();
        private ObservableCollection<ActionType> actionTypes;

        private static string connectionString =
            @"Data Source=C:\Users\Lazerek\Desktop\OSP\OSP\OSPDatabase.db; Version=3";
        #endregion

        #region Properties
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

        public Action Action
        {
            get { return action; }
            set
            {
                action = value;
                RaisePropertyChanged();
            }
        }

        public string ButtonText
        {
            get { return buttonText; }
            set
            {
                buttonText = value;
                RaisePropertyChanged();
            }
        }

        public List<ListModel> CarList
        {
            get { return carList; }
            set
            {
                carList = value;
                RaisePropertyChanged();
            }
        }

        public List<ListModel> FirefighterList
        {
            get { return firefighterList; }
            set
            {
                firefighterList = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<ActionType> ActionTypes
        {
            get { return actionTypes; }
        }

        public ActionType ActionType
        {
            get { return actionType; }
            set
            {
                actionType = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region ICommands

        public ICommand SaveActionCommand { get; private set; }

        #endregion

        #region Constructors
        public ActionViewModel()
        {
            SaveActionCommand = new RelayCommand(SaveAction);

            RegisterMessages();
        }
        #endregion

        #region Methods
        private void CreateCheckLists()
        {
            carList = new List<ListModel>();
            firefighterList = new List<ListModel>();
            foreach (var car in cars)
            {
                carList.Add(new ListModel(car.Id, car.CarNumber));
            }
            foreach (var firefighter in firefighters)
            {
                firefighterList.Add(new ListModel(firefighter.Id, firefighter.FirstName  + " " + firefighter.LastName));
            }
            LoadCars();
            LoadFirefighters();
        }

        private void LoadCars()
        {
            doneCar = new List<int>();
            using (con = new SQLiteConnection(connectionString))
            {
                try
                {
                    string cmdString = "SELECT * FROM WozyNaAkcjach WHERE IdWozu=" + Action.Id.ToString();
                    con.Open();
                    using (SQLiteCommand command = new SQLiteCommand(cmdString, con))
                    {

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader.HasRows)
                                    doneCar.Add(Int32.Parse(reader["IdWozu"].ToString()));
                            }
                        }
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd połączenia z bazą");
                }

            }
            foreach (var id in doneCar)
            {
                foreach (var car in carList)
                {
                    if (car.Id == id)
                    {
                        car.SetSelect();
                    }
                }
            }
        }

        private void SaveCars()
        {
            List<int> toAdd = new List<int>();
            List<int> toRemove = new List<int>();
            foreach (var car in carList)
            {
                if (car.IsSelected)
                {
                    if (!doneCar.Contains(car.Id))
                    {
                        toAdd.Add(car.Id);
                    }
}
                else
                {
                    if (doneCar.Contains(car.Id))
                    {
                        toRemove.Add(car.Id);
                    }
                }
            }
            if (toAdd.Count > 0)
            {
                foreach (var id in toAdd)
                {
                    using (con = new SQLiteConnection(connectionString))
                    {
                        con.Open();
                        try
                        {
                            using (SQLiteCommand insertSQL = con.CreateCommand())
                            {
                                insertSQL.CommandText = "INSERT INTO WozyNaAkcjach(IdAkcji, IdWozu) VALUES (@idK,@idF)";
                                insertSQL.Parameters.Add(new SQLiteParameter("@idK", SqlDbType.Int) { Value = action.Id });
                                insertSQL.Parameters.Add(new SQLiteParameter("@idF", SqlDbType.Int) { Value = id });
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
            }

            if (toRemove.Count > 0)
            {
                foreach (var id in toRemove)
                {
                    using (con = new SQLiteConnection(connectionString))
                    {
                        try
                        {
                            con.Open();
                            string cmdString = "DELETE FROM WozyNaAkcjach WHERE IdAkcji='" + action.Id + "' AND IdWozu='" + id + "'";
                            using (SQLiteCommand command = new SQLiteCommand(cmdString, con))
                            {
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
            }

        }

        private void LoadFirefighters()
        {
            doneFirefighter = new List<int>();
            using (con = new SQLiteConnection(connectionString))
            {
                try
                {
                    string cmdString = "SELECT * FROM Uczestnicy WHERE IdAkcji=" + Action.Id.ToString();
                    con.Open();
                    using (SQLiteCommand command = new SQLiteCommand(cmdString, con))
                    {

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader.HasRows)
                                    doneFirefighter.Add(Int32.Parse(reader["IdStraz"].ToString()));
                            }
                        }
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd połączenia z bazą");
                }

            }
            foreach (var id in doneFirefighter)
            {
                foreach (var firefighter in firefighterList)
                {
                    if (firefighter.Id == id)
                    {
                        firefighter.SetSelect();
                    }
                }
            }
        }

        private void SaveFirefighters()
        {
            List<int> toAdd = new List<int>();
            List<int> toRemove = new List<int>();
            foreach (var firefighter in firefighterList)
            {
                if (firefighter.IsSelected)
                {
                    if (!doneFirefighter.Contains(firefighter.Id))
                    {
                        toAdd.Add(firefighter.Id);
                    }
                }
                else
                {
                    if (doneFirefighter.Contains(firefighter.Id))
                    {
                        toRemove.Add(firefighter.Id);
                    }
                }
            }
            if (toAdd.Count > 0)
            {
                foreach (var id in toAdd)
                {
                    using (con = new SQLiteConnection(connectionString))
                    {
                        con.Open();
                        try
                        {
                            using (SQLiteCommand insertSQL = con.CreateCommand())
                            {
                                insertSQL.CommandText = "INSERT INTO Uczestnicy(IdAkcji, IdStraz) VALUES (@idK,@idF)";
                                insertSQL.Parameters.Add(new SQLiteParameter("@idK", SqlDbType.Int) { Value = action.Id });
                                insertSQL.Parameters.Add(new SQLiteParameter("@idF", SqlDbType.Int) { Value = id });
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
            }

            if (toRemove.Count > 0)
            {
                foreach (var id in toRemove)
                {
                    using (con = new SQLiteConnection(connectionString))
                    {
                        try
                        {
                            con.Open();
                            string cmdString = "DELETE FROM Uczestnicy WHERE IdStraz='" + id + "' AND IdAkcji='" + action.Id + "'";
                            using (SQLiteCommand command = new SQLiteCommand(cmdString, con))
                            {
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
            }
        }
        private void RegisterMessages()
        {
            Messenger.Default.Register<Message<Action>>(this, (message) =>
            {
                Action = message.Content?.Clone() ?? new Action();
                if (message.Token == NotificationToken.EditAction)
                {
                    windowType = TypeOfWindow.EditAction;
                    windowTitle = Helpers.GetDescription(windowType);
                    buttonText = "Zmień";
                    Debug.WriteLine(Action.Id.ToString());
                    Debug.WriteLine(Action.ActionType.Type);
                    foreach (ActionType at in actionTypes)
                    {
                        Debug.WriteLine(at.Id.ToString());
                        Debug.WriteLine(at.Type);
                        if (Action.Id == at.Id)
                        {
                            ActionType = at;
                        }
                    }
                }
                else if (message.Token == NotificationToken.NewAction)
                {
                    windowType = TypeOfWindow.AddAction;
                    windowTitle = Helpers.GetDescription(windowType);
                    buttonText = "Dodaj";
                    ActionType = actionTypes.ElementAt(0);
                }
            });
            Messenger.Default.Register<Message<ObservableCollection<Car>>>(this, (message) =>
            {
                if (message.Token == NotificationToken.SendCars)
                {
                    cars = new ObservableCollection<Car>();
                    cars = message.Content;
                }

            });
            Messenger.Default.Register<Message<ObservableCollection<Firefighter>>>(this, (message) =>
            {
                if (message.Token == NotificationToken.SendFirefighters)
                {
                    firefighters = new ObservableCollection<Firefighter>();
                    firefighters = message.Content;
                }
            });
            Messenger.Default.Register<Message<ObservableCollection<ActionType>>>(this, (message) =>
            {
                if (message.Token == NotificationToken.SendActionTypes)
                {
                    actionTypes = new ObservableCollection<ActionType>();
                    actionTypes = message.Content;
                }
            });
            Messenger.Default.Register<Message>(this, (message) =>
            {
                if (message.Token == NotificationToken.LoadAction)
                {
                    CreateCheckLists();
                }
            });
        }

        private void SaveAction()
        {
            SaveCars();
            SaveFirefighters();
            action.ActionType = ActionType;
            if ((WindowType == TypeOfWindow.AddAction))
                Messenger.Default.Send(new Message<Action>(NotificationToken.AddAction, action));
            else
                Messenger.Default.Send(new Message<Action>(NotificationToken.ChangedAction, action));
        }
        #endregion
    }
}