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
            //First generate the new product ID
            List<Employee> Allemps = itsDAL.ReadFromFile(Elements.Employee).Cast<Employee>().ToList();
            int maxID = 0;
            foreach (Employee emp in Allemps)
            {
                if (emp.Id > maxID)
                    maxID = emp.Id;
            }
            //set the new ID
            ((Employee)e).Id = maxID++;
            //Add the new product to the system
            Allemps.Add((Employee)e);
            itsDAL.WriteToFile(Allemps.Cast<object>().ToList());
        }
        public void Remove(object e)
        {
            List<Employee> Allemps = itsDAL.ReadFromFile(Elements.Employee).Cast<Employee>().ToList();
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
        public void Edit(object oldE, object newE)
        {
            List<Employee> Allemps = itsDAL.ReadFromFile(Elements.Employee).Cast<Employee>().ToList();
            ((Employee)newE).Id = ((Employee)oldE).Id;
            Allemps.Remove((Employee)oldE);
            Allemps.Add((Employee)newE);
            itsDAL.WriteToFile(Allemps.Cast<object>().ToList());
        }

        public List<object> FindByName(string name, StringFields field)
        {
            List<object> result;
            if (name == null) throw new System.Data.DataException("Bad Input!");
            if (field == StringFields.firstName)
                result = itsDAL.EmployeeFirstNameQuery(name).Cast<object>().ToList();
            else if (field == StringFields.lastName)
                result = itsDAL.EmployeeLastNameQuery(name).Cast<object>().ToList();
            else
                throw new System.Data.DataException("Bad Input!");
            return result;
        }

        public List<object> FindByNumber(int number, IntFields field)
        {
            List<object> result;
            if (field == IntFields.id)
            {
                result = itsDAL.EmployeeIDQuery(number).Cast<object>().ToList();
            }
            if (field == IntFields.depID)
            {
                result = itsDAL.EmployeeDepartmentIDQuery(number).Cast<object>().ToList();
            }
            if (field == IntFields.salary)
            {
                result = itsDAL.EmployeeSalaryQuery(number).Cast<object>().ToList();
            }
            if (field == IntFields.supervisiorID)
            {
                result = itsDAL.EmployeesupervisiorIDQuery(number).Cast<object>().ToList();
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return result;
                                 
        }

        public List<object> FindByType(object type)
        {
            if (!(type is Gender))
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return itsDAL.EmployeeGenderQuery((Gender)type).Cast<object>().ToList();
        }

        public List<object> GetAll(Elements element)
        {
            return itsDAL.ReadFromFile(element);
        }
    }
}
