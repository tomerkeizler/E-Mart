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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;


namespace DAL
{
    public class LINQ_DAL : IDAL
    {
        //Fields:
        private List<object> p;
        private XmlSerializer SerializerObj;

        //Constuctors:
        public LINQ_DAL()
        {
            SerializerObj = new XmlSerializer(typeof(List<object>), new Type[] { typeof(Product) });
        }

        //Behaviour:
        public static XmlDocument SerializeToXmlDoc(List<object> list)
        {
            DataContractSerializer dcs = new DataContractSerializer(typeof(List<object>));
            MemoryStream ms = new MemoryStream();
            dcs.WriteObject(ms, list);
            ms.Position = 0;
            XmlDocument doc = new XmlDocument();
            doc.Load(ms);
            return doc;
        }
        public void WriteToFile(List<object> list)
        {
            TextWriter WriteFileStream = new StreamWriter(list.ElementAt(0).GetType()+".xml");
            SerializerObj.Serialize(WriteFileStream, list);
            WriteFileStream.Close();
        }

        public List<object> ReadFromFile(Elements element)
        {
            if (File.Exists("Backend."+element.ToString()+".xml"))
            {
                using (FileStream stream = File.OpenRead("Backend."+element.ToString()+".xml"))
               {
                return (List<object>)SerializerObj.Deserialize(stream);
               }
            }
            else
            {
                return new List<object>();
            }
        }
        
        public void AddProduct(Backend.Product p)
        {
            this.p.Add(p);
        }

        public List<Product> ProductNameQuery(string name)
        {
            //perform query
            var results = from Product p in this.p
                          where p.Name == name
                          select p;
            //return results
            return results.ToList();
        }

        public List<object> GetAllProducts()
        {
            return p;
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


        public List<Product> ProductStockCountQuery(int stockCount)
        {
            throw new NotImplementedException();
        }


        public List<Employee> EmployeesupervisiorIDQuery(int superID)
        {
            throw new NotImplementedException();
        }
    }
}
