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
        public void addEmployee(Employee e)
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


        public void AddProduct(Product p)
        {
            throw new NotImplementedException();
        }

        public void RemoveProduct(Product p)
        {
            throw new NotImplementedException();
        }

        public void EditProduct(Product p)
        {
            throw new NotImplementedException();
        }

        public List<Product> FindProductByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<Product> FindProductByPrice(int price)
        {
            throw new NotImplementedException();
        }

        public List<Product> FindProductByID(int productID)
        {
            throw new NotImplementedException();
        }

        public List<Product> FindProductByLocation(int departID)
        {
            throw new NotImplementedException();
        }

        public List<Product> FindProductByType(PType type)
        {
            throw new NotImplementedException();
        }

        public void AddEmployee(Employee e)
        {
            throw new NotImplementedException();
        }

        public void RemoveEmployee(Employee e)
        {
            throw new NotImplementedException();
        }

        public void EditEmployee(Employee e)
        {
            throw new NotImplementedException();
        }

        public List<Employee> FindEmployeeByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        public List<Employee> FindEmployeeByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        public List<Employee> FindEmployeeByID(int id)
        {
            throw new NotImplementedException();
        }

        public List<Employee> FindEmployeeByDepartmentID(int depID)
        {
            throw new NotImplementedException();
        }

        public List<Employee> FindEmployeeBySalary(int Salary)
        {
            throw new NotImplementedException();
        }

        public List<Employee> FindEmployeeByGender(Gender gender)
        {
            throw new NotImplementedException();
        }
    }
}
