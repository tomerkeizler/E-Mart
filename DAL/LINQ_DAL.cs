using System;
using System.Collections.Generic;
using System.Linq;
using Backend;
using System.Xml.Serialization;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;


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
            //This is for Encrypt any list with the same class's key
            myRijndael.Key = key;
            myRijndael.IV = iv;
            var mStream = new MemoryStream();
            binFormatter.Serialize(mStream, original);
            //create the byte array:
            byte[] encrypted = EncryptBytes(myRijndael, mStream.ToArray());
            return encrypted;
        }
        private static List<object> Decrypt(byte[] encrypted)
        {
            //This is for decrypt any list with the same class's key
            myRijndael.Key = key;
            myRijndael.IV = iv;
            var mStream = new MemoryStream(encrypted);
            //decrypt the byte array from mem stream:
            byte[] decrypted = DecryptBytes(myRijndael, mStream.ToArray());
            mStream = new MemoryStream(decrypted);
            List<object> list = binFormatter.Deserialize(mStream) as List<object>;
            return list;
        }
        private static byte[] EncryptBytes(SymmetricAlgorithm alg, byte[] message)
        {
            //check if the value to encrypt exist
            if ((message == null) || (message.Length == 0))
            {
                return message;
            }
            //check if there is valid encrypt algorithm
            if (alg == null)
            {
                throw new ArgumentNullException("Bad Encrypt Type!");
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
            //check if the value to encrypt exist
            if ((message == null) || (message.Length == 0))
            {
                return message;
            }
            //check if there is valid encrypt algorithm
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
        

        //This method recieve list of object and create/override its xml file by the runtime types of the objects 
        public void WriteToFile(List<object> list, object obj)
        {
            //Delete the xml if the list is empty (last object deleted)
            if (list.ElementAtOrDefault(0) == null)
            {
                try
                {
                    File.Delete(obj.GetType() + ".xml");
                }
                catch (IOException e)
                {
                    throw new IOException("Cannot Clear " + list.ElementAtOrDefault(0).GetType() + ".xml File! Details: " + e.Message);
                }
            }
            //Perform encryption and write the XML file
            else
            {
                try
                {
                    StreamWriter WriteFileStream = new StreamWriter(list.ElementAtOrDefault(0).GetType() + ".xml");
                    byte[] encrypted = Encrypt(list);
                    SerializerObj.Serialize(WriteFileStream, encrypted);
                    WriteFileStream.Close();
                }
                catch (IOException e)
                {
                    throw new IOException("Cannot Write " + list.ElementAtOrDefault(0).GetType() + ".xml File! Details: " + e.Message);
                }
            } 
        }

        //This method recieve element to read and return its list from the XML file with this elements
        public List<object> ReadFromFile(Elements element)
        {
            if (File.Exists("Backend." + element.ToString() + ".xml"))
            {
                try
                {
                    using (FileStream stream = File.OpenRead("Backend." + element.ToString() + ".xml"))
                    {
                        byte[] encrypted = SerializerObj.Deserialize(stream) as byte[];
                        return Decrypt(encrypted);

                    }
                }
                catch (IOException e)
                {
                    throw new IOException("Cannot Read " + element.ToString() + ".xml File! Details: " + e.Message);
                }
            }
            //for not exists xml, return empty list
            else
            {
                return new List<object>();
            }
        }

        //Filter by name for product
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

        //Filter by number for product
        public List<Product> ProductNumberQuery(int minNumber, int maxNumber, IntFields field)
        {
            List<Product> allProducts = ReadFromFile(Elements.Product).Cast<Product>().ToList();
            List<Product> filteredProducts;
            if (allProducts.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (field == IntFields.price)
            {
                filteredProducts = allProducts.Where(n => n.Price >= minNumber && n.Price <= maxNumber).Cast<Product>().ToList();
            }
            else if (field == IntFields.productID)
            {
                filteredProducts = allProducts.Where(n => n.ProductID >= minNumber && n.ProductID <= maxNumber).Cast<Product>().ToList();
            }
            else if (field == IntFields.location)
            {
                filteredProducts = allProducts.Where(n => n.Location >= minNumber && n.Location <= maxNumber).Cast<Product>().ToList();
            }
            else if (field == IntFields.stockCount)
            {
                filteredProducts = allProducts.Where(n => n.StockCount >= minNumber && n.StockCount <= maxNumber).Cast<Product>().ToList();
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return filteredProducts;
        }

        //Filter by type for product
        public List<Product> ProductTypeQuery(ValueType type)
        {
            List<Product> allProducts = ReadFromFile(Elements.Product).Cast<Product>().ToList();
            List<Product> filteredProducts;
            if (allProducts.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (type is PType)
            {
                filteredProducts = allProducts.Where(n => n.Type.Equals((PType)type)).Cast<Product>().ToList();
            }
            else if (type is PStatus)
            {
                filteredProducts = allProducts.Where(n => n.Type.Equals((PStatus)type)).Cast<Product>().ToList();
            }
              
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return filteredProducts;
        }

        //Filter by name for employee
        public List<Employee> EmployeeNameQuery(string name, StringFields field)
        {
            List<Employee> allEmployee = ReadFromFile(Elements.Employee).Cast<Employee>().ToList();
            List<Employee> filteredEmployee;
            if (allEmployee.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (field == StringFields.firstName)
            {
                filteredEmployee = allEmployee.Where(n => n.FirstName.Equals(name)).Cast<Employee>().ToList();
            }
            else if (field == StringFields.lastName)
            {
                filteredEmployee = allEmployee.Where(n => n.LastName.Equals(name)).Cast<Employee>().ToList();
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return filteredEmployee;
        }

        //Filter by number for employee
        public List<Employee> EmployeeNumberQuery(int minNumber, int maxNumber, IntFields field)
        {
            List<Employee> allEmployee = ReadFromFile(Elements.Employee).Cast<Employee>().ToList();
            List<Employee> filteredEmployee;
            if (allEmployee.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (field == IntFields.id)
            {
                filteredEmployee = allEmployee.Where(n => n.Id >= minNumber && n.Id <= maxNumber).Cast<Employee>().ToList();
            }
            else if (field == IntFields.depID)
            {
                filteredEmployee = allEmployee.Where(n => n.DepID >= minNumber && n.DepID <= maxNumber).Cast<Employee>().ToList();
            }
            else if (field == IntFields.salary)
            {
                filteredEmployee = allEmployee.Where(n => n.Salary >= minNumber && n.Salary <= maxNumber).Cast<Employee>().ToList();
            }
            else if (field == IntFields.supervisiorID)
            {
                filteredEmployee = allEmployee.Where(n => n.SupervisiorID >= minNumber && n.SupervisiorID <= maxNumber).Cast<Employee>().ToList();
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return filteredEmployee;
        }

        //Filter by type for employee
        public List<Employee> EmployeeTypeQuery(ValueType type)
        {
            List<Employee> allEmployee = ReadFromFile(Elements.Employee).Cast<Employee>().ToList();
            List<Employee> filteredEmployee;
            if (allEmployee.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (!(type is Gender))
            {
                throw new System.Data.DataException("Bad Input!");
            }
            else
            {
                filteredEmployee = allEmployee.Where(n => n.Gender.Equals((Gender)type)).Cast<Employee>().ToList();
            }
            return filteredEmployee;
        }

        //Filter by name for club memeber
        public List<ClubMember> ClubMemberNameQuery(string name, StringFields field)
        {
            List<ClubMember> allClubMember = ReadFromFile(Elements.ClubMember).Cast<ClubMember>().ToList();
            List<ClubMember> filteredClubMember;
            if (allClubMember.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (field == StringFields.firstName)
            {
                filteredClubMember = allClubMember.Where(n => n.FirstName.Equals(name)).Cast<ClubMember>().ToList();
            }
            else if (field == StringFields.lastName)
            {
                filteredClubMember = allClubMember.Where(n => n.LastName.Equals(name)).Cast<ClubMember>().ToList();
            }
            else if (field == StringFields.dateOfBirth)
            {
                filteredClubMember = allClubMember.Where(n => n.DateOfBirth.ToShortDateString().Equals(name)).Cast<ClubMember>().ToList();
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return filteredClubMember;
        }

        //Filter by number for club member
        public List<ClubMember> ClubMemberNumberQuery(int minNumber, int maxNumber, IntFields field)
        {
            List<ClubMember> allClubMember = ReadFromFile(Elements.ClubMember).Cast<ClubMember>().ToList();
            List<ClubMember> filteredClubMember;
            if (allClubMember.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (field == IntFields.memberID)
            {
                filteredClubMember = allClubMember.Where(n => n.MemberID >= minNumber && n.MemberID <= maxNumber).Cast<ClubMember>().ToList();
            }
            else if (field == IntFields.id)
            {
                filteredClubMember = allClubMember.Where(n => n.Id >= minNumber && n.Id <= maxNumber).Cast<ClubMember>().ToList();
            }
            else if (field == IntFields.tranHistory)
            {
                filteredClubMember = allClubMember.Where(n => n.TranHistory.Any(x => x.TransactionID >= minNumber && x.TransactionID <= maxNumber)).Cast<ClubMember>().ToList();
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return filteredClubMember;
        }

        //Filter by type for club member
        public List<ClubMember> ClubMemberTypeQuery(ValueType type)
        {
            List<ClubMember> allClubMember = ReadFromFile(Elements.ClubMember).Cast<ClubMember>().ToList();
            List<ClubMember> filteredClubMember;
            if (allClubMember.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (type is Gender)
            {
                filteredClubMember = allClubMember.Where(n => n.Gender.Equals((Gender)type)).Cast<ClubMember>().ToList();
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return filteredClubMember;
        }

        //Filter by name for department
        public List<Department> DepartmentNameQuery(string name, StringFields field)
        {
            List<Department> allDepartment = ReadFromFile(Elements.Department).Cast<Department>().ToList();
            List<Department> filteredDepartment;
            if (allDepartment.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (field != StringFields.name)
            {
                throw new System.Data.DataException("Bad Input!");
            }
            filteredDepartment = allDepartment.Where(n => n.Name.Equals(name)).Cast<Department>().ToList();
            return filteredDepartment;
        }

        //Filter by number for department
        public List<Department> DepartmentNumberQuery(int minNumber, int maxNumber, IntFields field)
        {
            {
                List<Department> allDepartment = ReadFromFile(Elements.Department).Cast<Department>().ToList();
                List<Department> filteredDepartment;
                if (allDepartment.ElementAtOrDefault(0) == null)
                {
                    throw new InvalidDataException("There is nothing to find from.");
                }
                if (field != IntFields.departmentID)
                {
                    throw new System.Data.DataException("Bad Input!");
                }
                filteredDepartment = allDepartment.Where(n => n.DepartmentID >= minNumber && n.DepartmentID <= maxNumber).Cast<Department>().ToList();
                return filteredDepartment;
            }
        }

        //Filter by number for transaction
        public List<Transaction> TransactionNumberQuery(int minNumber, int maxNumber, IntFields field)
        {
            {
                List<Transaction> allTransaction = ReadFromFile(Elements.Transaction).Cast<Transaction>().ToList();
                List<Transaction> filteredTransaction;
                if (allTransaction.ElementAtOrDefault(0) == null)
                {
                    throw new InvalidDataException("There is nothing to find from.");
                }
                if (field == IntFields.transactionID)
                {
                    filteredTransaction = allTransaction.Where(n => n.TransactionID >= minNumber && n.TransactionID <= maxNumber).Cast<Transaction>().ToList();
                }
                else if (field == IntFields.receipt)
                {
                    filteredTransaction = allTransaction.Where(n => n.Receipt.ProductsIDs.Any(x => x >= minNumber && x <= maxNumber)).Cast<Transaction>().ToList();
                }
                else
                {
                    throw new System.Data.DataException("Bad Input!");
                }
                return filteredTransaction;
            }
        }

        //Filter by type for transaction
        public List<Transaction> TransactionTypeQuery(ValueType type)
        {
            List<Transaction> allTransaction = ReadFromFile(Elements.Transaction).Cast<Transaction>().ToList();
            List<Transaction> filteredTransaction;
            if (allTransaction.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (type is Is_a_return)
            {
                filteredTransaction = allTransaction.Where(n => n.Is_a_Return.Equals(type)).Cast<Transaction>().ToList();
            }
            else if (type is PaymentMethod)
            {
                filteredTransaction = allTransaction.Where(n => n.Payment.Equals(type)).Cast<Transaction>().ToList();
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return filteredTransaction;
        }

        //Filter by name for user
        public List<User> UserNameQuery(string name, StringFields field)
        {
            List<User> allUser = ReadFromFile(Elements.User).Cast<User>().ToList();
            List<User> filteredUser;
            if (allUser.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (field != StringFields.username)
            {
                throw new System.Data.DataException("Bad Input!");
            }
            filteredUser = allUser.Where(n => n.UserName.Equals(name)).Cast<User>().ToList();
            return filteredUser;
        }


        public List<Transaction> TransactionNameQuery(string name, StringFields field)
        {
            List<Transaction> allTrans = ReadFromFile(Elements.Transaction).Cast<Transaction>().ToList();
            List<Transaction> filteredTrans;
            if (field != StringFields.currentDate)
            {
                throw new System.Data.DataException("Bad Input!");
            }
            filteredTrans = allTrans.Where(n => n.CurrentDate.ToShortDateString().Equals(name)).Cast<Transaction>().ToList();
            return filteredTrans;

        }
    }
}
