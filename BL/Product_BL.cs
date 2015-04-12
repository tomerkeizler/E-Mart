using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend;
using DAL;

namespace BL
{
    public class Product_BL : IBL
    {
        //Fields:
        IDAL itsDAL;

        //Constructors:
        public Product_BL(IDAL dal)
        {
            itsDAL = dal;
        }

        public void AddProduct(Backend.Product p)
        {
            //First generate the new product ID
            List<Product> Allprods = itsDAL.GetAllProducts();
            int maxID = 0;
            foreach (Product prod in Allprods)
            {
                if (prod.ProductID > maxID)
                    maxID = prod.ProductID;
            }
            //set the new ID
            p.ProductID = maxID++;
            //Add the new product to the system
            itsDAL.AddProduct(p);
        }
        public void RemoveProduct(Product p)
        {
            itsDAL.RemoveProduct(p);
        }
        public void EditProduct(Product p)
        {

        }

        public List<Product> FindProductByName(string name)
        {
            return itsDAL.ProductNameQuery(name);
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
