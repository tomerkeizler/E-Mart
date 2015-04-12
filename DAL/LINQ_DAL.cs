using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend;

namespace DAL
{
    public class LINQ_DAL : IDAL
    {
        public List<Product> DB;

        public LINQ_DAL()
        {
            DB = new List<Product>();
            /*
            DB.Add(new Product("beans", "food", 0));
            DB.Add(new Product("corn", "food", 1));
            DB.Add(new Product("scale", "food", 2));
            DB.Add(new Product("tv", "electronics", 3));
            DB.Add(new Product("scale", "electronics", 4));
            DB.Add(new Product("corn", "food", 5));
            DB.Add(new Product("shirt", "clothes", 6));
            DB.Add(new Product("pants", "clothes", 7));
             */
        }

        public void AddProduct(Backend.Product p)
        {
            DB.Add(p);
        }

        public List<Backend.Product> ProductNameQuery(string name)
        {
            //perform query
            var results = from Product p in DB
                          where p.Name == name
                          select p;
            //return results
            return results.ToList();
        }

        public List<Backend.Product> GetAllProducts()
        {
            return DB;
        }


        public void RemoveProduct(Product p)
        {
            throw new NotImplementedException();
        }

        public void EditProduct(Product p)
        {
            throw new NotImplementedException();
        }

        public List<Product> ProductIDQuery(int id)
        {
            throw new NotImplementedException();
        }

        public List<Product> ProductTypeQuery(PType type)
        {
            throw new NotImplementedException();
        }

        public List<Product> ProductLocationQuery(int departID)
        {
            throw new NotImplementedException();
        }

        public List<Product> ProductPriceQuery(int price)
        {
            throw new NotImplementedException();
        }

        public List<Employee> GetAllEmployees()
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

        public List<Employee> EmployeeFirstNameQuery(string firstName)
        {
            throw new NotImplementedException();
        }

        public List<Employee> EmployeeLastNameQuery(string lastName)
        {
            throw new NotImplementedException();
        }

        public List<Employee> EmployeeIDQuery(int id)
        {
            throw new NotImplementedException();
        }

        public List<Employee> EmployeeSalaryQuery(int salary)
        {
            throw new NotImplementedException();
        }

        public List<Employee> EmployeeDepartmentIDQuery(int depID)
        {
            throw new NotImplementedException();
        }

        public List<Employee> EmployeeGenderQuery(Gender gender)
        {
            throw new NotImplementedException();
        }
    }
}
