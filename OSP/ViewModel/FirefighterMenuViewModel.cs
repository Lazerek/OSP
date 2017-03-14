using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using OSP.Interface;
using OSP.Model;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace OSP.ViewModel
{
    public class FirefighterMenuViewModel : ViewModelBase
    {
        #region Fields

        private TypeOfWindow windowType;
        private string windowTitle;
        private readonly INavigationService navigationService;
        private ObservableCollection<Firefighter> firefighters = new ObservableCollection<Firefighter>();
        private Firefighter selectedFirefighter;
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

        public ObservableCollection<Firefighter> Firefighters
        {
            get { return firefighters; }
            set
            {
                firefighters = value;
                RaisePropertyChanged();
            }
        }

        public Firefighter SelectedFirefighter
        {
            get { return selectedFirefighter; }
            set
            {
                selectedFirefighter = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region ICommands

        public ICommand AddFirefighterCommand { get; private set; }
        public ICommand EditFirefighterCommand { get; private set; }
        public ICommand RemoveFirefigterCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        #endregion

        #region Constructors

        public FirefighterMenuViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            AddFirefighterCommand = new RelayCommand(NewFirefighter);
            EditFirefighterCommand = new RelayCommand(EditFirefighter);
            RemoveFirefigterCommand = new RelayCommand(RemoveFirefighter);
            CancelCommand = new RelayCommand(OnExit);


            windowType = TypeOfWindow.Firefighters;
            windowTitle = Helpers.GetDescription(windowType);

            RegisterMessages();

            using (con)
            {
                try
                {
                    con.Open();
                    LoadFirefighters();
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

            selectedFirefighter = firefighters.ElementAt(0);
        }
        #endregion

        #region Methods

        private void LoadFirefighters()
        {
            SQLiteCommand sql_cmd;
            sql_cmd = con.CreateCommand();
            sql_cmd.CommandText = "SELECT * FROM Strazacy";
            sql_cmd.CommandType = CommandType.Text;
            SQLiteDataReader r = sql_cmd.ExecuteReader();
            while (r.Read())
            {
                Firefighters.Add(new Firefighter(Int32.Parse(r["IdStraz"].ToString()), r["NazwiskoStraz"].ToString(), r["ImieStraz"].ToString(), r["PeselStraz"].ToString(), r["TelefonStraz"].ToString(), r["StopienStraz"].ToString(), r["DataUrodzenia"].ToString(), r["DataDolaczenia"].ToString()));
            }
        }
        private void NewFirefighter()
        {
            int id = 0;
            using (con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SQLiteCommand command = new SQLiteCommand("SELECT IdStraz FROM Strazacy ORDER BY IdStraz DESC", con))
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        id = Int32.Parse(reader["IdStraz"].ToString()) + 1;
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd połączenia z bazą: " + ex);
                }
            }
            Messenger.Default.Send(new Message(NotificationToken.AskPermissions));
            Messenger.Default.Send(new Message(NotificationToken.AskCourses));
            Messenger.Default.Send(new Message<Firefighter>(NotificationToken.NewFirefighter, new Firefighter(id)));
            Messenger.Default.Send(new Message(NotificationToken.LoadFirefighter));
            navigationService.NavigateToFirefighter();
        }
        private void AddFirefighter(Firefighter firefighter)
        {
            Firefighters.Add(firefighter);
            using (con = new SQLiteConnection(connectionString))
            {
                con.Open();
                try
                {
                    using (SQLiteCommand insertSQL = con.CreateCommand())
                    {
                        insertSQL.CommandText = "INSERT INTO Strazacy(IdStraz, NazwiskoStraz, ImieStraz, PeselStraz, TelefonStraz, StopienStraz, DataUrodzenia, DataDolaczenia) VALUES (@id,@lastname,@firstname,@pesel,@phone,@degree,@birthdate,@joindate)";
                        insertSQL.Parameters.Add(new SQLiteParameter("@id", SqlDbType.Int) { Value = firefighter.Id });
                        insertSQL.Parameters.AddWithValue("@lastname", firefighter.LastName);
                        insertSQL.Parameters.AddWithValue("@firstname", firefighter.FirstName);
                        insertSQL.Parameters.AddWithValue("@pesel", firefighter.Pesel);
                        insertSQL.Parameters.AddWithValue("@phone", firefighter.Phone);
                        insertSQL.Parameters.AddWithValue("@degree", firefighter.Degree);
                        insertSQL.Parameters.AddWithValue("@birthdate", firefighter.BirthDate);
                        insertSQL.Parameters.AddWithValue("@joindate", firefighter.JoinDate);
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
        private void EditFirefighter()
        {
            Messenger.Default.Send(new Message(NotificationToken.AskPermissions));
            Messenger.Default.Send(new Message(NotificationToken.AskCourses));
            Messenger.Default.Send(new Message<Firefighter>(NotificationToken.EditFirefighter, selectedFirefighter));
            Messenger.Default.Send(new Message(NotificationToken.LoadFirefighter));
            navigationService.NavigateToFirefighter();
        }

        private void ChangeFirefighter(Firefighter firefighter)
        {
            for (int i = 0; i < Firefighters.Count; i++)
            {
                    if (Firefighters[i].Id == firefighter.Id)
                    {
                        Firefighters[i] = firefighter.Clone();
                    }
            }
            CollectionViewSource.GetDefaultView(this.Firefighters).Refresh();


            using (con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SQLiteCommand command = new SQLiteCommand(con))
                    {
                        command.CommandText =
                            "UPDATE Strazacy SET NazwiskoStraz = @lastName, ImieStraz = @firstName, PeselStraz = @pesel," +
                            "TelefonStraz = @phone, StopienStraz = @degree, DataUrodzenia = @birthdate, DataDolaczenia = @joindate " +
                            "WHERE IdStraz=@id";
                        command.Parameters.AddWithValue("id", firefighter.Id);
                        command.Parameters.AddWithValue("lastName", firefighter.LastName);
                        command.Parameters.AddWithValue("firstName", firefighter.FirstName);
                        command.Parameters.AddWithValue("pesel", firefighter.Pesel);
                        command.Parameters.AddWithValue("phone", firefighter.Phone);
                        command.Parameters.AddWithValue("degree", firefighter.Degree);
                        command.Parameters.AddWithValue("birthdate", firefighter.BirthDate);
                        command.Parameters.AddWithValue("joindate", firefighter.JoinDate);
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


        private void RemoveFirefighter()
        {
            
            using (con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    SQLiteCommand command = new SQLiteCommand(con);
                    command.CommandText = "DELETE FROM Strazacy WHERE IdStraz='" + SelectedFirefighter.Id + "'";
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
            Firefighters.Remove(SelectedFirefighter);
        }

        private void OnExit()
        {
        }

        private void RegisterMessages()
        {
            Messenger.Default.Register<Message<Firefighter>>(this, (message) =>
            {
                if (message.Token == NotificationToken.ChangedFirefighter)
                {
                    ChangeFirefighter(message.Content);
                }
                else if (message.Token == NotificationToken.AddFirefighter)
                {
                    AddFirefighter(message.Content);
                }
            });
            Messenger.Default.Register<Message>(this, (message) =>
            {
                if (message.Token == NotificationToken.AskFirefighters)
                {
                    SendFirefighters();
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

        private void SendFirefighters()
        {
            Messenger.Default.Send(new Message<ObservableCollection<Firefighter>>(NotificationToken.SendFirefighters,firefighters));
        }
        #endregion

    
    }
}