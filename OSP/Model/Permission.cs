using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace OSP.Model
{
    /// <summary>
    /// Klasa będąca modelem uprawnień
    /// </summary>
   [Serializable]
   public class Permission
    {
        #region Fields

        #endregion

        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        #endregion

        #region Constructors

        public Permission()
        {
        }

        public Permission(int id)
        {
            Id = id;
        }

        public Permission(int id, string name)
        {
            Id = id;
            Name = name;
        }
        #endregion

        #region Methods
        public Permission Clone()
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return (Permission)formatter.Deserialize(stream);
            }
        }
        #endregion
    }
}