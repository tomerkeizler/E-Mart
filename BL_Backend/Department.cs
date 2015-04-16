using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_Backend
{
    class Department
    {
        //Fields:
        private string name;
        private int id;

        //Constructors:
        public Department(string _name, int _id)
        {
            name = _name;
            id = _id;
        }
        public string ToString()
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
