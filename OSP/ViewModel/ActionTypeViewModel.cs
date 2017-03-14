using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using OSP.Model;

namespace OSP.ViewModel
{
    /// <summary>
    /// ViewModel obsługujący okno typ akcji
    /// </summary>
    public class ActionTypeViewModel : ViewModelBase
    {
        #region Fields
        private ActionType actionType;
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

        public ActionType ActionType
        {
            get { return actionType; }
            set
            {
                actionType = value;
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
        public ICommand SaveActionTypeCommand { get; private set; }
        #endregion

        #region Constructors
        public ActionTypeViewModel()
        {
            SaveActionTypeCommand = new RelayCommand(SaveActionType);

            RegisterMessages();
        }
        #endregion

        #region Methods
        private void RegisterMessages()
        {
            Messenger.Default.Register<Message<ActionType>>(this, (message) =>
            {
                ActionType = message.Content?.Clone() ?? new ActionType();
                if (message.Token == NotificationToken.EditActionType)
                {
                    windowType = TypeOfWindow.EditActionType;
                    windowTitle = Helpers.GetDescription(windowType);
                    buttonText = "Zmień";
                }
                else if (message.Token == NotificationToken.NewActionType)
                {
                    windowType = TypeOfWindow.AddActionType;
                    windowTitle = Helpers.GetDescription(windowType);
                    buttonText = "Dodaj";
                }
            });
        }

        private void SaveActionType()
        {
            if ((WindowType == TypeOfWindow.AddActionType))
                Messenger.Default.Send(new Message<ActionType>(NotificationToken.AddActionType, actionType));
            else
                Messenger.Default.Send(new Message<ActionType>(NotificationToken.ChangedActionType, actionType));
        }
        #endregion
    }
}