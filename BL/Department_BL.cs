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
        public object Add(object d)
        {
            List<Backend.Department> Alldeparts = itsDAL.ReadFromFile(Elements.Department).Cast<Backend.Department>().ToList();
             //Generate the new department ID
             int maxID = 0;
             foreach (Backend.Department depart in Alldeparts)
             {
                 if (depart.DepartmentID > maxID)
                     maxID = depart.DepartmentID;
                 if (((Backend.Department)d).DepartmentID != 0 && ((Backend.Department)d).DepartmentID == depart.DepartmentID)
                 {
                     throw new System.Data.DataException("The ID allready exist in the system");
                 }
             }
             if (((Backend.Department)d).DepartmentID == 0)
             {
                 //set the new ID
                 ((Backend.Department)d).DepartmentID = maxID + 1;
             }
            //Add the new department to the system
            Alldeparts.Add((Backend.Department)d);
            itsDAL.WriteToFile(Alldeparts.Cast<object>().ToList(), (Backend.Department)d);
            return d;
        }

        public void Remove(object d, Boolean isEdit = false)
        {
            List<Backend.Department> Alldeparts = itsDAL.ReadFromFile(Elements.Department).Cast<Backend.Department>().ToList();
            List<Backend.Employee> Allemps = itsDAL.ReadFromFile(Elements.Employee).Cast<Backend.Employee>().ToList();
            List<Backend.Product> Allprod = itsDAL.ReadFromFile(Elements.Product).Cast<Backend.Product>().ToList();
            //check if there are any departments to remove
            if (!Alldeparts.Any())
                throw new NullReferenceException("No Departments to remove!");
            else
            {
                //check if an employee is under this department
                foreach (Backend.Employee emp in Allemps)
                {
                    if (((Backend.Department)d).DepartmentID == emp.DepID)
                        throw new Exception("this department is currently in use!");
                }
                foreach (Backend.Product prod in Allprod)
                {
                    if (((Backend.Department)d).DepartmentID == prod.Location)
                        throw new Exception("this department is currently in use!");
                }
                //find and remove department
                foreach (Backend.Department depart in Alldeparts)
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
            //throw new FieldAccessException("not authorize!");
            //preserve the id for the edited department
            ((Backend.Department)newD).DepartmentID = ((Backend.Department)oldD).DepartmentID;
            this.Remove(oldD);
            this.Add(newD);
        }

        public List<object> FindByName(string name, Backend.StringFields field)
        {
            //search method by string
            if (name == null)
                throw new System.Data.DataException("Bad Input!");
            List<object> result = itsDAL.DepartmentNameQuery(name, field).Cast<object>().ToList();
            return result;
        }

        public List<object> FindByNumber(IntFields field, int minNumber, int maxNumber)
        {
            //search method by number
            return itsDAL.DepartmentNumberQuery(minNumber,maxNumber, field).Cast<object>().ToList();
        }

        public List<object> FindByType(ValueType type)
        {
            //search method by type
            throw new System.Data.DataException("transactions doesn't have types!");
        }

        public List<object> GetAll()
        {
            //return all departments
            return itsDAL.ReadFromFile(Elements.Department);
        }

        public Type GetEntityType()
        {
            //return the department type
            return typeof(Backend.Department);
        }
        public string GetEntityName()
        {
            //return the Department type as a string
            return "Department";
        }
    }
}
