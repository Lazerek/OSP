using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace OSP.Model
{
    /// <summary>
    /// Klasa będąca modelem akcji
    /// </summary>
    [Serializable]
    public class Action
    {
        #region Fields
        #endregion

        #region Properties
        public int Id { get; set; }
        public string PlaceOfAction { get; set; }
        public string StartOfAction { get; set; }
        public string EndOfAction { get; set; }
        public int MinutesOfAction { get; set; }
        public int NumberOfAction { get; set; }
        public string DateOfAction { get; set; }

        public ActionType ActionType { get; set; }

        public int NumberOfKM { get; set; }

        #endregion

        #region Constructors

        public Action()
        {
        }

        public Action(int id)
        {
            Id = id;
        }

        public Action(int id, int numberOfActions, string dateOfAction, int minutesOfAction, string placeOfAction,  
            int numberOfKM, ActionType actionType)
        {
            Id = id;
            PlaceOfAction = placeOfAction;
            MinutesOfAction = minutesOfAction;
            NumberOfKM = numberOfKM;
            ActionType = actionType;
            NumberOfAction = numberOfActions;
            DateOfAction = dateOfAction;
        }
        #endregion

        #region Methods
        public Action Clone()
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return (Action)formatter.Deserialize(stream);
            }
        }
        #endregion

    }
}