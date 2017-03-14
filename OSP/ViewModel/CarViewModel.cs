using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using OSP.Model;

namespace OSP.ViewModel
{
    /// <summary>
    /// Klasa typu viewModel obsługująca widok wozu
    /// </summary>
    public class CarViewModel : ViewModelBase
    {
        #region Fields
        private Car car;
        private TypeOfWindow windowType;
        private string windowTitle;
        private string buttonText;
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

        public Car Car
        {
            get { return car; }
            set
            {
                car = value;
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
        #endregion

        #region ICommands
        public ICommand SaveCarCommand { get; private set; }
        #endregion

        #region Constructors

        public CarViewModel()
        {

            SaveCarCommand = new RelayCommand(SaveCar);

            RegisterMessages();
            
        }
        #endregion

        #region Methods
        private void RegisterMessages()
        {
            //Rejestrowanie wiadomości z zawartościa wozu
            Messenger.Default.Register<Message<Car>>(this, (message) =>
            {
                Car = message.Content?.Clone() ?? new Car(); //Jeśli zawartość jest pusta stworzenie nowego wozu, jeśli wóz nie jest pusty to stworzenie głębokiej kopii naszego pojazdu
                if (message.Token == NotificationToken.EditCar) //Przypadek gdy planujemy modyfikować wóz
                {
                    windowType = TypeOfWindow.EditCar; //Ustawienie typu okna na edycję wozu
                    windowTitle = Helpers.GetDescription(windowType); //Pobranie z enumeratorów opisu okna
                    buttonText = "Zmień"; //Ustawienie tekstu przycisku
                }
                else if (message.Token == NotificationToken.NewCar)
                {
                    windowType = TypeOfWindow.AddCar;//Ustawienie typu okna na edycję wozu
                    windowTitle = Helpers.GetDescription(windowType);//Pobranie z enumeratorów opisu okna
                    buttonText = "Dodaj";//Ustawienie tekstu przycisku
                }
            });
        }

        private void SaveCar()
        {
            if ((WindowType == TypeOfWindow.AddCar))
                Messenger.Default.Send(new Message<Car>(NotificationToken.AddCar, car));
            else
                Messenger.Default.Send(new Message<Car>(NotificationToken.ChangedCar, car));
        }
        #endregion
    }
}