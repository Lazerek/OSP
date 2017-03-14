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
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using OSP.Interface;
using OSP.Model;

namespace OSP.ViewModel
{
    public class CourseMenuViewModel : ViewModelBase
    {
        #region Fields

        private TypeOfWindow windowType;
        private string windowTitle;
        private readonly INavigationService navigationService;
        private ObservableCollection<Course> courses = new ObservableCollection<Course>();
        private Course selectedCourse;
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

        public ObservableCollection<Course> Courses
        {
            get { return courses; }
            set
            {
                courses = value;
                RaisePropertyChanged();
            }
        }

        public Course SelectedCourse
        {
            get { return selectedCourse; }
            set
            {
                selectedCourse = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region ICommands

        public ICommand AddCourseCommand { get; private set; }
        public ICommand EditCourseCommand { get; private set; }
        public ICommand RemoveCourseCommand { get; private set; }

        #endregion

        #region Constructors

        public CourseMenuViewModel(INavigationService navigationService)
        {
            RegisterMessages();

            this.navigationService = navigationService;

            windowType = TypeOfWindow.Courses;
            windowTitle = Helpers.GetDescription(windowType);

            AddCourseCommand = new RelayCommand(NewCourse);
            EditCourseCommand = new RelayCommand(EditCourse);
            RemoveCourseCommand = new RelayCommand(RemoveCourse);

            using (con)
            {
                try
                {
                    con.Open();
                    LoadCourses();
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd połączenia z bazą");
                }

            }

            selectedCourse = Courses.ElementAt(0);
        }

        #endregion

        #region Methods

        private void LoadCourses()
        {

            SQLiteCommand sql_cmd;
            sql_cmd = con.CreateCommand();
            sql_cmd.CommandText = "SELECT * FROM Kursy";
            sql_cmd.CommandType = CommandType.Text;
            SQLiteDataReader r = sql_cmd.ExecuteReader();
            while (r.Read())
            {
                Courses.Add(new Course(Int32.Parse(r["IdKursu"].ToString()), r["NazwaKursu"].ToString()));
            }
        }

        private void NewCourse()
        {
            int id = 0;
            using (con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using(SQLiteCommand command = new SQLiteCommand("SELECT IdKursu FROM Kursy ORDER BY IdKursu DESC", con))
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        id = Int32.Parse(reader["IdKursu"].ToString()) + 1;
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd połączenia z bazą: " + ex);
                }
            }
            Messenger.Default.Send(new Message<Course>(NotificationToken.NewCourse, new Course(id)));
            navigationService.NavigateToCourse();
        }

        private void AddCourse(Course course)
        {
            Courses.Add(course);
            using (con = new SQLiteConnection(connectionString))
            {
                con.Open();
                try
                {
                    using (SQLiteCommand insertSQL = con.CreateCommand())
                    {
                        insertSQL.CommandText = "INSERT INTO Kursy(IdKursu, NazwaKursu) VALUES (@id,@name)";
                        insertSQL.Parameters.Add(new SQLiteParameter("@id", SqlDbType.Int) { Value = course.Id });
                        insertSQL.Parameters.AddWithValue("@name", course.Name);
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
        
        private void EditCourse()
        {
            Messenger.Default.Send(new Message<Course>(NotificationToken.EditCourse, selectedCourse));
            navigationService.NavigateToCourse();
        }

        private void ChangeCourse(Course course)
        {
            for (int i = 0; i < Courses.Count; i++)
            {
                if (Courses[i].Id == course.Id)
                {
                    Courses[i] = course.Clone();
                }
            }
            CollectionViewSource.GetDefaultView(this.Courses).Refresh();


            using (con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SQLiteCommand command = new SQLiteCommand(con))
                    {
                        command.CommandText =
                            "UPDATE Kursy SET IdKursu = @id, NazwaKursu = @courseType WHERE IdKursu='" + course.Id.ToString() + "'";
                        command.Parameters.AddWithValue("id", course.Id);
                        command.Parameters.AddWithValue("courseType", course.Name);
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

        private void RemoveCourse()
        {

            using (con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    SQLiteCommand command = new SQLiteCommand(con);
                    command.CommandText = "DELETE FROM Kursy WHERE IdKursu='" + SelectedCourse.Id + "'";
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
            Courses.Remove(SelectedCourse);
        }

        private void RegisterMessages()
        {
            Messenger.Default.Register<Message<Course>>(this, (message) =>
            {
                if (message.Token == NotificationToken.ChangedCourse)
                {
                    ChangeCourse(message.Content);
                }
                else if (message.Token == NotificationToken.AddCourse)
                {
                    AddCourse(message.Content);
                }
            });
            Messenger.Default.Register<Message>(this, (message) =>
            {
                if (message.Token == NotificationToken.AskCourses)
                {
                    SendCourses();
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

        private void SendCourses()
        {
            Messenger.Default.Send(new Message<ObservableCollection<Course>>(NotificationToken.SendCourses, courses));
        }
        #endregion
    }
}