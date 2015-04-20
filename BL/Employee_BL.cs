using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend;
using DAL;

namespace BL
{
    public class Employee_BL : IBL
    {
        //Fields:
        IDAL itsDAL;

        //Constructors:
        public Employee_BL(IDAL dal)
        {
            itsDAL = dal;
        }

        //Methods:
        public void Add(object e)
        {
            //Add the new employee to the system
            List<Employee> Allemps = itsDAL.ReadFromFile(Elements.Employee).Cast<Employee>().ToList();
            List<Department> Alldeparts = itsDAL.ReadFromFile(Elements.Department).Cast<Department>().ToList();
            bool checkID = false;
            foreach (Department dep in Alldeparts)
            {
                if (((Employee)e).DepID == dep.Id)
                {
                    checkID = true;
                    break;
                }
            }
            if (!checkID)
                throw new Exception("department ID doesn't exist!");
            else
            {
                foreach (Employee emp in Allemps)
                {
                    if (emp.Equals(e))
                        throw new Exception("employee is already exists!");
                }
                Allemps.Add((Employee)e);
            }
            itsDAL.WriteToFile(Alldeparts.Cast<object>().ToList());
            itsDAL.WriteToFile(Allemps.Cast<object>().ToList());
        }
        public void Remove(object e)
        {
            List<Employee> Allemps = itsDAL.ReadFromFile(Elements.Employee).Cast<Employee>().ToList();
            if (!Allemps.Any())
                throw new NullReferenceException("No Employees to remove!");
            else
            {
                foreach (Employee emp in Allemps)
                {
                    if (emp.Equals(e))
                    {
                        Allemps.Remove(emp);
                        break;
                    }
                }
                itsDAL.WriteToFile(Allemps.Cast<object>().ToList());
            }
        }
        public void Edit(object oldE, object newE)
        {
            this.Remove(oldE);
            this.Add(newE);            
        }

        public List<object> FindByName(string name, StringFields field)
        {
            if (name == null)
                throw new System.Data.DataException("Bad Input!");
            List<object> result = itsDAL.EmployeeNameQuery(name, field).Cast<object>().ToList();
            return result;
        }

        public List<object> FindByNumber(int number, IntFields field)
        {
            return itsDAL.EmployeeNumberQuery(number, field).Cast<object>().ToList();                     
        }

        public List<object> FindByType(ValueType type)
        {
            return itsDAL.EmployeeTypeQuery(type).Cast<object>().ToList();
        }

        public List<object> GetAll()
        {
            return itsDAL.ReadFromFile(Elements.Employee);
        }
    }
}
