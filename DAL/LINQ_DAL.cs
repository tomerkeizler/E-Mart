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
        private static RijndaelManaged myRijndael;
        private static BinaryFormatter binFormatter;
        private static byte[] key;
        private static byte[] iv;

        //Constuctors:
        public LINQ_DAL()
        {
            binFormatter = new BinaryFormatter();
            SerializerObj = new XmlSerializer(typeof(byte[]), new Type[] { typeof(Product), typeof(byte[]) });
            myRijndael = new RijndaelManaged();
            key = new byte[32] { 118, 123, 23, 17, 161, 152, 35, 68, 126, 213, 16, 115, 68, 217, 58, 108, 56, 218, 5, 78, 28, 128, 113, 208, 61, 56, 10, 87, 187, 162, 233, 38 };
            iv = new byte[16] { 33, 241, 14, 16, 103, 18, 14, 248, 4, 54, 18, 5, 60, 76, 16, 191 };
        }


        //Behaviour:
        private static byte[] Encrypt(List<object> original)
        {
            myRijndael.Key = key;
            myRijndael.IV = iv;
            var mStream = new MemoryStream();
            binFormatter.Serialize(mStream, original);
            byte[] encrypted = EncryptBytes(myRijndael, mStream.ToArray());
            return encrypted;
        }
        private static List<object> Decrypt(byte[] encrypted)
        {
            myRijndael.Key = key;
            myRijndael.IV = iv;
            var mStream = new MemoryStream(encrypted);
            byte[] decrypted = DecryptBytes(myRijndael, mStream.ToArray());
            mStream = new MemoryStream(decrypted);
            List<object> list = binFormatter.Deserialize(mStream) as List<object>;
            return list;
        }
        private static byte[] EncryptBytes(SymmetricAlgorithm alg, byte[] message)
        {
            if ((message == null) || (message.Length == 0))
            {
                return message;
            }

            if (alg == null)
            {
                throw new ArgumentNullException("alg");
            }

            using (var stream = new MemoryStream())
            using (var encryptor = alg.CreateEncryptor())
            using (var encrypt = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
            {
                encrypt.Write(message, 0, message.Length);
                encrypt.FlushFinalBlock();
                return stream.ToArray();
            }
        }

        private static byte[] DecryptBytes(SymmetricAlgorithm alg, byte[] message)
        {
            if ((message == null) || (message.Length == 0))
            {
                return message;
            }

            if (alg == null)
            {
                throw new ArgumentNullException("alg");
            }

            using (var stream = new MemoryStream())
            using (var decryptor = alg.CreateDecryptor())
            using (var encrypt = new CryptoStream(stream, decryptor, CryptoStreamMode.Write))
            {
                encrypt.Write(message, 0, message.Length);
                encrypt.FlushFinalBlock();
                return stream.ToArray();
            }
        }
        


        public void WriteToFile(List<object> list)
        {
            if (list.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("Nothing to Write");
            }
            StreamWriter WriteFileStream = new StreamWriter(list.ElementAtOrDefault(0).GetType() + ".xml");
            byte[] encrypted = Encrypt(list);
            SerializerObj.Serialize(WriteFileStream, encrypted);
            WriteFileStream.Close();
        }

        public List<object> ReadFromFile(Elements element)
        {
            if (File.Exists("Backend." + element.ToString() + ".xml"))
            {
                using (FileStream stream = File.OpenRead("Backend." + element.ToString() + ".xml"))
                {
                    byte[] encrypted = SerializerObj.Deserialize(stream) as byte[];
                    return Decrypt(encrypted);

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
