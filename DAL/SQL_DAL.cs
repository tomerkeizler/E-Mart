using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend;
using System.IO;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace DAL
{
    public class SQL_DAL : IDAL
    {
        //Fields
        E_MartDB_LINQtoSQLDataContext db;

        //Constructors
        public SQL_DAL()
        {
            try
            {
                db = new E_MartDB_LINQtoSQLDataContext();
            }
            catch
            {
                throw new Exception("Cant connect to E-Mart DB!");
            }
        }

        //Adding the updated list of entities to DB (by remove all the types and re-add them to compitable the BL operations)
        public void WriteToFile(List<object> list, object obj)
        {
            if (obj is Backend.Product)
            {
                foreach (TopSeller top in db.TopSellers)
                {
                    db.TopSellers.DeleteOnSubmit(top);
                }
                foreach (Product prod in db.Products){
                    db.Products.DeleteOnSubmit(prod);
                }
                foreach (Backend.Product prod in list)
                {
                    db.Products.InsertOnSubmit(ProductConverterToContext(prod));
                }
            }
            else if (obj is Backend.Department)
            {
                foreach (Department dep in db.Departments)
                {
                    db.Departments.DeleteOnSubmit(dep);
                }
                foreach (Backend.Department dep in list)
                {
                    db.Departments.InsertOnSubmit(DepartmentConverterToContext(dep));
                }
            }
            else if (obj is Backend.Employee)
            {
                foreach (Employee emp in db.Employees)
                {
                    db.Employees.DeleteOnSubmit(emp);
                }
                foreach (Backend.Employee emp in list)
                {
                    db.Employees.InsertOnSubmit(EmployeeConverterToContext(emp));
                }

            }
            else if (obj is Backend.ClubMember)
            {
                foreach (TranHistoryLinkedTable linkedTrans in db.TranHistoryLinkedTables)
                {
                    if (linkedTrans.IsAClubMember)
                    {
                        db.TranHistoryLinkedTables.DeleteOnSubmit(linkedTrans);
                    }
                }
                foreach (ClubMember club in db.ClubMembers)
                {
                    db.ClubMembers.DeleteOnSubmit(club);
                }
                foreach (Customer clubAsCust in db.Customers)
                {
                    if (clubAsCust.IsAClubMember)
                    {
                        db.Customers.DeleteOnSubmit(clubAsCust);
                    }
                    foreach (CreditCard credit in db.CreditCards)
                    {
                        if (credit.CreditNumber == clubAsCust.CreditCard)
                        {
                            db.CreditCards.DeleteOnSubmit(credit);
                        }
                    }
                }
                foreach (Backend.ClubMember club in list)
                {
                    db.ClubMembers.InsertOnSubmit(ClubMemberConverterToContext(club));
                    foreach (Backend.Transaction trans in club.TranHistory)
                    {
                        TranHistoryLinkedTable currentLinkedTrans = new TranHistoryLinkedTable();
                        currentLinkedTrans.CustomerID = club.Id;
                        currentLinkedTrans.TransID = trans.TransactionID;
                        currentLinkedTrans.IsAClubMember = true;
                        db.TranHistoryLinkedTables.InsertOnSubmit(currentLinkedTrans);
                    }
                }
            }
            else if (obj is Backend.Customer)
            {
                foreach (TranHistoryLinkedTable linkedTrans in db.TranHistoryLinkedTables)
                {
                    if (!linkedTrans.IsAClubMember)
                    {
                        db.TranHistoryLinkedTables.DeleteOnSubmit(linkedTrans);
                    }
                }
                foreach (Customer cust in db.Customers)
                {
                    if (!cust.IsAClubMember)
                    {
                        db.Customers.DeleteOnSubmit(cust);
                        foreach (CreditCard credit in db.CreditCards)
                        {
                            if (credit.CreditNumber == cust.CreditCard)
                            {
                                db.CreditCards.DeleteOnSubmit(credit);
                            }
                        }
                    } 
                }
                foreach (Backend.Customer cust in list)
                {
                    db.Customers.InsertOnSubmit(CustomerConverterToContext(cust));
                    foreach (Backend.Transaction trans in cust.TranHistory)
                    {
                        TranHistoryLinkedTable currentLinkedTrans = new TranHistoryLinkedTable();
                        currentLinkedTrans.CustomerID = cust.Id;
                        currentLinkedTrans.TransID = trans.TransactionID;
                        currentLinkedTrans.IsAClubMember = false;
                        db.TranHistoryLinkedTables.InsertOnSubmit(currentLinkedTrans);
                    }
                }
            }
            else if (obj is Backend.Transaction)
            {
                List<Transaction> currentTransList = new List<Transaction>();
                foreach (Transaction trans in db.Transactions)
                {
                    currentTransList.Add(trans);
                }
                foreach (Backend.Transaction trans in list)
                {
                    bool isIn = false;
                    foreach (Transaction currTran in currentTransList)
                    {
                        if (currTran.TransactionID == trans.TransactionID)
                        {
                            isIn = true;
                            break;
                        }
                    }
                    if (!isIn)
                    {
                        db.Transactions.InsertOnSubmit(TransactionConverterToContext(trans));
                        foreach (Backend.Purchase purch in trans.Receipt){
                            db.Purchases.InsertOnSubmit(PurchaseConverterToContext(purch));
                        }
                        
                    }
                }
            }
            else if (obj is Backend.User)
            {
                foreach (User usr in db.Users)
                {
                    db.Users.DeleteOnSubmit(usr);
                }
                foreach (Backend.User usr in list)
                {
                    db.Users.InsertOnSubmit(UserConverterToContext(usr));
                }

            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            try
            {
                db.SubmitChanges();
            }
            catch
            {
                throw new InvalidDataException("There is a conflict with the change. Operation aborted.");
            }
        }

        //Read the specific entity from the DB, return it as List
        public List<object> ReadFromFile(Backend.Elements element)
        {
            List<object> currentList = new List<object>();
            if (element.Equals(Elements.Product))
            {
                foreach (Product prod in db.Products)
                {
                    currentList.Add(ProductConverterToBackend(prod));
                }
            }
            else if (element.Equals(Elements.Department))
            {
                foreach (Department dep in db.Departments)
                {
                    currentList.Add(DepartmentConverterToBackend(dep));
                }
            }
            else if (element.Equals(Elements.Employee))
            {
                foreach (Employee emp in db.Employees)
                {
                    currentList.Add(EmployeeConverterToBackend(emp));
                }
            }
            else if (element.Equals(Elements.ClubMember))
            {
                foreach (ClubMember club in db.ClubMembers)
                {
                    currentList.Add(ClubMemberConverterToBackend(club));
                }
            }
            else if (element.Equals(Elements.Customer))
            {
                foreach (Customer cust in db.Customers)
                {
                    if (!cust.IsAClubMember)
                    {
                        currentList.Add(CustomerConverterToBackend(cust));
                    }
                }
            }
            else if (element.Equals(Elements.Transaction))
            {
                foreach (Transaction trans in db.Transactions)
                {
                    currentList.Add(TransactionConverterToBackend(trans));
                }
            }
            else if (element.Equals(Elements.User))
            {
                foreach (User usr in db.Users)
                {
                    currentList.Add(UserConverterToBackend(usr));
                }
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return currentList;
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
            foreach (Department currentDep in db.Departments)
            {
                if (currentDep.DepartmentID == dep.DepartmentID)
                {
                    currentDep.Name = dep.Name;
                    break;
                }
            }
            try
            {
                db.SubmitChanges();
            }
            catch
            {
                throw new InvalidDataException("There is a conflict with the change. Operation aborted.");
            }
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
                filteredUser = allUser.Where(n => (n.Person is Employee) && ((Employee)n.Person).Rank.Equals((Rank)type)).Cast<Backend.User>().ToList();
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



        //Converters


        //Product
        public Backend.Product ProductConverterToBackend(Product dataContextProduct)
        {
            Backend.Product currentProduct = new Backend.Product();
            currentProduct.Name = dataContextProduct.Name;
            currentProduct.Type = (PType)dataContextProduct.Type;
            currentProduct.ProductID = dataContextProduct.ProductID;
            currentProduct.Location = dataContextProduct.Location;
            currentProduct.InStock = (PStatus)dataContextProduct.InStock;
            currentProduct.StockCount = dataContextProduct.StockCount;
            currentProduct.Price = dataContextProduct.Price;
            currentProduct.TopSellerStatus.CurrentMonth = dataContextProduct.TopSeller.CurrentMonth;
            currentProduct.TopSellerStatus.IsTopSeller = dataContextProduct.TopSeller.IsTopSeller;
            currentProduct.TopSellerStatus.SellCounter = dataContextProduct.TopSeller.SellCounter;
            currentProduct.TopSellerStatus.ProductID = dataContextProduct.TopSeller.ProductID;
            return currentProduct;
        }

        public Product ProductConverterToContext(Backend.Product currentProduct)
        {
            TopSeller dataContextTopSeller = new TopSeller();
            Product dataContextProduct = new Product();
            dataContextProduct.Name = currentProduct.Name;
            dataContextProduct.Type = (int)currentProduct.Type;
            dataContextProduct.ProductID = currentProduct.ProductID;
            dataContextProduct.Location = currentProduct.Location;
            dataContextProduct.InStock = (int)currentProduct.InStock;
            dataContextProduct.StockCount = currentProduct.StockCount;
            dataContextProduct.Price = currentProduct.Price;
            dataContextTopSeller.CurrentMonth = currentProduct.TopSellerStatus.CurrentMonth;
            dataContextTopSeller.IsTopSeller = currentProduct.TopSellerStatus.IsTopSeller;
            dataContextTopSeller.SellCounter = currentProduct.TopSellerStatus.SellCounter;
            dataContextTopSeller.ProductID = currentProduct.TopSellerStatus.ProductID;
            dataContextProduct.TopSeller = dataContextTopSeller;
            return dataContextProduct;
        }


        //Department
        public Backend.Department DepartmentConverterToBackend(Department dataContextDepartment)
        {
            Backend.Department currentDepartment = new Backend.Department();
            currentDepartment.DepartmentID = dataContextDepartment.DepartmentID;
            currentDepartment.Name = dataContextDepartment.Name;
            return currentDepartment;
        }

        public Department DepartmentConverterToContext(Backend.Department currentDepartment)
        {
            Department dataContextDepartment = new Department();
            dataContextDepartment.Name = currentDepartment.Name;
            dataContextDepartment.DepartmentID = currentDepartment.DepartmentID;
            return dataContextDepartment;
        }


        //Employee
        public Backend.Employee EmployeeConverterToBackend(Employee dataContextEmployee)
        {
            Backend.Employee currentEmployee = new Backend.Employee();
            currentEmployee.DepID = dataContextEmployee.DepID;
            currentEmployee.FirstName = dataContextEmployee.FirstName;
            currentEmployee.LastName = dataContextEmployee.LastName;
            currentEmployee.Gender = (Gender)dataContextEmployee.Gender;
            currentEmployee.Id = dataContextEmployee.Id;
            currentEmployee.Rank = (Rank)dataContextEmployee.Rank;
            currentEmployee.Salary = dataContextEmployee.Salary;
            currentEmployee.SupervisiorID = dataContextEmployee.SupervisiorID;
            return currentEmployee;
        }

        public Employee EmployeeConverterToContext(Backend.Employee currentEmployee)
        {
            Employee dataContextEmployee = new Employee();
            dataContextEmployee.DepID = currentEmployee.DepID;
            dataContextEmployee.FirstName = currentEmployee.FirstName;
            dataContextEmployee.LastName = currentEmployee.LastName;
            dataContextEmployee.Gender = (int)currentEmployee.Gender;
            dataContextEmployee.Id = currentEmployee.Id;
            dataContextEmployee.Rank = (int)currentEmployee.Rank;
            dataContextEmployee.Salary = currentEmployee.Salary;
            dataContextEmployee.SupervisiorID = currentEmployee.SupervisiorID;
            return dataContextEmployee;
        }


        //Customer
        public Backend.Customer CustomerConverterToBackend(Customer dataContextCustomer)
        {
            Backend.Customer currentCustomer = new Backend.Customer();

            //Credit Card Entity
            if (dataContextCustomer.CreditCard != null)
            {
                currentCustomer.CreditCard.CreditNumber = dataContextCustomer.CreditCard1.CreditNumber;
                currentCustomer.CreditCard.ExpirationDate = dataContextCustomer.CreditCard1.ExpirationDate;
                currentCustomer.CreditCard.FirstName = dataContextCustomer.CreditCard1.FirstName;
                currentCustomer.CreditCard.LastName = dataContextCustomer.CreditCard1.LastName;
            }
            else
            {
                currentCustomer.CreditCard = null;
            }
            
            //Customer Entity
            currentCustomer.FirstName = dataContextCustomer.FirstName;
            currentCustomer.LastName = dataContextCustomer.LastName;
            currentCustomer.Id = dataContextCustomer.Id;
            IQueryable transQuery = from Transaction trans in db.Transactions
                                    from TranHistoryLinkedTable linkedTrans in db.TranHistoryLinkedTables
                                    where (trans.TransactionID == linkedTrans.TransID && linkedTrans.CustomerID == currentCustomer.Id)
                                    select trans;
            foreach (Transaction trans in transQuery)
            {
                currentCustomer.TranHistory.Add(TransactionConverterToBackend(trans));
            }
            return currentCustomer;
        }

        public Customer CustomerConverterToContext(Backend.Customer currentCustomer)
        {
            CreditCard dataContextCreditCard = new CreditCard();
            Customer dataContextCustomer = new Customer();

            //Credit Card Entity
            if (currentCustomer.CreditCard != null)
            {
                dataContextCreditCard.CreditNumber = currentCustomer.CreditCard.CreditNumber;
                dataContextCreditCard.ExpirationDate = currentCustomer.CreditCard.ExpirationDate;
                dataContextCreditCard.FirstName = currentCustomer.CreditCard.FirstName;
                dataContextCreditCard.LastName = currentCustomer.CreditCard.LastName;
                dataContextCustomer.CreditCard1 = dataContextCreditCard;
                dataContextCustomer.CreditCard = currentCustomer.CreditCard.CreditNumber;
            }
            
            //Customer Entity
            dataContextCustomer.IsAClubMember = false; 
            dataContextCustomer.FirstName = currentCustomer.FirstName;
            dataContextCustomer.LastName = currentCustomer.LastName;
            dataContextCustomer.Id = currentCustomer.Id;
            return dataContextCustomer;
        }


        //Transaction
        public Backend.Transaction TransactionConverterToBackend(Transaction dataContextTransaction)
        {
            Backend.Transaction currentTransaction = new Backend.Transaction();
            currentTransaction.CurrentDate = dataContextTransaction.CurrentDate;
            currentTransaction.Is_a_Return = (Is_a_return)dataContextTransaction.Is_a_Return;
            currentTransaction.Payment = (PaymentMethod)dataContextTransaction.Payment;
            currentTransaction.TransactionID = dataContextTransaction.TransactionID;
            IQueryable receiptQuery = from Purchase purch in db.Purchases
                                      where purch.TransID == dataContextTransaction.TransactionID
                                      select purch;
            foreach (Purchase purch in receiptQuery){
                currentTransaction.Receipt.Add(PurchaseConverterToBackend(purch));
            }
            return currentTransaction;
        }

        public Transaction TransactionConverterToContext(Backend.Transaction currentTransaction)
        {
            Transaction dataContextTransaction = new Transaction();
            dataContextTransaction.CurrentDate = currentTransaction.CurrentDate;
            dataContextTransaction.Is_a_Return = (int)currentTransaction.Is_a_Return;
            dataContextTransaction.Payment = (int)currentTransaction.Payment;
            dataContextTransaction.TransactionID = currentTransaction.TransactionID;
            dataContextTransaction.Receipt = 0;
            IQueryable receiptQuery = from Purchase purch in db.Purchases
                                      where purch.TransID == dataContextTransaction.TransactionID
                                      select purch;
            foreach (Purchase purch in receiptQuery)
            {
                dataContextTransaction.Receipt++;
            }
            return dataContextTransaction;
        }


        //Purchase
        public Backend.Purchase PurchaseConverterToBackend(Purchase dataContextPurchase)
        {
            Backend.Purchase currentPurchase = new Backend.Purchase();
            currentPurchase.Amount = dataContextPurchase.Amount;
            currentPurchase.PrdID = dataContextPurchase.PrdID;
            currentPurchase.PrdName = dataContextPurchase.PrdName;
            currentPurchase.Price = dataContextPurchase.Price;
            currentPurchase.TimeOfPurchase = dataContextPurchase.TimeOfPurchase;
            currentPurchase.TransID = dataContextPurchase.TransID;
            return currentPurchase;
        }

        public Purchase PurchaseConverterToContext(Backend.Purchase currentPurchase)
        {
            Purchase dataContextPurchase = new Purchase();
            dataContextPurchase.Amount = currentPurchase.Amount;
            dataContextPurchase.PrdID = currentPurchase.PrdID;
            dataContextPurchase.PrdName = currentPurchase.PrdName;
            dataContextPurchase.Price = currentPurchase.Price;
            dataContextPurchase.TimeOfPurchase = currentPurchase.TimeOfPurchase;
            dataContextPurchase.TransID = currentPurchase.TransID;
            return dataContextPurchase;
        }


        //Club Member
        public Backend.ClubMember ClubMemberConverterToBackend(ClubMember dataContextClubMember)
        {
            Backend.ClubMember currentClubMember = new Backend.ClubMember();

            //Credit Card Entity
            if (dataContextClubMember.Customer.CreditCard != null)
            {
                currentClubMember.CreditCard.CreditNumber = dataContextClubMember.Customer.CreditCard1.CreditNumber;
                currentClubMember.CreditCard.ExpirationDate = dataContextClubMember.Customer.CreditCard1.ExpirationDate;
                currentClubMember.CreditCard.FirstName = dataContextClubMember.Customer.CreditCard1.FirstName;
                currentClubMember.CreditCard.LastName = dataContextClubMember.Customer.CreditCard1.LastName;
            }
            else
            {
                currentClubMember.CreditCard = null;
            }

            //Customer Entity
            currentClubMember.FirstName = dataContextClubMember.Customer.FirstName;
            currentClubMember.LastName = dataContextClubMember.Customer.LastName;
            currentClubMember.Id = dataContextClubMember.Customer.Id;
            IQueryable transQuery = from Transaction trans in db.Transactions
                                    from TranHistoryLinkedTable linkedTrans in db.TranHistoryLinkedTables
                                    where (trans.TransactionID == linkedTrans.TransID && linkedTrans.CustomerID == currentClubMember.Id)
                                    select trans;
            foreach (Transaction trans in transQuery)
            {
                currentClubMember.TranHistory.Add(TransactionConverterToBackend(trans));
            }

            //Clubmember Entity
            currentClubMember.DateOfBirth = dataContextClubMember.DateOfBirth;
            currentClubMember.Gender = (Gender)dataContextClubMember.Gender;
            currentClubMember.MemberID = dataContextClubMember.MemberID;
            return currentClubMember;
        }

        public ClubMember ClubMemberConverterToContext(Backend.ClubMember currentClubMember)
        {
            CreditCard dataContextCreditCard = new CreditCard();
            ClubMember dataContextClubMember = new ClubMember();

            //Customer Entity
            Customer currtCusAsClub = new Customer();
            currtCusAsClub.IsAClubMember = true;
            currtCusAsClub.FirstName = currentClubMember.LastName;
            currtCusAsClub.LastName = currentClubMember.LastName;
            currtCusAsClub.Id = currentClubMember.Id;
            dataContextClubMember.Customer = currtCusAsClub;
            //Credit Card Entity
            if (currentClubMember.CreditCard != null)
            {
                dataContextCreditCard.CreditNumber = currentClubMember.CreditCard.CreditNumber;
                dataContextCreditCard.ExpirationDate = currentClubMember.CreditCard.ExpirationDate;
                dataContextCreditCard.FirstName = currentClubMember.CreditCard.FirstName;
                dataContextCreditCard.LastName = currentClubMember.CreditCard.LastName;
                dataContextClubMember.Customer.CreditCard1 = dataContextCreditCard;
                dataContextClubMember.Customer.CreditCard = currentClubMember.CreditCard.CreditNumber;
            }
            //Clubmember Entity
            dataContextClubMember.DateOfBirth = currentClubMember.DateOfBirth;
            dataContextClubMember.Gender = (int)currentClubMember.Gender;
            dataContextClubMember.MemberID = currentClubMember.MemberID;
            return dataContextClubMember;
        }


        //User
        public Backend.User UserConverterToBackend(User dataContextUser)
        {
            Backend.User currentUser = new Backend.User();
            currentUser.UserName = dataContextUser.UserName;
            currentUser.Password = dataContextUser.Password;
            if (dataContextUser.PersonAsClubMember != null){
                IQueryable personQuery = from ClubMember person in db.ClubMembers
                                         where person.Id == dataContextUser.PersonID
                                         select person;
                currentUser.Person = ClubMemberConverterToBackend(personQuery.Cast<ClubMember>().ToList().ElementAt(0));
            }
            else if (dataContextUser.PersonAsCustomer != null)
            {
                IQueryable personQuery = from Customer person in db.Customers
                                         where person.Id == dataContextUser.PersonID
                                         select person;
                currentUser.Person = CustomerConverterToBackend(personQuery.Cast<Customer>().ToList().ElementAt(0));
            }
            else if (dataContextUser.PersonAsEmployee != null)
            {
                IQueryable personQuery = from Employee person in db.Employees
                                         where person.Id == dataContextUser.PersonID
                                         select person;
                currentUser.Person = EmployeeConverterToBackend(personQuery.Cast<Employee>().ToList().ElementAt(0));
            }
            return currentUser;
            
        }

        public User UserConverterToContext(Backend.User currentUser)
        {
            User dataContextUser = new User();
            dataContextUser.Password = currentUser.Password;
            dataContextUser.UserName = currentUser.UserName;
            if (currentUser.Person is Backend.ClubMember)
            {
                dataContextUser.PersonAsClubMember = ((Backend.ClubMember)currentUser.Person).Id;
                dataContextUser.PersonID = ((Backend.ClubMember)currentUser.Person).Id;
            }
            else if (currentUser.Person is Backend.Customer)
            {
                dataContextUser.PersonAsCustomer = ((Backend.Customer)currentUser.Person).Id;
                dataContextUser.PersonID = ((Backend.Customer)currentUser.Person).Id;
            }
            else if (currentUser.Person is Backend.Employee)
            {
                dataContextUser.PersonAsEmployee = ((Backend.Employee)currentUser.Person).Id;
                dataContextUser.PersonID = ((Backend.Employee)currentUser.Person).Id;
            }
            return dataContextUser;
        }
            

    }
}
