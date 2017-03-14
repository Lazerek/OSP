using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace OSP.Model
{
    /// <summary>
    /// Klasa będąca modelem strażaka
    /// </summary>
    [Serializable]
    public class Firefighter
    {
        #region Fields
        #endregion

        #region Properties
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Pesel { get; set; }
        public string Phone { get; set; }
        public string Degree { get; set; }
        public string BirthDate { get; set; }
        public string JoinDate { get; set; }
        #endregion

        #region Constructors
        public Firefighter()
        {

        }

        public Firefighter(int id)
        {
            Id = id;
        }
        public Firefighter(int id, string lastName, string firstName, string pesel, string phone, string degree, string birthTime, string joinDate)
        {
            Id = id;
            LastName = lastName;
            FirstName = firstName;
            Pesel = pesel;
            Phone = phone;
            Degree = degree;
            BirthDate = birthTime;
            JoinDate = joinDate;
        }

        #endregion



        #region Methods
        public Firefighter Clone()
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return (Firefighter)formatter.Deserialize(stream);
            }
        }
        #endregion
    }
}