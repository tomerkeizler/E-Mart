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
        public void EditProduct(Product oldp, Product newp)
        {
            int tempID = oldp.ProductID;
            itsDAL.RemoveProduct(oldp);
            itsDAL.AddProduct(newp);
            newp.ProductID = tempID;
        }
        public List<Product> FindProductByName(string name)
        {
            if (name == null)
                throw new System.Data.DataException("Bad Input!");
            return itsDAL.ProductNameQuery(name);
        }


        public List<Product> FindProductByPrice(int price)
        {
            if (price == null)
                throw new System.Data.DataException("Bad Input!");
            return itsDAL.ProductPriceQuery(price);
        }

        public List<Product> FindProductByID(int productID)
        {
            if (productID == null)
                throw new System.Data.DataException("Bad Input!");
            return itsDAL.ProductIDQuery(productID);
        }

        public List<Product> FindProductByLocation(int departID)
        {
            if (departID == null)
                throw new System.Data.DataException("Bad Input!");
            return itsDAL.ProductLocationQuery(departID);
        }
        public List<Product> FindProductByType(PType type)
        {
            if (type == null)
                throw new System.Data.DataException("Bad Input!");
            return itsDAL.ProductTypeQuery(type);
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


        public void EditProduct(Product p)
        {
            throw new NotImplementedException();
        }
    }
}
