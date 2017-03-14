/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:OSP"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using OSP.Interface;
using OSP.Model;
using OSP.Service;

namespace OSP.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<INavigationService, NavigationService>();
            SimpleIoc.Default.Register<MainViewModel>(true);
            SimpleIoc.Default.Register<ActionTypeViewModel>(true);
            SimpleIoc.Default.Register<ActionTypeMenuViewModel>(true);
            SimpleIoc.Default.Register<ActionViewModel>(true);
            SimpleIoc.Default.Register<ActionMenuViewModel>(true);
            SimpleIoc.Default.Register<CarViewModel>(true);
            SimpleIoc.Default.Register<CarMenuViewModel>(true);
            SimpleIoc.Default.Register<CourseViewModel>(true);
            SimpleIoc.Default.Register<CourseMenuViewModel>(true);
            SimpleIoc.Default.Register<FirefigherViewModel>(true);
            SimpleIoc.Default.Register<FirefighterMenuViewModel>(true);
            SimpleIoc.Default.Register<PermissionViewModel>(true);
            SimpleIoc.Default.Register<PermissionMenuViewModel>(true);
            SimpleIoc.Default.Register<RaportViewModel>(true);
            SimpleIoc.Default.Register<RaportMenuViewModel>(true);
            SimpleIoc.Default.Register<LoginViewModel>(true);
            SimpleIoc.Default.Register<RaportViewModel2>(true);
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public RaportViewModel2 RaportViewModel2
        {
            get { return ServiceLocator.Current.GetInstance<RaportViewModel2>(); }
        }

        public LoginViewModel LoginViewModel
        {
            get { return ServiceLocator.Current.GetInstance<LoginViewModel>(); }
        }

        public ActionViewModel ActionViewModel
        {
            get { return ServiceLocator.Current.GetInstance<ActionViewModel>(); }
        }

        public ActionMenuViewModel ActionMenuViewModel
        {
            get { return ServiceLocator.Current.GetInstance<ActionMenuViewModel>(); }
        }

        public ActionTypeViewModel ActionTypeViewModel
        {
            get { return ServiceLocator.Current.GetInstance<ActionTypeViewModel>(); }
        }

        public ActionTypeMenuViewModel ActionTypeMenuViewModel
        {
            get { return ServiceLocator.Current.GetInstance<ActionTypeMenuViewModel>(); }
        }

        public CarViewModel CarViewModel
        {
            get { return ServiceLocator.Current.GetInstance<CarViewModel>(); }
        }

        public CarMenuViewModel CarMenuViewModel
        {
            get { return ServiceLocator.Current.GetInstance<CarMenuViewModel>(); }
        }

        public CourseViewModel CourseViewModel
        {
            get { return ServiceLocator.Current.GetInstance<CourseViewModel>(); }
        }

        public CourseMenuViewModel CourseMenuViewModel
        {
            get { return ServiceLocator.Current.GetInstance<CourseMenuViewModel>(); }
        }
        
        public FirefigherViewModel FirefigherViewModel
        {
            get { return ServiceLocator.Current.GetInstance<FirefigherViewModel>(); }
        }

        public FirefighterMenuViewModel FirefighterMenuViewModel
        {
            get { return ServiceLocator.Current.GetInstance<FirefighterMenuViewModel>(); }
        }

        public PermissionViewModel PermissionViewModel
        {
            get { return ServiceLocator.Current.GetInstance<PermissionViewModel>(); }
        }

        public PermissionMenuViewModel PermissionMenuViewModel
        {
            get { return ServiceLocator.Current.GetInstance<PermissionMenuViewModel>(); }
        }

        public RaportViewModel RaportViewModel
        {
            get { return ServiceLocator.Current.GetInstance<RaportViewModel>(); }
        }

        public RaportMenuViewModel RaportMenuViewModel
        {
            get { return ServiceLocator.Current.GetInstance<RaportMenuViewModel>(); }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}