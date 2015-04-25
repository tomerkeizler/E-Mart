using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend;
using DAL;

namespace BL
{
    public class Department_BL : IBL
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
            List<Department> Alldeparts = itsDAL.ReadFromFile(Elements.Department).Cast<Department>().ToList();
             //Generate the new department ID
             int maxID = 0;
             foreach (Department depart in Alldeparts)
             {
                 if (depart.Id > maxID)
                     maxID = depart.Id;
                 if (((Department)d).Id != 0 && ((Department)d).Id == depart.Id)
                 {
                     throw new System.Data.DataException("The ID allready exist in the system");
                 }
             }
             if (((Department)d).Id == 0)
             {
                 //set the new ID
                 ((Department)d).Id = maxID + 1;
             }
            //Add the new department to the system
            Alldeparts.Add((Department)d);
            itsDAL.WriteToFile(Alldeparts.Cast<object>().ToList(), (Department)d);
        }

        public void Remove(object d)
        {
            List<Department> Alldeparts = itsDAL.ReadFromFile(Elements.Department).Cast<Department>().ToList();
            List<Employee> Allemps = itsDAL.ReadFromFile(Elements.Employee).Cast<Employee>().ToList();
            if (!Alldeparts.Any())
                throw new NullReferenceException("No Departments to remove!");
            else
            {
                foreach (Employee emp in Allemps)
                {
                    if (((Department)d).Id == emp.DepID)
                        throw new Exception("this department is currently in use!");
                }
                foreach (Department depart in Alldeparts)
                {
                    if (depart.Equals(d))
                    {
                            Alldeparts.Remove(depart);
                            break;
                    }
                }
                itsDAL.WriteToFile(Alldeparts.Cast<object>().ToList(), d);
            }
        }

        public void Edit(object oldD, object newD)
        {
            ((Department)newD).Id = ((Department)oldD).Id;
            this.Remove(oldD);
            this.Add(newD);
        }

        public List<object> FindByName(string name, Backend.StringFields field)
        {
            if (name == null)
                throw new System.Data.DataException("Bad Input!");
            List<object> result = itsDAL.DepartmentNameQuery(name, field).Cast<object>().ToList();
            return result;
        }

        public List<object> FindByNumber(IntFields field, int minNumber, int maxNumber)
        {
            return itsDAL.DepartmentNumberQuery(minNumber,maxNumber, field).Cast<object>().ToList();
        }

        public List<object> FindByType(ValueType type)
        {
            throw new System.Data.DataException("transactions doesn't have types!");
        }

        public List<object> GetAll()
        {
            return itsDAL.ReadFromFile(Elements.Department);
        }
    }
}
