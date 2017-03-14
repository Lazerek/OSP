using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace OSP.Model
{
    /// <summary>
    /// Klasa będąca modelem wozu
    /// </summary>
    [Serializable]
    public class Car
    {
        #region Fields
        #endregion

        #region Properties
        public int Id { get; set; }
        public string CarNumber { get; set; }
        public string CarType { get; set; }
        public int WaterVolume { get; set; }
        #endregion

        #region Constructors

        public Car()
        {
        }


        public Car(int id)
        {
            Id = id;
        }
        public Car(int id, string carNumber, string carType, int waterVolume)
        {
            Id = id;
            CarNumber = carNumber;
            CarType = carType;
            WaterVolume = waterVolume;
        }

        #endregion

        #region Methods
        public Car Clone()
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return (Car)formatter.Deserialize(stream);
            }
        }
        #endregion
    }
}