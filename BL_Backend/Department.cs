using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class Department
    {
        //Fields:
        private string name;
        private int id;

        //Constructors:
        public Department(string _name, int _id = 0)
        {
            name = _name;
            id = _id;
        }
        public Department(Department other)
        {
            name = other.name;
            id = other.id;
        }
        public override bool Equals(object _other)
        {
            if (!(_other is Department)) return false;
            Department other = (Department)_other;
            return (this.id == other.id && this.name.Equals(other.name));
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ name.GetHashCode();
        }
        public override string ToString()
        {
            return name;
        }
        //getters and setters:
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}
