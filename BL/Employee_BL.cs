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
        public void addEmployee(Backend.Employee e)
        {
            //First find conflicts by ID
            List<Employee> AllEmps = itsDAL.GetAllEmployees();
            foreach (Employee emp in AllEmps)
            {
                if (emp.Id == e.Id)
                {
                    throw new System.Data.DuplicateNameException("The ID is already exist in the DB");
                }
            }
            itsDAL.AddEmployee(e);
        }

    }
}
