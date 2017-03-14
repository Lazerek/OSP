using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using OSP.Model;

namespace OSP.ViewModel
{
    /// <summary>
    /// Klasa typu viewmodel obsługująca okno uprawnień 
    /// </summary>
    public class PermissionViewModel : ViewModelBase
    {
        #region Fields
        private Permission permission;
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

        public Permission Permission
        {
            get { return permission; }
            set
            {
                permission = value;
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
        public ICommand SavePermissionCommand { get; private set; }
        #endregion

        #region Constructors

        public PermissionViewModel()
        {
            SavePermissionCommand = new RelayCommand(SavePermission);

            RegisterMessages();
        }

        #endregion

        #region Methods
        private void RegisterMessages()
        {
            Messenger.Default.Register<Message<Permission>>(this, (message) =>
            {
                Permission = message.Content?.Clone() ?? new Permission();
                if (message.Token == NotificationToken.EditPermission)
                {
                    windowType = TypeOfWindow.EditPermission;
                    windowTitle = Helpers.GetDescription(windowType);
                    buttonText = "Zmień";
                }
                else if (message.Token == NotificationToken.NewPermission)
                {
                    windowType = TypeOfWindow.AddPermission;
                    windowTitle = Helpers.GetDescription(windowType);
                    buttonText = "Dodaj";
                }
            });
        }

        private void SavePermission()
        {
            if ((WindowType == TypeOfWindow.AddPermission))
                Messenger.Default.Send(new Message<Permission>(NotificationToken.AddPermission, permission));
            else
                Messenger.Default.Send(new Message<Permission>(NotificationToken.ChangedPermission, permission));
        }
        #endregion
    }
}