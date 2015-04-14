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
            //First find conflicts by ID
            List<Employee> AllEmps = itsDAL.GetAllEmployees();
            foreach (Employee emp in AllEmps)
            {
                if (emp.Id == ((Employee)e).Id)
                {
                    throw new System.Data.DuplicateNameException("The ID is already exist in the DB");
                }
            }
            itsDAL.AddEmployee((Employee)e);
        }
        public void Remove(object obj)
        {
            itsDAL.RemoveEmployee((Employee)obj);
        }
        public void Edit(object oldObj, object newObj)
        {
            itsDAL.RemoveEmployee((Employee)oldObj);
            this.Add(newObj);
        }

        public List<object> FindByName(string name, StringFields field)
        {
            List<object> result;
            if (name == null) throw new System.Data.DataException("Bad Input!");
            if (field == StringFields.firstName)
                result = itsDAL.EmployeeFirstNameQuery(name).Cast<object>().ToList();
            else if (field == StringFields.lastName)
            {
                result = itsDAL.EmployeeLastNameQuery(name).Cast<object>().ToList();
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
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
    }
}
