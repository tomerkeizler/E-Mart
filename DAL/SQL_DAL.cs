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
    class SQL_DAL : IDAL
    {
        //Fields
        IQueryable prodQuery;
        E_MartDB_LINQtoSQLDataContext db;

        public SQL_DAL()
        {
            db = new E_MartDB_LINQtoSQLDataContext();
            prodQuery = from Product prod in db.Products
                        select prod;
        }
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
                db.SubmitChanges();
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
        }

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

        public void FilterProducts(System.Collections.ObjectModel.ObservableCollection<Backend.Buyable> currentList, Backend.PType type, bool isAdd)
        {
            throw new NotImplementedException();
        }

        public List<Backend.Employee> EmployeeNameQuery(string name, Backend.StringFields field)
        {
            throw new NotImplementedException();
        }

        public List<Backend.Employee> EmployeeNumberQuery(int minNumber, int maxNumber, Backend.IntFields field)
        {
            throw new NotImplementedException();
        }

        public List<Backend.Employee> EmployeeTypeQuery(ValueType type)
        {
            throw new NotImplementedException();
        }

        public List<Backend.ClubMember> ClubMemberNameQuery(string name, Backend.StringFields field)
        {
            throw new NotImplementedException();
        }

        public List<Backend.ClubMember> ClubMemberNumberQuery(int minNumber, int maxNumber, Backend.IntFields field)
        {
            throw new NotImplementedException();
        }

        public List<Backend.ClubMember> ClubMemberTypeQuery(ValueType type)
        {
            throw new NotImplementedException();
        }

        public List<Backend.Customer> CustomerNameQuery(string name, Backend.StringFields field)
        {
            throw new NotImplementedException();
        }

        public List<Backend.Customer> CustomerNumberQuery(int minNumber, int maxNumber, Backend.IntFields field)
        {
            throw new NotImplementedException();
        }

        public List<Backend.Department> DepartmentNameQuery(string name, Backend.StringFields field)
        {
            throw new NotImplementedException();
        }

        public List<Backend.Department> DepartmentNumberQuery(int minNumber, int maxNumber, Backend.IntFields field)
        {
            throw new NotImplementedException();
        }

        public List<Backend.Transaction> TransactionNumberQuery(int minNumber, int maxNumber, Backend.IntFields field)
        {
            throw new NotImplementedException();
        }

        public List<Backend.Transaction> TransactionTypeQuery(ValueType type)
        {
            throw new NotImplementedException();
        }

        public List<Backend.User> UserNameQuery(string name, Backend.StringFields field)
        {
            throw new NotImplementedException();
        }

        public List<Backend.User> UserTypeQuery(ValueType type)
        {
            throw new NotImplementedException();
        }

        public List<Backend.User> UserPersonQuery(object person)
        {
            throw new NotImplementedException();
        }

        public Backend.Product ProductConverterToBackend(Product dataContextProduct)
        {
            Backend.Product currentProduct = new Backend.Product();//ADDINSTEADOFBELOW
            currentProduct.Name = dataContextProduct.Name;
            currentProduct.Type = (Backend.PType)dataContextProduct.Type;
            currentProduct.ProductID = dataContextProduct.ProductID;
            currentProduct.Location = dataContextProduct.Location;
            currentProduct.InStock = (Backend.PStatus)dataContextProduct.InStock;
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
    }
}
