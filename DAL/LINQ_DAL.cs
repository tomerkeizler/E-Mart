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
        public List<Product> p;
        private XmlSerializer serializerP = new XmlSerializer(typeof(List<Product>));
        //XmlSerializer serializerE = new XmlSerializer(typeof(List<Employee>));
        TripleDESCryptoServiceProvider tDESkey;

        public LINQ_DAL()
        {


            p = new List<Product>();
            //p.Add(new Product("beans", PType.a, 1, PStatus.Empty, 1, 12, 2));
            tDESkey =  new TripleDESCryptoServiceProvider();
            //WriteToFile(p);
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
        public static XmlDocument SerializeToXmlDoc(List<Product> list)
        {
            DataContractSerializer dcs = new DataContractSerializer(typeof(List<Product>));
            MemoryStream ms = new MemoryStream();
            dcs.WriteObject(ms, list);
            ms.Position = 0;
            XmlDocument doc = new XmlDocument();
            doc.Load(ms);
            return doc;
        }
        private XmlDocument encrypt(XmlDocument list)
        {
            /*BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            formatter.Serialize(memStream, list);
            byte[] listBytes = memStream.ToArray();*/
            
            return list;
        }
        public void WriteToFile(List<Product> list)
        {
            XmlDocument myXML = SerializeToXmlDoc(list);
            //myXML = encrypt(myXML);
            using (FileStream stream = File.OpenWrite(myXML.GetType() + "XML.xml"))
            {
                //serializerP.Serialize(stream, myXML);
                myXML.Save(stream);
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

        public List<Backend.Product> GetAllProducts()
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
            //throw new NotImplementedException();
            Console.WriteLine("yaaay");
            Console.Read();
            return new List<Product>();
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
    }
}
