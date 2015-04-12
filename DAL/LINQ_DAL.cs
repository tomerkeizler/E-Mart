using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;


namespace DAL
{
    public class LINQ_DAL : IDAL
    {
        public List<Product> DB;
        XmlSerializer serializerP = new XmlSerializer(typeof(List<Product>));
        //XmlSerializer serializerE = new XmlSerializer(typeof(List<Employee>));
        RijndaelManaged key = null;

        public LINQ_DAL()
        {


            List<Product> p = new List<Product>();
            p.Add(new Product("beans", PType.a, 1, PStatus.Empty, 1, 12, 2));
            key = new RijndaelManaged();
            WriteToFile(p);
            /*
            p.Add(new Product("beans", PType.a, 1, PStatus.Empty, 1, 12, 2));
            DB.Add(new Product("corn", "food", 1));
            DB.Add(new Product("scale", "food", 2));
            DB.Add(new Product("tv", "electronics", 3));
            DB.Add(new Product("scale", "electronics", 4));
            DB.Add(new Product("corn", "food", 5));
            DB.Add(new Product("shirt", "clothes", 6));
            DB.Add(new Product("pants", "clothes", 7));
            */
        }

        public void WriteToFile(Object list)
        {
            using (FileStream stream = File.OpenWrite(list.GetType()+"XML.xml"))
            {
                serializerP.Serialize(stream, list);
            }
        }

        public List<Object> ReadFromFile(List<Object> p)
        {
            using (FileStream stream = File.OpenRead("ProductXML.xml"))
            {
                return (List<Object>)serializerP.Deserialize(stream);
            }
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
