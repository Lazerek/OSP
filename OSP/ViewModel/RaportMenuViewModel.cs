using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using OSP.Interface;
using OSP.Model;

namespace OSP.ViewModel
{
    public class RaportMenuViewModel : ViewModelBase
    {
        #region Fields

        private List<string> quarter = new List<string>();
        private List<string> year = new List<string>();
        private string selectedQuarter;
        private string selectedYear;
        private INavigationService navigationService;

        #endregion

        #region Properties

        public List<string> Quarter
        {
            get { return quarter;}
        }

        public List<string> Year
        {
            get { return year; }
        }

        public string SelectedQuarter
        {
            get { return selectedQuarter;}
            set
            {
                selectedQuarter = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedYear
        {
            get {return selectedYear; }
            set
            {
                selectedYear = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region ICommands
        public ICommand OpenFirstRaportCommand { get; private set; }
        public ICommand OpenSecondRaportCommand { get; private set; }
        #endregion

        #region Constuctors

        public RaportMenuViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            CreateDates();
            OpenFirstRaportCommand = new RelayCommand(OpenFirstRaport);
            OpenSecondRaportCommand = new RelayCommand(OpenSecondRapord);
            
        }
        #endregion

        #region Methods

        private void OpenSecondRapord()
        {
            Messenger.Default.Send(new Message(NotificationToken.AskAction));
            Messenger.Default.Send(new Message(NotificationToken.AskActionTypes));
            Messenger.Default.Send(new Message<string, string>(NotificationToken.ActionRaport2, selectedQuarter, selectedYear));
            navigationService.NavigateToSecondRaport();
        }
        private void OpenFirstRaport()
        {
            Messenger.Default.Send(new Message(NotificationToken.AskAction));
            Messenger.Default.Send(new Message<string, string>(NotificationToken.ActionRaport,selectedQuarter,selectedYear));
            navigationService.NavigateToFirstRaport();
        }
        private void CreateDates()
        {
            quarter.Add("I kwartał");
            quarter.Add("II kwartał");
            quarter.Add("III kwartał");
            quarter.Add("IV kwartał");
            year.Add("2017");
            year.Add("2016");
            year.Add("2015");
            year.Add("2014");
            selectedQuarter = quarter.ElementAt(0);
            selectedYear = year.ElementAt(0);
        }
        #endregion

    }
}