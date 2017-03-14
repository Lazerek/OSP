using System;
using System.Collections.ObjectModel;
using System.Globalization;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using OSP.Model;
using Action = OSP.Model.Action;

namespace OSP.ViewModel
{
    /// <summary>
    /// Klasa typu viewmodel obsługująca widok raportów
    /// </summary>
    public class RaportViewModel : ViewModelBase
    {
        #region Fields

        private string quartet;
        private string period;
        private string year;
        private string numberOfActions;
        private string timeOfActions;
        private string numberOfKM;
        private ObservableCollection<Action> actions = new ObservableCollection<Action>();
        private ObservableCollection<Action> selectedActions = new ObservableCollection<Action>();
        #endregion

        #region Properties
        public string Period { get {return period;} }
        public string NumberOfActions { get {return numberOfActions;} }
        public string TimeOfActions { get {return timeOfActions;} }
        public string NumberOfKM { get {return numberOfKM;} }
        #endregion

        #region Constructors

        public RaportViewModel()
        {
            period = "";
            numberOfActions = "";
            timeOfActions = "";
            numberOfKM = "";
            RegisterMessages();
        }
        #endregion

        #region Methods

        private void RegisterMessages()
        {
            Messenger.Default.Register<Message<ObservableCollection<Action>>>(this, (message) =>
            {
                if (message.Token == NotificationToken.SendActions)
                {
                    actions = new ObservableCollection<Action>();
                    actions = message.Content;
                }
            });
            Messenger.Default.Register<Message<string, string>>(this, (message) =>
            {
                if (message.Token == NotificationToken.ActionRaport)
                {
                    period = message.Content1 + " " + message.Content2 + "r";
                    year = message.Content2;
                    quartet = message.Content1;
                    SelectActions();
                }
            });
        }

        private void SelectActions()
        {
            string start;
            string end;
            string wholeStart;
            string wholeEnd;
            if (quartet == "I kwartał")
            {
                start = "01-01";
                end = "31-03";
            }
            else if (quartet == "II kwartał")
            {
                start = "01-04";
                end = "30-06";
            }
            else if (quartet == "III kwartał")
            {
                start = "01-07";
                end = "30-09";
            }
            else
            {
                start = "01-10";
                end = "31-12";
            }
            wholeStart = start + "-" + year;
            wholeEnd = end + "-" + year;
            DateTime periodStartDate = DateTime.ParseExact(wholeStart, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime periodEndDate = DateTime.ParseExact(wholeEnd, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            selectedActions = new ObservableCollection<Action>();
            foreach (var action in actions)
            {
                DateTime thisDate = DateTime.ParseExact(action.DateOfAction, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                if (thisDate <= periodEndDate && thisDate >= periodStartDate)
                {
                    selectedActions.Add(action);
                }
            }
            Count();
        }

        private void Count()
        {
            int number = 0;
            int km = 0;
            int time = 0;
            int hours;
            int minutes;
            foreach (Action action in selectedActions)
            {
                number++;
                km += action.NumberOfKM;
                time += action.MinutesOfAction;
            }

            numberOfActions = number.ToString();
            numberOfKM = km.ToString();
            hours = time/60;
            minutes = time - (hours*60);
            timeOfActions = hours.ToString() + " godzin " + minutes.ToString() + " minut";

        }
        #endregion
    }
}