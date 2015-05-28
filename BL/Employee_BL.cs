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
            //check id the employee's department accually exists
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
                    if ((emp.Id != 0) && ((Employee)e).SupervisiorID == emp.Id)
                    {
                        checkSup = true;
                        emp.Rank = Rank.Manager;
                    }
                }
                if (((Employee)e).SupervisiorID == 0)
                {
                    ((Employee)e).Rank = Rank.Administrator;
                    checkSup = true;
                }
                if (!checkSup)
                {
                    throw new Exception("his supervisor doesn't exists!");
                }
                Allemps.Add((Employee)e);
            }
            itsDAL.WriteToFile(Allemps.Cast<object>().ToList(), (Employee)e);
        }
        public void Remove(object e)
        {
            List<Employee> Allemps = itsDAL.ReadFromFile(Elements.Employee).Cast<Employee>().ToList();
            bool hasMoreEmployees = false;
            Employee temp = new Employee();
            if (!Allemps.Any())
                throw new NullReferenceException("No Employees to remove!");
             //check if an employee is under this department
                foreach (Employee emp in Allemps)
                {
                    if (((Employee)e).Id == emp.SupervisiorID)
                        throw new Exception("this employee has worker under him!");
                }
                foreach (Employee emp in Allemps)
                {
                    if (emp.Equals(e))
                    {
                        Allemps.Remove(emp);
                    }
                    else if (((Employee)e).SupervisiorID == emp.SupervisiorID)
                        hasMoreEmployees = true;
                    if (((Employee)e).SupervisiorID == emp.Id)
                        temp = emp;
                }
                if (!hasMoreEmployees)
                    temp.Rank = Rank.Worker;
                itsDAL.WriteToFile(Allemps.Cast<object>().ToList(), e);
        }
        public void Edit(object oldE, object newE)
        {
            List<User> oldUserList = itsDAL.UserPersonQuery(oldE);
            User oldUser = oldUserList.ElementAtOrDefault(0);
            if (oldUser == null)
            {
                throw new NullReferenceException("The customer does not exist!");
            }
            User_BL itsUserBL = new User_BL(itsDAL);
            User newUser = new User(oldUser);
            newUser.Person = newE;
            itsUserBL.Edit(oldUser, newUser);
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
