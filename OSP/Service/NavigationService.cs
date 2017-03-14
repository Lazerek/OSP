using GalaSoft.MvvmLight.Messaging;
using OSP.Interface;
using OSP.Model;
using OSP.View;
using OSP.ViewModel;

namespace OSP.Service
{
    public class NavigationService : INavigationService
    {
        public void NavigateToAction()
        {
            var view = new ActionView();
            view.ShowDialog();
        }

        public void NavigateToActionMenu()
        {
            var view = new ActionMenuView();
            view.ShowDialog();
        }

        public void NavigateToActionType()
        {
            var view = new ActionTypeView();
            view.ShowDialog();
        }

        public void NavigateToActionTypeMenu()
        {
            var view = new ActionTypeMenuView();
            view.ShowDialog();
        }

        public void NavigateToCar()
        {
            var view = new CarView();
            view.ShowDialog();
        }

        public void NavigateToCarMenu()
        {
            var view = new CarMenuView();
            view.ShowDialog();
        }

        public void NavigateToCourse()
        {
            var view = new CourseView();
            view.ShowDialog();
        }

        public void NavigateToCourseMenu()
        {
            var view = new CourseMenuView();
            view.ShowDialog();
        }

        public void NavigateToFirefighter()
        {
            var view = new FirefighterView();
            view.ShowDialog();
        }


        public void NavigateToPermission()
        {
           var view = new PermissionView();
            view.ShowDialog();
        }

        public void NavigateToPermissionMenu()
        {
            var view = new PermissionMenuView();
            view.ShowDialog();
        }

        public void NavigateToFirefighterMenu()
        {
            var view = new FirefighterMenuView();
            view.ShowDialog();
        }

        public void NavigateToRaport()
        {
            var view = new RaportMenuView();
            view.ShowDialog();
        }

        public void NavigatoToMain()
        {
            var view = new MainView();
            Messenger.Default.Send(new Message(NotificationToken.Close));
            view.ShowDialog();
        }

        public void NavigateToSecondRaport()
        {
            var view = new RaportView2();
            view.ShowDialog();
        }

        public void NavigateToFirstRaport()
        {
            var view = new RaportView();
            view.ShowDialog();
        }
    }
}