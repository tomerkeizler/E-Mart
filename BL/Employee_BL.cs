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
            bool checkSup = false;
            foreach (Department dep in Alldeparts)
            {
                if (((Employee)e).DepID == dep.DepartmentID)
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
                    if (((Employee)e).SupervisiorID == emp.SupervisiorID)
                        checkSup = true;
                }
                if(checkSup)
                Allemps.Add((Employee)e);
                else
                    throw new Exception("his supervisor doesn't exists!");
            }
            itsDAL.WriteToFile(Allemps.Cast<object>().ToList(), (Employee)e);
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
                itsDAL.WriteToFile(Allemps.Cast<object>().ToList(), e);
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

        public List<object> FindByNumber(IntFields field, int minNumber, int maxNumber)
        {
            return itsDAL.EmployeeNumberQuery(minNumber,maxNumber, field).Cast<object>().ToList();                     
        }

        public List<object> FindByType(ValueType type)
        {
            return itsDAL.EmployeeTypeQuery(type).Cast<object>().ToList();
        }

        public List<object> GetAll()
        {
            return itsDAL.ReadFromFile(Elements.Employee);
        }

        public Type GetEntityType()
        {
            return typeof(Employee);
        }
    }
}
