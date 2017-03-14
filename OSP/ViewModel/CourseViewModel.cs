using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using OSP.Model;

namespace OSP.ViewModel
{
    /// <summary>
    /// Klasa typu viewModel obslugująca okno kurs
    /// </summary>
    public class CourseViewModel : ViewModelBase
    {
        #region Fields
        private Course course;
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

        public Course Course
        {
            get { return course; }
            set
            {
                course = value;
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
        public ICommand SaveCourseCommand { get; private set; }
        #endregion

        #region Constructors

        public CourseViewModel()
        {
            SaveCourseCommand = new RelayCommand(SaveCourse);

            RegisterMessages();
        }
        #endregion

        #region Methods
        private void RegisterMessages()
        {
            Messenger.Default.Register<Message<Course>>(this, (message) =>
            {
                Course = message.Content?.Clone() ?? new Course();
                if (message.Token == NotificationToken.EditCourse)
                {
                    windowType = TypeOfWindow.EditCourse;
                    windowTitle = Helpers.GetDescription(windowType);
                    buttonText = "Zmień";
                }
                else if (message.Token == NotificationToken.NewCourse)
                {
                    windowType = TypeOfWindow.AddCourse;
                    windowTitle = Helpers.GetDescription(windowType);
                    buttonText = "Dodaj";
                }
            });
        }

        private void SaveCourse()
        {
            if ((WindowType == TypeOfWindow.AddCourse))
                Messenger.Default.Send(new Message<Course>(NotificationToken.AddCourse, course));
            else
                Messenger.Default.Send(new Message<Course>(NotificationToken.ChangedCourse, course));
        }
        #endregion
    }
}