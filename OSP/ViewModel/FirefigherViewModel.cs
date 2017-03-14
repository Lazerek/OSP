using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using OSP.Model;

namespace OSP.ViewModel
{
    /// <summary>
    /// Klasa typu viewmodel obsługująca okno strażaka
    /// </summary>
    public class FirefigherViewModel : ViewModelBase
    {
        #region Fields

        private Firefighter firefighter;
        private TypeOfWindow windowType;
        private string windowTitle;
        private string buttonText;
        private SQLiteConnection con = new SQLiteConnection(connectionString);
        private List<ListModel> coursesList = new List<ListModel>();
        private List<ListModel> permissionList = new List<ListModel>();
        private ObservableCollection<Permission> permissions;
        private ObservableCollection<Course> courses;
        private List<int> doneCourses = new List<int>();
        private List<int> donePermissions = new List<int>();

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

        public Firefighter Firefighter
        {
            get { return firefighter; }
            set
            {
                firefighter = value; 
                RaisePropertyChanged();
            }
        }

        public string ButtonText
        {
            get {  return buttonText;}
            set
            {
                buttonText = value;
                RaisePropertyChanged();
            }
        }

        public List<ListModel> CoursesList
        {
            get { return coursesList;}
            set
            {
                coursesList = value;
                RaisePropertyChanged();
            }
        }

        public List<ListModel> PermissionsList
        {
            get {  return permissionList;}
            set
            {
                permissionList = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public ICommand SaveFirefighterCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }
        #region Constructors

        public FirefigherViewModel()
        {
            SaveFirefighterCommand = new RelayCommand(SaveFirefighter);

            RegisterMessages();
        }
        #endregion

        #region Methods

        private void CreateCheckLists()
        {
            coursesList = new List<ListModel>();
            permissionList = new List<ListModel>();
            foreach (var course in courses)
            {
                coursesList.Add(new ListModel(course.Id, course.Name));
            }
            foreach (var permission in permissions)
            {
                permissionList.Add(new ListModel(permission.Id, permission.Name));
            }
            LoadCourses();
            LoadPermissions();
        }

        private void LoadCourses()
        {
            doneCourses = new List<int>();
            using (con = new SQLiteConnection(connectionString))
            {
                try
                {
                    string cmdString = "SELECT * FROM OsobyPoKursach WHERE idStraz=" + firefighter.Id.ToString();
                    con.Open();
                    using (SQLiteCommand command = new SQLiteCommand(cmdString, con))
                    {

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader.HasRows)
                                    doneCourses.Add(Int32.Parse(reader["IdKursu"].ToString()));
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
            foreach (var id in doneCourses)
            {
                foreach (var course in coursesList)
                {
                    if (course.Id == id)
                    {
                        course.SetSelect();
                    }
                }
            }
        }

        private void SaveCourses()
        {
            List<int> toAdd = new List<int>();
            List<int> toRemove = new List<int>();
            foreach (var course in coursesList)
            {
                if (course.IsSelected)
                {
                    if (!doneCourses.Contains(course.Id))
                    {
                        toAdd.Add(course.Id);
                    }
                }
                else
                {
                    if (doneCourses.Contains(course.Id))
                    {
                        toRemove.Add(course.Id);
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
                                insertSQL.CommandText = "INSERT INTO OsobyPoKursach(IdKursu, IdStraz) VALUES (@idK,@idF)";
                                insertSQL.Parameters.Add(new SQLiteParameter("@idK", SqlDbType.Int) { Value = id });
                                insertSQL.Parameters.Add(new SQLiteParameter("@idF", SqlDbType.Int) { Value = firefighter.Id });
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
                            string cmdString = "DELETE FROM OsobyPoKursach WHERE IdKursu='" + id + "' AND IdStraz='" + firefighter.Id + "'";
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

        private void LoadPermissions()
        {
            donePermissions = new List<int>();
            using (con = new SQLiteConnection(connectionString))
            {
                try
                {
                    string cmdString = "SELECT * FROM Uprawnieni WHERE idStraz=" + firefighter.Id.ToString();
                    con.Open();
                    using (SQLiteCommand command = new SQLiteCommand(cmdString, con))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader.HasRows)
                                    donePermissions.Add(Int32.Parse(reader["IdUprawnienia"].ToString()));
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
            foreach (var id in donePermissions)
            {
                foreach (var permission in permissionList)
                {
                    if (permission.Id == id)
                    {
                        permission.SetSelect();
                    }
                }
            }
        }

        private void SavePermissions()
        {
            List<int> toAdd = new List<int>();
            List<int> toRemove = new List<int>();
            foreach (var permission in permissionList)
            {
                if (permission.IsSelected)
                {
                    if (!donePermissions.Contains(permission.Id))
                    {
                        toAdd.Add(permission.Id);
                        Debug.WriteLine("Do dodania: " + permission.Id);
                    }
                }
                else
                {
                    if (donePermissions.Contains(permission.Id))
                    {
                        toRemove.Add(permission.Id);
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
                                insertSQL.CommandText = "INSERT INTO Uprawnieni(IdUprawnienia, IdStraz) VALUES (@idK,@idF)";
                                insertSQL.Parameters.Add(new SQLiteParameter("@idK", SqlDbType.Int) {Value = id});
                                insertSQL.Parameters.Add(new SQLiteParameter("@idF", SqlDbType.Int) { Value = firefighter.Id });
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
                            string cmdString = "DELETE FROM Uprawnieni WHERE IdUprawnienia='" + id + "' AND IdStraz='" + firefighter.Id + "'";
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
            Messenger.Default.Register<Message<Firefighter>>(this, (message) =>
            {
                Firefighter = message.Content?.Clone() ?? new Firefighter();
                if (message.Token == NotificationToken.EditFirefighter)
                {
                    windowType = TypeOfWindow.EditFirefighter;
                    windowTitle = Helpers.GetDescription(windowType);
                    buttonText = "Zmień";
                }
                else if (message.Token == NotificationToken.NewFirefighter)
                {
                    windowType = TypeOfWindow.AddFirefighter;
                    windowTitle = Helpers.GetDescription(windowType);
                    buttonText = "Dodaj";
                }
            });
            Messenger.Default.Register<Message<ObservableCollection<Permission>>>(this, (message) =>
            {
                if (message.Token == NotificationToken.SendPermissions)
                {
                        permissions = new ObservableCollection<Permission>();
                        permissions = message.Content;
                }
                
            });
            Messenger.Default.Register<Message<ObservableCollection<Course>>>(this, (message) =>
            {
                if (message.Token == NotificationToken.SendCourses)
                {
                    courses = new ObservableCollection<Course>();
                    courses = message.Content;
                }
            });
            Messenger.Default.Register<Message>(this, (message) =>
            {
                if (message.Token == NotificationToken.LoadFirefighter)
                {
                    CreateCheckLists();
                }
            });
        }

      

        private void SaveFirefighter()
        {
            SaveCourses();
            SavePermissions();
            if ((WindowType == TypeOfWindow.AddFirefighter))
                Messenger.Default.Send(new Message<Firefighter>(NotificationToken.AddFirefighter, firefighter));
            else
                Messenger.Default.Send(new Message<Firefighter>(NotificationToken.ChangedFirefighter, firefighter));
        }
        #endregion
    }
}