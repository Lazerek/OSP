using System.ComponentModel;
using System.Runtime.CompilerServices;
using OSP.Annotations;

namespace OSP.Model
{
    public class ListModel : INotifyPropertyChanged
    {
        private bool isSelected;
        private int id;
        private string name;
        private int quantity = 0;
        private string name2;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
                    }
                }
            }
        }

        public int Quantity
        {
            get { return quantity; }
            set
            {
                value = quantity;
            }
        }
        public void SetSelect()
        {
            isSelected = true;
        }

        public void UnsetSelect()
        {
            isSelected = false;
        }

        public ListModel()
        {
            
        }

        public ListModel(int myob)
        {
            id = myob;

        }

        public ListModel(string name)
        {
            this.name = name;
        }

        public ListModel(int myob, string name)
        {
            this.id = myob;
            this.name = name;
        }

        public ListModel(int myob, string name, int quantity)
        {
            this.id = myob;
            this.name = name;
            this.quantity = quantity;
        }

        public ListModel(int myon, string name, string name2)
        {
            id = myon;
            this.name = name;
            this.name2 = name2;
        }
        public int Id
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
        }

        public string Name2
        {
            get { return name2; }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}