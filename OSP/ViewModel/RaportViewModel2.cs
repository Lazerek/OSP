using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using OSP.Model;
using Action = OSP.Model.Action;

namespace OSP.ViewModel
{
    public class RaportViewModel2 : ViewModelBase
    {
        #region Fields
        private string quartet;
        private string period;
        private string year;
        private ObservableCollection<Action> actions = new ObservableCollection<Action>();
        private ObservableCollection<Action> selectedActions = new ObservableCollection<Action>();
        private ObservableCollection<ActionType> actionTypes = new ObservableCollection<ActionType>();
        private ObservableCollection<RaportList> list = new ObservableCollection<RaportList>();

        #endregion

        #region Properties

        public ObservableCollection<RaportList> MyList
        {
            get { return list; }
        }
        public string Period { get { return period; } }
        #endregion

        #region ICommands
        #endregion

        #region Constructors

        public RaportViewModel2()
        {
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
            Messenger.Default.Register<Message<ObservableCollection<ActionType>>>(this, (message) =>
            {
                if (message.Token == NotificationToken.SendActionTypes)
                {
                    actionTypes = new ObservableCollection<ActionType>();
                    actionTypes = message.Content;
                }
            });
            Messenger.Default.Register<Message<string, string>>(this, (message) =>
            {
                if (message.Token == NotificationToken.ActionRaport2)
                {
                    period = "Okres: " + message.Content1 + " " + message.Content2 + "r";
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
            foreach (var actionType in actionTypes)
            {
                list.Add(new RaportList(actionType.Id, actionType.Type));
            }
            foreach (var action in selectedActions)
            {
                foreach (var mylist in list)
                {
                    if (action.ActionType.Id == mylist.Id)
                    {
                        mylist.Increase();
                    }
                }
            }
        }
        #endregion

    }
}