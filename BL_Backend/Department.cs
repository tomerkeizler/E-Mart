using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    [Serializable()]
    public class Department
    {
        //Fields:
        private string name;
        private int departmentID;

        //Constructors:
        public Department() { }
        public Department(string _name, int _id = 0)
        {
            name = _name;
            departmentID = _id;
        }
        //For Deep Copy
        public Department(Department other)
        {
            name = other.name;
            departmentID = other.departmentID;
        }
        public override bool Equals(object _other)
        {
            if (!(_other is Department)) return false;
            Department other = (Department)_other;
            return (this.departmentID == other.departmentID && this.name.Equals(other.name));
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
        public int DepartmentID
        {
            get { return departmentID; }
            set { departmentID = value; }
        }
    }
}
