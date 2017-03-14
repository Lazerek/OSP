namespace OSP.Model
{
    public class RaportList
    {
        private string type;
        private int number;
        private int id;

        public string Type
        {
            get { return type;}
            set
            {
                type = value;
            }
        }

        public int Number
        {
            get { return number;}
            set { number = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public RaportList()
        {
            
        }

        public RaportList(int id)
        {
            this.id = id;
            number = 0;
        }


        public RaportList(int id, string type)
        {
            this.id = id;
            this.type = type;
            number = 0;
        }

        public void Increase()
        {
            number++;
        }
    }
}