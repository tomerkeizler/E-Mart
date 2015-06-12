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
        User_BL itsUserBL;

        //Constructors:
        public Employee_BL(IDAL dal)
        {
            itsDAL = dal;
            itsUserBL = new User_BL(itsDAL);
        }

        //Methods:
        public object Add(object e)
        {
            //Add the new employee to the system
            List<Backend.Employee> Allemps = itsDAL.ReadFromFile(Elements.Employee).Cast<Backend.Employee>().ToList();
            List<Backend.Department> Alldeparts = itsDAL.ReadFromFile(Elements.Department).Cast<Backend.Department>().ToList();
            bool checkID = false;
            bool checkSup = false;
            //check id the employee's department accually exists
            foreach (Backend.Department dep in Alldeparts)
            {
                if (((Backend.Employee)e).DepID == dep.DepartmentID)
                {
                    checkID = true;
                    break;
                }
            }
            if (!checkID)
                throw new Exception("department ID doesn't exist!");
            else
            {
                foreach (Backend.Employee emp in Allemps)
                {
                    if (emp.Equals(e))
                        throw new Exception("employee is already exists!");
                    if (emp.Id == ((Backend.Employee)e).Id)
                    {
                        throw new Exception("This employee have duplicate ID with another employee!");
                    }
                    if (((Backend.Employee)e).SupervisiorID == emp.Id)
                    {
                        checkSup = true;
                        if ((emp.SupervisiorID != 0))
                        {
                            Backend.User oldUsr = itsDAL.UserPersonQuery(emp).ElementAt(0);
                            emp.Rank = Rank.Manager;
                            Backend.User newUser = new Backend.User(oldUsr);
                            newUser.Person = emp;
                            itsUserBL.Edit(oldUsr, newUser);
                        }
                    }
                    if (emp.SupervisiorID == ((Backend.Employee)e).Id)
                    {
                        Backend.User oldUsr = itsDAL.UserPersonQuery(((Backend.Employee)e)).ElementAtOrDefault(0);
                        if (oldUsr != null)
                        {
                            ((Backend.Employee)e).Rank = Rank.Manager;
                            Backend.User newUser = new Backend.User(oldUsr);
                            newUser.Person = ((Backend.Employee)e);
                            itsUserBL.Edit(oldUsr, newUser);
                        }
                    }
                }
                if (((Backend.Employee)e).SupervisiorID == 0)
                {
                    checkSup = true;
                    ((Backend.Employee)e).Rank = Rank.Administrator;
                }
                if (!checkSup)
                {
                    throw new Exception("his supervisor doesn't exists!");
                }
                Allemps.Add((Backend.Employee)e);
            }
            itsDAL.WriteToFile(Allemps.Cast<object>().ToList(), (Backend.Employee)e);
            return e;
        }
        public void Remove(object e, Boolean isEdit = false)
        {
            List<Backend.Employee> Allemps = itsDAL.ReadFromFile(Elements.Employee).Cast<Backend.Employee>().ToList();
            List<Backend.User> Allusers = itsDAL.ReadFromFile(Elements.User).Cast<Backend.User>().ToList();
            bool hasMoreEmployees = false;
            object temp = new Backend.Employee();
            Backend.User tempUser = new Backend.User();
            if (!Allemps.Any())
                throw new NullReferenceException("No Employees to remove!");
            if (!isEdit)
            {
                //check if this employee has workers under him
                foreach (Backend.Employee emp in Allemps)
                {
                    if (((Backend.Employee)e).Id == emp.SupervisiorID)
                        throw new Exception("this employee has worker under him!");
                }
            }
            foreach (Backend.User user in Allusers)
            {
                if (user.Person.Equals(e))
                {
                    Allusers.Remove(user);
                    break;
                }
            }
            foreach (Backend.Employee emp in Allemps)
            {
                if (emp.Equals(e))
                {
                    Allemps.Remove(emp);
                    break;
                }
            }
            foreach (Backend.Employee emp in Allemps)
            {
                if (((Backend.Employee)e).SupervisiorID == emp.SupervisiorID)
                    hasMoreEmployees = true;
                if (((Backend.Employee)e).SupervisiorID == emp.Id)
                {
                    temp = emp;
                    tempUser = itsDAL.UserPersonQuery(temp).ElementAtOrDefault(0);
                }
            }
            if (!hasMoreEmployees && ((Backend.Employee)temp).SupervisiorID != 0 && ((Backend.Employee)temp).SupervisiorID != -1)
            {
                ((Backend.Employee)temp).Rank = Rank.Worker;
                Backend.User newUser = new Backend.User(tempUser);
                newUser.Person = temp;
                Allusers.Remove(tempUser);
                Allusers.Add(newUser);
            }

            itsDAL.WriteToFile(Allusers.Cast<object>().ToList(), new Backend.User());
            itsDAL.WriteToFile(Allemps.Cast<object>().ToList(), (Backend.Employee)e);
        }   
        public void Edit(object oldE, object newE)
        {
            if (((Backend.Employee)oldE).Id == -1)
                throw new UnauthorizedAccessException("can't edit default administrator");
            List<Backend.User> oldUserList = itsDAL.UserPersonQuery(oldE);
            Backend.User oldUser = oldUserList.ElementAtOrDefault(0);
            if (oldUser == null)
            {
                throw new NullReferenceException("The employee does not exist!");
            }
            if (((Backend.Employee)oldE).Id != ((Backend.Employee)newE).Id)
            {
                List<Backend.Employee> Allemps = itsDAL.ReadFromFile(Elements.Employee).Cast<Backend.Employee>().ToList();
                foreach (Backend.Employee emp in Allemps)
                {
                    if (((Backend.Employee)oldE).Id == emp.SupervisiorID)
                        throw new Exception("Cant Change ID, this employee has worker under him!");
                }
            }
            if (((Backend.Employee)newE).SupervisiorID != 0)
            {
                ((Backend.Employee)newE).Rank = Rank.Worker;
            }
            Backend.User newUser = new Backend.User(oldUser);
            this.Remove(oldE,true);
            newUser.Person = this.Add(newE);
            itsUserBL.Add(newUser);
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

        public List<Backend.Employee> GetAllWorkers(List<Backend.Employee> emps, int supervisorId)
        {
            List<Backend.Employee> allWorkers = new List<Backend.Employee>();
            foreach (Backend.Employee e in emps)
            {
                if (e.SupervisiorID == supervisorId)
                    allWorkers.Add(e);
            }
            return allWorkers;  
        }

        public Type GetEntityType()
        {
            return typeof(Backend.Employee);
        }
        public string GetEntityName()
        {
            //return the Employee type as a string
            return "Employee";
        }
    }
}
