using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace OSP.Model
{
    /// <summary>
    /// Klasa będąca modelem typu akcji
    /// </summary>
    [Serializable]
    public class ActionType
    {
        #region Fields


        #endregion

        #region Properties
        public int Id { get; set; }
        public string Type { get; set; }
        #endregion

        #region Constructors

        public ActionType()
        {
           
        }

        public ActionType(int id)
        {
            Id = id;
        }

        public ActionType(int id, string type)
        {
            Id = id;
            Type = type;
        }
        #endregion

        #region Methods
        public ActionType Clone()
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return (ActionType)formatter.Deserialize(stream);
            }
        }
        #endregion
    }
}