using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using OSP.Interface;
using OSP.Model;

namespace OSP.ViewModel
{
    /// <summary>
    /// Klasa typu viewmodel obs³uguj¹ca logikê g³ównego okna programu
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private TypeOfWindow windowType;
        private User user = User.Commander;
        private string windowTitle;
        private readonly INavigationService navigationService;

        

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
        #endregion

        #region ICommands
        public ICommand OpenFirefigterMenuCommand { get; private set; }
        public ICommand OpenCarMenuCommand { get; private set; }
        public ICommand OpenActionTypeMenuCommand { get; private set; }
        public ICommand OpenActionMenuCommand { get; private set; }
        public ICommand OpenCourseMenuCommand { get; private set; }
        public ICommand OpenPermissionMenuCommand { get; private set; }
        public ICommand OpenRaportViewCommand { get; private set; }
        
        #endregion


        #region Constructors
        public MainViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            windowType = TypeOfWindow.MainView;
            windowTitle = Helpers.GetDescription(windowType);

            OpenFirefigterMenuCommand = new RelayCommand(OpenFirefighterMenu);
            OpenCarMenuCommand = new RelayCommand(OpenCarMenu);
            OpenCourseMenuCommand = new RelayCommand(OpenCourseMenu);
            OpenActionTypeMenuCommand = new RelayCommand(OpenActionTypeMenu);
            OpenCourseMenuCommand = new RelayCommand(OpenCourseMenu);
            OpenPermissionMenuCommand = new RelayCommand(OpenPermissionMenu);
            OpenRaportViewCommand = new RelayCommand(OpenRaportView);
            OpenActionMenuCommand = new RelayCommand(OpenActionMenu);

            RegisterMessages();
        }
        #endregion

        #region Methods

        private void RegisterMessages()
        {
            Messenger.Default.Register<Message>(this, (message) =>
            {
                if (message.User == User.Commander)
                {
                    user = User.Commander;
                }
                else if (message.User == User.Firefighter)
                {
                    user = User.Firefighter;
                }
            });
        }

        private void OpenFirefighterMenu()
        {
           /* if (user == User.Firefighter)
            {
                Messenger.Default.Send(new Message(User.Firefighter));
            }
            else if(user == User.Commander)
            {
                Messenger.Default.Send(new Message(User.Commander));
            }*/
            navigationService.NavigateToFirefighterMenu();
        }

        private void OpenActionTypeMenu()
        {
            /*
            if (user == User.Firefighter)
            {
                Messenger.Default.Send(new Message(User.Firefighter));
            }
            else if (user == User.Commander)
            {
                Messenger.Default.Send(new Message(User.Commander));
            }*/
            navigationService.NavigateToActionTypeMenu();
        }

        private void OpenActionMenu()
        {
            /*
            if (user == User.Firefighter)
            {
                Messenger.Default.Send(new Message(User.Firefighter));
            }
            else if (user == User.Commander)
            {
                Messenger.Default.Send(new Message(User.Commander));
            }
            */
            navigationService.NavigateToActionMenu();
        }

        private void OpenCarMenu()
        {
            /*
            if (user == User.Firefighter)
            {
                Messenger.Default.Send(new Message(User.Firefighter));
            }
            else if (user == User.Commander)
            {
                Messenger.Default.Send(new Message(User.Commander));
            }
            */
            navigationService.NavigateToCarMenu();
        }

        private void OpenCourseMenu()
        {
            /*
            if (user == User.Firefighter)
            {
                Messenger.Default.Send(new Message(User.Firefighter));
            }
            else if (user == User.Commander)
            {
                Messenger.Default.Send(new Message(User.Commander));
            }*/
            navigationService.NavigateToCourseMenu();
        }


        private void OpenPermissionMenu()
        {
            /*
            if (user == User.Firefighter)
            {
                Messenger.Default.Send(new Message(User.Firefighter));
            }
            else if (user == User.Commander)
            {
                Messenger.Default.Send(new Message(User.Commander));
            }
            */
            navigationService.NavigateToPermissionMenu();
        }

        private void OpenRaportView()
        {
            /*
            if (user == User.Firefighter)
            {
                Messenger.Default.Send(new Message(User.Firefighter));
            }
            else if (user == User.Commander)
            {
                Messenger.Default.Send(new Message(User.Commander));
            }*/
            navigationService.NavigateToRaport();
        }

        #endregion


    }
}