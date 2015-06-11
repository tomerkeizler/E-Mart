using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend;
using DAL;
using System.Collections.ObjectModel;

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

        public object Add(object p)
        {
            List<Backend.Product> Allprods = itsDAL.ReadFromFile(Elements.Product).Cast<Backend.Product>().ToList();
            //Generate the new product ID
            int maxID = 0;
            List<Department> Alldeparts = itsDAL.ReadFromFile(Elements.Department).Cast<Department>().ToList();
            bool checkID = false;
            //check id the product's department accually exists
            foreach (Department dep in Alldeparts)
            {
                if (((Backend.Product)p).Location == dep.DepartmentID)
                {
                    checkID = true;
                    break;
                }
            }
            if (!checkID)
                throw new Exception("department ID doesn't exist!");
            foreach (Backend.Product prod in Allprods)
            {
                if (prod.ProductID > maxID)
                    maxID = prod.ProductID;
                if (((Backend.Product)p).ProductID != 0 && ((Backend.Product)p).ProductID == prod.ProductID)
                {
                    throw new System.Data.DataException("The ID allready exist in the system");
                }
            }
            if (((Backend.Product)p).ProductID == 0)
            {
                //set the new ID
                ((Backend.Product)p).ProductID = maxID + 1;
            }
            //Add the new product to the system
            Allprods.Add((Backend.Product)p);
            itsDAL.WriteToFile(Allprods.Cast<object>().ToList(), (Backend.Product)p);
            return p;
        }
        public void Remove(object p, Boolean isEdit = false)
        {
            List<Backend.Product> Allprods = itsDAL.ReadFromFile(Elements.Product).Cast<Backend.Product>().ToList();
            //check if there are any products to remove
            if (!Allprods.Any())
                throw new NullReferenceException("No Products to remove!");
            else
            {
                //find and remove product
                foreach (Backend.Product prod in Allprods)
                {
                    if (prod.Equals(p))
                    {
                        Allprods.Remove(prod);
                        break;
                    }
                }
                itsDAL.WriteToFile(Allprods.Cast<object>().ToList(), p);
            }
        }
        public void Edit(object oldP, object newP)
        {
            //preserve the id for the edited product
            ((Backend.Product)newP).ProductID = ((Backend.Product)oldP).ProductID;
            this.Remove(oldP);
            this.Add(newP);
        }
        public List<object> FindByName(string name, StringFields field)
        {
            //search method by string
            if (name == null)
                throw new System.Data.DataException("Bad Input!");
            List<object> result = itsDAL.ProductNameQuery(name, field).Cast<object>().ToList();
            return result;
        }


        public List<object> FindByNumber(IntFields field, int minNumber, int maxNumber)
        {
            //search method by number
            return itsDAL.ProductNumberQuery(minNumber,maxNumber, field).Cast<object>().ToList();
        }

        public List<object> FindByType(ValueType type)
        {
            //search method by type
            return itsDAL.ProductTypeQuery(type).Cast<object>().ToList();
        }


        public List<object> GetAll()
        {
            //return all products
            return itsDAL.ReadFromFile(Elements.Product);
        }

        public Type GetEntityType()
        {
            //return the prudct type
            return typeof(Backend.Product);
        }
        public string GetEntityName()
        {
            //return the Product type as a string
            return "Product";
        }

        public void GenerateTopSeller()
        {
            int currMonth = DateTime.Today.Month;
            List<Backend.Product> Allprods = itsDAL.ReadFromFile(Elements.Product).Cast<Backend.Product>().ToList();
            int currMax = 1;
            foreach (Backend.Product prod in Allprods)
            {
                if (prod.TopSellerMonth != currMonth)
                {
                    prod.ResetSells();
                }
                if (prod.SellCounter >= currMax)
                {
                    currMax = prod.SellCounter;
                }
            }
            foreach (Backend.Product prod in Allprods)
            {
                if (prod.SellCounter == currMax)
                {
                    prod.IsTopSeller = true;
                }
                else
                {
                    prod.IsTopSeller = false;
                }
            }
            itsDAL.WriteToFile(Allprods.Cast<object>().ToList(), new Backend.Product());
        }
        public void FilterProducts(ObservableCollection<Buyable> currentList, PType type, bool isAdd)
        {
            if (currentList == null)
            {
                throw new System.Data.DataException("Bad Input!");
            }
            itsDAL.FilterProducts(currentList, type, isAdd);
        }
    }
}
