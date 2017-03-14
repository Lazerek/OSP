using System;
using System.ComponentModel;
using System.Data.SQLite;
using System.Windows;

namespace OSP.Model
{
    /// <summary>
    /// Klasa z metodami pomocniczymi wykorzystywanych w modelach oraz dotyczących enumeratorów
    /// </summary>
    public class Helpers
    {
        private SQLiteConnection con;
        private string connectionString =
            @"Data Source=C:\Users\Lazerek\Documents\Visual Studio 2015\Projects\OSP\OSP\OSPDatabase.db; Version=3";
       
        public static string GetDescription(object name)
        {
            System.Reflection.FieldInfo oFieldInfo = name.GetType().GetField(name.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])oFieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : name.ToString();
        }

    }
}