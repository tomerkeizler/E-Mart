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
            SerializerObj = new XmlSerializer(typeof(byte[]), new Type[] { typeof(Backend.Product), typeof(byte[]) });
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
                catch (System.IO.IOException e)
                {
                    throw new System.IO.IOException("Cannot Clear " + list.ElementAtOrDefault(0).GetType() + ".xml File! Details: " + e.Message);
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
                catch (System.IO.IOException e)
                {
                    throw new System.IO.IOException("Cannot Write " + list.ElementAtOrDefault(0).GetType() + ".xml File! Details: " + e.Message);
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
                catch (System.IO.IOException e)
                {
                    throw new System.IO.IOException("Cannot Read " + element.ToString() + ".xml File! Details: " + e.Message);
                }
            }
            //for not exists xml, return empty list
            else
            {
                return new List<object>();
            }
        }

        //Filter by name for product
        public List<Backend.Product> ProductNameQuery(string name, StringFields field)
        {
            List<Backend.Product> allProducts = ReadFromFile(Elements.Product).Cast<Backend.Product>().ToList();
            List<Backend.Product> filteredProducts;
            if (allProducts.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (field != StringFields.name)
            {
                throw new System.Data.DataException("Bad Input!");
            }
            filteredProducts = allProducts.Where(n => n.Name.Equals(name)).Cast<Backend.Product>().ToList();
            return filteredProducts;
        }

        //Filter by number for product
        public List<Backend.Product> ProductNumberQuery(int minNumber, int maxNumber, IntFields field)
        {
            List<Backend.Product> allProducts = ReadFromFile(Elements.Product).Cast<Backend.Product>().ToList();
            List<Backend.Product> filteredProducts;
            if (allProducts.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (field == IntFields.price)
            {
                filteredProducts = allProducts.Where(n => n.Price >= minNumber && n.Price <= maxNumber).Cast<Backend.Product>().ToList();
            }
            else if (field == IntFields.productID)
            {
                filteredProducts = allProducts.Where(n => n.ProductID >= minNumber && n.ProductID <= maxNumber).Cast<Backend.Product>().ToList();
            }
            else if (field == IntFields.location)
            {
                filteredProducts = allProducts.Where(n => n.Location >= minNumber && n.Location <= maxNumber).Cast<Backend.Product>().ToList();
            }
            else if (field == IntFields.stockCount)
            {
                filteredProducts = allProducts.Where(n => n.StockCount >= minNumber && n.StockCount <= maxNumber).Cast<Backend.Product>().ToList();
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return filteredProducts;
        }

        //Filter by type for product
        public List<Backend.Product> ProductTypeQuery(ValueType type)
        {
            List<Backend.Product> allProducts = ReadFromFile(Elements.Product).Cast<Backend.Product>().ToList();
            List<Backend.Product> filteredProducts;
            if (allProducts.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (type is PType)
            {
                filteredProducts = allProducts.Where(n => n.Type.Equals((PType)type)).Cast<Backend.Product>().ToList();
            }
            else if (type is PStatus)
            {
                filteredProducts = allProducts.Where(n => n.InStock.Equals((PStatus)type)).Cast<Backend.Product>().ToList();
            }

            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return filteredProducts;
        }

        //Filter by name for employee
        public List<Backend.Employee> EmployeeNameQuery(string name, StringFields field)
        {
            List<Backend.Employee> allEmployee = ReadFromFile(Elements.Employee).Cast<Backend.Employee>().ToList();
            List<Backend.Employee> filteredEmployee;
            if (allEmployee.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (field == StringFields.firstName)
            {
                filteredEmployee = allEmployee.Where(n => n.FirstName.Equals(name)).Cast<Backend.Employee>().ToList();
            }
            else if (field == StringFields.lastName)
            {
                filteredEmployee = allEmployee.Where(n => n.LastName.Equals(name)).Cast<Backend.Employee>().ToList();
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return filteredEmployee;
        }

        //Filter by number for employee
        public List<Backend.Employee> EmployeeNumberQuery(int minNumber, int maxNumber, IntFields field)
        {
            List<Backend.Employee> allEmployee = ReadFromFile(Elements.Employee).Cast<Backend.Employee>().ToList();
            List<Backend.Employee> filteredEmployee;
            if (allEmployee.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (field == IntFields.id)
            {
                filteredEmployee = allEmployee.Where(n => n.Id >= minNumber && n.Id <= maxNumber).Cast<Backend.Employee>().ToList();
            }
            else if (field == IntFields.depID)
            {
                filteredEmployee = allEmployee.Where(n => n.DepID >= minNumber && n.DepID <= maxNumber).Cast<Backend.Employee>().ToList();
            }
            else if (field == IntFields.salary)
            {
                filteredEmployee = allEmployee.Where(n => n.Salary >= minNumber && n.Salary <= maxNumber).Cast<Backend.Employee>().ToList();
            }
            else if (field == IntFields.supervisiorID)
            {
                filteredEmployee = allEmployee.Where(n => n.SupervisiorID >= minNumber && n.SupervisiorID <= maxNumber).Cast<Backend.Employee>().ToList();
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return filteredEmployee;
        }

        //Filter by type for employee
        public List<Backend.Employee> EmployeeTypeQuery(ValueType type)
        {
            List<Backend.Employee> allEmployee = ReadFromFile(Elements.Employee).Cast<Backend.Employee>().ToList();
            List<Backend.Employee> filteredEmployee;
            if (allEmployee.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (type is Gender)
            {
                filteredEmployee = allEmployee.Where(n => n.Gender.Equals((Gender)type)).Cast<Backend.Employee>().ToList();
            }
            else if (type is Rank)
            {
                filteredEmployee = allEmployee.Where(n => n.Rank.Equals((Rank)type)).Cast<Backend.Employee>().ToList();
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return filteredEmployee;
        }

        //Filter by name for club memeber
        public List<Backend.ClubMember> ClubMemberNameQuery(string name, StringFields field)
        {
            List<Backend.ClubMember> allClubMember = ReadFromFile(Elements.ClubMember).Cast<Backend.ClubMember>().ToList();
            List<Backend.ClubMember> filteredClubMember;
            if (allClubMember.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (field == StringFields.firstName)
            {
                filteredClubMember = allClubMember.Where(n => n.FirstName.Equals(name)).Cast<Backend.ClubMember>().ToList();
            }
            else if (field == StringFields.lastName)
            {
                filteredClubMember = allClubMember.Where(n => n.LastName.Equals(name)).Cast<Backend.ClubMember>().ToList();
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return filteredClubMember;
        }

        //Filter by number for club member
        public List<Backend.ClubMember> ClubMemberNumberQuery(int minNumber, int maxNumber, IntFields field)
        {
            List<Backend.ClubMember> allClubMember = ReadFromFile(Elements.ClubMember).Cast<Backend.ClubMember>().ToList();
            List<Backend.ClubMember> filteredClubMember;
            if (allClubMember.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (field == IntFields.memberID)
            {
                filteredClubMember = allClubMember.Where(n => n.MemberID >= minNumber && n.MemberID <= maxNumber).Cast<Backend.ClubMember>().ToList();
            }
            else if (field == IntFields.id)
            {
                filteredClubMember = allClubMember.Where(n => n.Id >= minNumber && n.Id <= maxNumber).Cast<Backend.ClubMember>().ToList();
            }
            else if (field == IntFields.tranHistory)
            {
                filteredClubMember = allClubMember.Where(n => n.TranHistory.Any(x => x.TransactionID >= minNumber && x.TransactionID <= maxNumber)).Cast<Backend.ClubMember>().ToList();
            }
            else if (field == IntFields.dateOfBirth)
            {
                filteredClubMember = allClubMember.Where(n => int.Parse(n.DateOfBirth.ToString("yyyyMMdd")) >= minNumber && int.Parse(n.DateOfBirth.ToString("yyyyMMdd")) <= maxNumber).Cast<Backend.ClubMember>().ToList();
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return filteredClubMember;
        }

        //Filter by type for club member
        public List<Backend.ClubMember> ClubMemberTypeQuery(ValueType type)
        {
            List<Backend.ClubMember> allClubMember = ReadFromFile(Elements.ClubMember).Cast<Backend.ClubMember>().ToList();
            List<Backend.ClubMember> filteredClubMember;
            if (allClubMember.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (type is Gender)
            {
                filteredClubMember = allClubMember.Where(n => n.Gender.Equals((Gender)type)).Cast<Backend.ClubMember>().ToList();
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return filteredClubMember;
        }

        //Department Name Changer
        public void DepartmentNameEdit(Backend.Department dep)
        {
            List<Backend.Department> allDepartment = ReadFromFile(Elements.Department).Cast<Backend.Department>().ToList();
            foreach (Backend.Department currentDep in allDepartment)
            {
                if (currentDep.DepartmentID == dep.DepartmentID)
                {
                    currentDep.Name = dep.Name;
                    break;
                }
            }
            WriteToFile(allDepartment.Cast<object>().ToList(), dep);
        }
        //Filter by name for department
        public List<Backend.Department> DepartmentNameQuery(string name, StringFields field)
        {
            List<Backend.Department> allDepartment = ReadFromFile(Elements.Department).Cast<Backend.Department>().ToList();
            List<Backend.Department> filteredDepartment;
            if (allDepartment.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (field != StringFields.name)
            {
                throw new System.Data.DataException("Bad Input!");
            }
            filteredDepartment = allDepartment.Where(n => n.Name.Equals(name)).Cast<Backend.Department>().ToList();
            return filteredDepartment;
        }

        //Filter by number for department
        public List<Backend.Department> DepartmentNumberQuery(int minNumber, int maxNumber, IntFields field)
        {
            {
                List<Backend.Department> allDepartment = ReadFromFile(Elements.Department).Cast<Backend.Department>().ToList();
                List<Backend.Department> filteredDepartment;
                if (allDepartment.ElementAtOrDefault(0) == null)
                {
                    throw new InvalidDataException("There is nothing to find from.");
                }
                if (field != IntFields.departmentID)
                {
                    throw new System.Data.DataException("Bad Input!");
                }
                filteredDepartment = allDepartment.Where(n => n.DepartmentID >= minNumber && n.DepartmentID <= maxNumber).Cast<Backend.Department>().ToList();
                return filteredDepartment;
            }
        }

        //Filter by number for transaction
        public List<Backend.Transaction> TransactionNumberQuery(int minNumber, int maxNumber, IntFields field)
        {
            {
                List<Backend.Transaction> allTransaction = ReadFromFile(Elements.Transaction).Cast<Backend.Transaction>().ToList();
                List<Backend.Transaction> filteredTransaction;
                if (allTransaction.ElementAtOrDefault(0) == null)
                {
                    throw new InvalidDataException("There is nothing to find from.");
                }
                if (field == IntFields.transactionID)
                {
                    filteredTransaction = allTransaction.Where(n => n.TransactionID >= minNumber && n.TransactionID <= maxNumber).Cast<Backend.Transaction>().ToList();
                }
 
                else if (field == IntFields.receipt)
                {
                    filteredTransaction = allTransaction.Where(n => n.Receipt.Any(x => x.PrdID >= minNumber && x.PrdID <= maxNumber)).Cast<Backend.Transaction>().ToList();
                }
                else if (field == IntFields.currentDate)
                {
                    filteredTransaction = allTransaction.Where(n => int.Parse(n.CurrentDate.ToString("yyyyMMdd")) >= minNumber && int.Parse(n.CurrentDate.ToString("yyyyMMdd")) <= maxNumber).Cast<Backend.Transaction>().ToList();
                }
                else
                {
                    throw new System.Data.DataException("Bad Input!");
                }
                return filteredTransaction;
            }
        }

        //Filter by type for transaction
        public List<Backend.Transaction> TransactionTypeQuery(ValueType type)
        {
            List<Backend.Transaction> allTransaction = ReadFromFile(Elements.Transaction).Cast<Backend.Transaction>().ToList();
            List<Backend.Transaction> filteredTransaction;
            if (allTransaction.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (type is Is_a_return)
            {
                filteredTransaction = allTransaction.Where(n => n.Is_a_Return.Equals(type)).Cast<Backend.Transaction>().ToList();
            }
            else if (type is PaymentMethod)
            {
                filteredTransaction = allTransaction.Where(n => n.Payment.Equals(type)).Cast<Backend.Transaction>().ToList();
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return filteredTransaction;
        }

        //Filter by name for user
        public List<Backend.User> UserNameQuery(string name, StringFields field)
        {
            List<Backend.User> allUser = ReadFromFile(Elements.User).Cast<Backend.User>().ToList();
            List<Backend.User> filteredUser;
            if (allUser.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (field != StringFields.username)
            {
                throw new System.Data.DataException("Bad Input!");
            }
            filteredUser = allUser.Where(n => n.UserName.Equals(name)).Cast<Backend.User>().ToList();
            return filteredUser;
        }
        public List<Backend.User> UserTypeQuery(ValueType type)
        {
            List<Backend.User> allUser = ReadFromFile(Elements.User).Cast<Backend.User>().ToList();
            List<Backend.User> filteredUser;
            if (allUser.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (type is Rank)
            {
                filteredUser = allUser.Where(n => (n.Person is Backend.Employee) && ((Backend.Employee)n.Person).Rank.Equals((Rank)type)).Cast<Backend.User>().ToList();
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return filteredUser;
        }
        public List<Backend.User> UserPersonQuery(object person)
        {
            List<Backend.User> allUser = ReadFromFile(Elements.User).Cast<Backend.User>().ToList();
            List<Backend.User> filteredUser;
            if (allUser.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (person == null)
            {
                throw new System.Data.DataException("Bad Input!");
            }
            filteredUser = allUser.Where(n => n.Person.Equals(person)).Cast<Backend.User>().ToList();
            return filteredUser;
        }
        //Filter by name for Customer
        public List<Backend.Customer> CustomerNameQuery(string name, StringFields field)
        {
            List<Backend.Customer> allCustomer = ReadFromFile(Elements.Customer).Cast<Backend.Customer>().ToList();
            List<Backend.Customer> filteredCustomer;
            if (allCustomer.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (field == StringFields.firstName)
            {
                filteredCustomer = allCustomer.Where(n => n.FirstName.Equals(name)).Cast<Backend.Customer>().ToList();
            }
            else if (field == StringFields.lastName)
            {
                filteredCustomer = allCustomer.Where(n => n.LastName.Equals(name)).Cast<Backend.Customer>().ToList();
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return filteredCustomer;
        }

        //Filter by number for Customer
        public List<Backend.Customer> CustomerNumberQuery(int minNumber, int maxNumber, IntFields field)
        {
            List<Backend.Customer> allCustomer = ReadFromFile(Elements.Customer).Cast<Backend.Customer>().ToList();
            List<Backend.Customer> filteredCustomer;
            if (allCustomer.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            if (field == IntFields.id)
            {
                filteredCustomer = allCustomer.Where(n => n.Id >= minNumber && n.Id <= maxNumber).Cast<Backend.Customer>().ToList();
            }
            else if (field == IntFields.tranHistory)
            {
                filteredCustomer = allCustomer.Where(n => n.TranHistory.Any(x => x.TransactionID >= minNumber && x.TransactionID <= maxNumber)).Cast<Backend.Customer>().ToList();
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return filteredCustomer;
        }
        //Filter by types for product in data grid
        public void FilterProducts(System.Collections.ObjectModel.ObservableCollection<Buyable> currentList, PType type, bool isAdd)
        {
            if (isAdd)
            {
                List<Backend.Product> filteredProducts = this.ProductTypeQuery(type);
                filteredProducts = filteredProducts.Where(n => !(n.InStock.Equals(PStatus.Empty))).Cast<Backend.Product>().ToList();
                foreach (Backend.Product p in filteredProducts)
                {
                    currentList.Add(new Buyable(p, 0, p.StockCount));
                }
            }
            else
            {
                List<Buyable> toRemove = new List<Buyable>();
                foreach (Buyable b in currentList)
                {
                    if (b.Prod.Type.Equals(type))
                    {
                        toRemove.Add(b);
                    }
                }
                foreach (Buyable b in toRemove)
                {
                    currentList.Remove(b);
                }
                toRemove.Clear();
            }
        }
    }
}
