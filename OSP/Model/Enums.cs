using System;
using System.ComponentModel;

namespace OSP.Model
{

       public enum NotificationToken
        {
            AddFirefighter,
            NewFirefighter,
            EditFirefighter,
            ChangedFirefighter,
            NewAction,
            EditAction,
            AddAction,
            ChangedAction,
            NewCar,
            EditCar,
            AddCar,
            ChangedCar,
            NewCourse,
            EditCourse,
            AddCourse,
            ChangedCourse,
            NewPermission,
            AddPermission,
            EditPermission,
            ChangedPermission,
            NewActionType,
            AddActionType,
            EditActionType,
            ChangedActionType,
            AskPermissions,
            SendPermissions,
            AskCourses,
            SendCourses,
            AskActionTypes,
            SendActionTypes,
            AskFirefighters,
            SendFirefighters,
            SendActions,
            AskCars,
            SendCars,
            LoadFirefighter,
            LoadAction,
            Close,
            ActionRaport,
            AskAction,
            ActionRaport2,
            User,
            FirefighterLogin,
            CommanderLogin
        }

    public enum Model
    {
        [Description("Akcja")]
        Action,
        [Description("TypAkcji")]
        ActionType,
        [Description("Woz")]
        Car,
        [Description("Kurs")]
        Course,
        [Description("Strazacy")]
        Firefighter,
        [Description("Uprawnienie")]
        Permission
    }

    public enum User
    {
        Firefighter,
        Commander
    }

    public enum TypeOfWindow
    {
        [Description("Zarządzenie jednostką OSP")]
        MainView,
        [Description("Strażacy")]
        Firefighters,
        [Description("Dodaj strażaka")]
        AddFirefighter,
        [Description("Edytuj strażaka")]
        EditFirefighter,
        [Description("Wozy")]
        Cars,
        [Description("Dodaj wóz")]
        AddCar,
        [Description("Edytuj wóz")]
        EditCar,
        [Description("Kursy")]
        Courses,
        [Description("Dodaj kurs")]
        AddCourse,
        [Description("Edytuj kurs")]
        EditCourse,
        [Description("Typy akcji")]
        ActionTypes,
        [Description("Dodaj typ akcji")]
        AddActionType,
        [Description("Edytuj typ akcji")]
        EditActionType,
        [Description("Uprawnienia")]
        Permissions,
        [Description("Dodaj uprawnienie")]
        AddPermission,
        [Description("Edytuj uprawnienie")]
        EditPermission,
        [Description("Akcje")]
        Actions,
        [Description("Dodaj akcje")]
        AddAction,
        [Description("Edytuj akcje")]
        EditAction,
    }

 }
