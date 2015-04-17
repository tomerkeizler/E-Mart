using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend;
using DAL;

namespace BL
{
    class Department_BL : IBL
    {
        //Fields:
        IDAL itsDAL;

        //Constructors:
        public Department_BL(IDAL dal)
        {
            itsDAL = dal;
        }

        //Methods:
        public void Add(object d)
        {
            //First generate the new department ID
            List<Department> Alldeparts = itsDAL.ReadFromFile(Elements.Department).Cast<Department>().ToList();
            int maxID = 0;
            foreach (Department depart in Alldeparts)
            {
                if (depart.Id > maxID)
                    maxID = depart.Id;
            }
            //set the new ID
            ((Department)d).Id = maxID++;
            //Add the new department to the system
            Alldeparts.Add((Department)d);
            itsDAL.WriteToFile(Alldeparts.Cast<object>().ToList());
        }

        public void Remove(object d)
        {
            List<Department> Alldeparts = itsDAL.ReadFromFile(Elements.Department).Cast<Department>().ToList();
            if (!Alldeparts.Any())
                throw new NullReferenceException("No Departments to remove!");
            else
            {
                foreach (Department depart in Alldeparts)
                {
                    if (depart.Equals(d))
                    {
                        Alldeparts.Remove(depart);
                        break;
                    }
                }
                itsDAL.WriteToFile(Alldeparts.Cast<object>().ToList());
            }
        }

        public void Edit(object oldD, object newD)
        {
            List<Department> Alldeparts = itsDAL.ReadFromFile(Elements.Department).Cast<Department>().ToList();
            ((Department)newD).Id = ((Department)oldD).Id;
            Alldeparts.Remove((Department)oldD);
            Alldeparts.Add((Department)newD);
            itsDAL.WriteToFile(Alldeparts.Cast<object>().ToList());
        }

        public List<object> FindByName(string name, Backend.StringFields field)
        {
            if (name == null)
                throw new System.Data.DataException("Bad Input!");
            List<object> result = itsDAL.DepartmentNameQuery(name, field).Cast<object>().ToList();
            return result;
        }

        public List<object> FindByNumber(int number, Backend.IntFields field)
        {
            return itsDAL.DepartmentNumberQuery(number, field).Cast<object>().ToList();
        }

        public List<object> FindByType(ValueType type)
        {
            throw new System.Data.DataException("transactions doesn't have types!");
        }

        public List<object> GetAll(Backend.Elements element)
        {
            return itsDAL.ReadFromFile(element);
        }
    }
}
