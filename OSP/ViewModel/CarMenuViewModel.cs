using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using OSP.Interface;
using OSP.Model;

namespace OSP.ViewModel
{
    public class CarMenuViewModel : ViewModelBase
    {
        #region Fields
        private TypeOfWindow windowType;
        private string windowTitle;
        private readonly INavigationService navigationService;
        private ObservableCollection<Car> cars = new ObservableCollection<Car>();
        private Car selectedCar;
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

        public ObservableCollection<Car> Cars
        {
            get { return cars; }
            set
            {
                cars = value;
                RaisePropertyChanged();
            }
        }

        public Car SelectedCar
        {
            get { return selectedCar; }
            set
            {
                selectedCar = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region ICommands
        public ICommand AddCarCommand { get; private set; }
        public ICommand EditCarCommand { get; private set; }
        public ICommand RemoveCarCommand { get; private set; }
        #endregion

        #region Constructors

        public CarMenuViewModel(INavigationService navigationService)
        {
            RegisterMessages();

            this.navigationService = navigationService;

            windowType = TypeOfWindow.Cars;
            windowTitle = Helpers.GetDescription(windowType);

            AddCarCommand = new RelayCommand(NewCar);
            EditCarCommand = new RelayCommand(EditCar);
            RemoveCarCommand = new RelayCommand(RemoveCar);

            using (con)
            {
                try
                {
                    con.Open();
                    LoadCars();
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

            selectedCar = cars.ElementAt(0);

        }
        #endregion

        #region Methods

        private void LoadCars()
        {

            SQLiteCommand sql_cmd;
            sql_cmd = con.CreateCommand();
            sql_cmd.CommandText = "SELECT * FROM Woz";
            sql_cmd.CommandType = CommandType.Text;
            SQLiteDataReader r = sql_cmd.ExecuteReader();
            while (r.Read())
            {
                Cars.Add(new Car(Int32.Parse(r["IdWozu"].ToString()), r["NumerWozu"].ToString(), r["RodzajWozu"].ToString(), Int32.Parse(r["PojemnoscWody"].ToString())));
            }
        }
        private void NewCar()
        {
            int id = 0;
            using (con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SQLiteCommand command = new SQLiteCommand("SELECT IdWozu FROM Woz ORDER BY IdWozu DESC", con))
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        id = Int32.Parse(reader["IdWozu"].ToString()) + 1;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd połączenia z bazą: " + ex);
                }
            }
            Messenger.Default.Send(new Message<Car>(NotificationToken.NewCar, new Car(id)));
            navigationService.NavigateToCar();
        }

        private void AddCar(Car car)
        {
            Cars.Add(car);
            using (con = new SQLiteConnection(connectionString))
            {
                con.Open();
                try
                {
                    using (SQLiteCommand insertSQL = con.CreateCommand())
                    {
                        insertSQL.CommandText = "INSERT INTO Woz(IdWozu, NumerWozu, RodzajWozu, PojemnoscWody) VALUES (@id,@number,@type,@volume)";
                        insertSQL.Parameters.Add(new SQLiteParameter("@id", SqlDbType.Int) { Value = car.Id });
                        insertSQL.Parameters.AddWithValue("@number", car.CarNumber);
                        insertSQL.Parameters.AddWithValue("@type", car.CarType);
                        insertSQL.Parameters.AddWithValue("@volume", car.WaterVolume);
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

        private void EditCar()
        {
            Messenger.Default.Send(new Message<Car>(NotificationToken.EditCar, selectedCar));
            navigationService.NavigateToCar();
        }

        private void ChangeCar(Car car)
        {
            for (int i = 0; i < Cars.Count; i++)
            {
                if (Cars[i].Id == car.Id)
                {
                    Cars[i] = car.Clone();
                }
            }
            CollectionViewSource.GetDefaultView(this.Cars).Refresh();


            using (con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    using (SQLiteCommand command = new SQLiteCommand(con))
                    {
                        command.CommandText =
                            "UPDATE Woz SET IdWozu = @id, RodzajWozu = @carType, NumerWozu = @carNumber," +
                            "PojemnoscWody = @waterVolume WHERE IdWozu='" + car.Id.ToString() + "'";
                        command.Parameters.AddWithValue("id", car.Id);
                        command.Parameters.AddWithValue("carType", car.CarType);
                        command.Parameters.AddWithValue("carNumber", car.CarNumber);
                        command.Parameters.AddWithValue("waterVolume", car.WaterVolume);
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


        private void RemoveCar()
        {

            using (con = new SQLiteConnection(connectionString))
            {
                try
                {
                    con.Open();
                    SQLiteCommand command = new SQLiteCommand(con);
                    command.CommandText = "DELETE FROM Woz WHERE IdWozu='" + SelectedCar.Id + "'";
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
            Cars.Remove(SelectedCar);
        }

        private void RegisterMessages()
        {
            Messenger.Default.Register<Message<Car>>(this, (message) =>
            {
                if (message.Token == NotificationToken.ChangedCar)
                {
                    ChangeCar(message.Content);
                }
                else if (message.Token == NotificationToken.AddCar)
                {
                    AddCar(message.Content);
                }
            });
            Messenger.Default.Register<Message>(this, (message) =>
            {
                if (message.Token == NotificationToken.AskFirefighters)
                {
                    SendCars();
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

        private void SendCars()
        {
            Messenger.Default.Send(new Message<ObservableCollection<Car>>(NotificationToken.SendCars, cars));
        }
        #endregion
    }
}