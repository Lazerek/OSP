using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace OSP.Model
{
    /// <summary>
    /// Klasa będąca modelem kursu
    /// </summary>
    [Serializable]
    public class Course
    {
        #region Fields

        #endregion

        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        #endregion

        #region Constructors

        public Course()
        {
        }

        public Course(int id)
        {
            Id = id;
        }

        public Course(int id, string name)
        {
            Id = id;
            Name = name;
        }
        #endregion

        #region Methods
        public Course Clone()
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return (Course)formatter.Deserialize(stream);
            }
        }
        #endregion
    }
}