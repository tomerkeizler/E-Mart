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
using System.Reflection;


namespace DAL
{
    public class LINQ_DAL : IDAL
    {
        //Fields:
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
            if (list.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("Nothing to Write");
            }
            TextWriter WriteFileStream = new StreamWriter(list.ElementAtOrDefault(0).GetType() + ".xml");
            SerializerObj.Serialize(WriteFileStream, list);
            WriteFileStream.Close();
        }

        public List<object> ReadFromFile(Elements element)
        {
            if (File.Exists("Backend." + element.ToString() + ".xml"))
            {
                using (FileStream stream = File.OpenRead("Backend." + element.ToString() + ".xml"))
                {
                    return (List<object>)SerializerObj.Deserialize(stream);
                }
            }
            else
            {
                return new List<object>();
            }
        }
        public List<Product> ProductNameQuery(string name, StringFields field)
        {
            List<Product> allProducts = ReadFromFile(Elements.Product).Cast<Product>().ToList();
            List<Product> filteredProducts;
            if (allProducts.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (field != StringFields.name)
            {
                throw new System.Data.DataException("Bad Input!");
            }
            filteredProducts = allProducts.Where(n => n.Name.Equals(name)).Cast<Product>().ToList();
            return filteredProducts;
        }


        public List<Product> ProductNumberQuery(int number, IntFields field)
        {
            List<Product> allProducts = ReadFromFile(Elements.Product).Cast<Product>().ToList();
            List<Product> filteredProducts;
            if (allProducts.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (field == IntFields.price)
            {
                filteredProducts = allProducts.Where(n => n.Price <= number).Cast<Product>().ToList();
            }
            else if (field == IntFields.productID)
            {
                filteredProducts = allProducts.Where(n => n.ProductID == number).Cast<Product>().ToList();
            }
            else if (field == IntFields.location)
            {
                filteredProducts = allProducts.Where(n => n.Location == number).Cast<Product>().ToList();
            }
            else if (field == IntFields.stockCount)
            {
                filteredProducts = allProducts.Where(n => n.StockCount <= number).Cast<Product>().ToList();
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return filteredProducts;
        }

        public List<Product> ProductTypeQuery(ValueType type)
        {
            List<Product> allProducts = ReadFromFile(Elements.Product).Cast<Product>().ToList();
            List<Product> filteredProducts;
            if (allProducts.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (!(type is PType))
            {
                throw new System.Data.DataException("Bad Input!");
            }
            else
            {
                filteredProducts = allProducts.Where(n => n.Type.Equals((PType)type)).Cast<Product>().ToList();
            }
            return filteredProducts;
        }

        public List<Employee> EmployeeNameQuery(string name, StringFields field)
        {
            throw new NotImplementedException();
        }

        public List<Employee> EmployeeNumberQuery(int number, IntFields field)
        {
            throw new NotImplementedException();
        }

        public List<Employee> EmployeeTypeQuery(ValueType type)
        {
            throw new NotImplementedException();
        }
    }
}
