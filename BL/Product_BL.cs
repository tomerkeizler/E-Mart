using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend;
using DAL;

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

        public void Add(object p)
        {
            //First generate the new product ID
            List<Product> Allprods = itsDAL.ReadFromFile(Elements.Product).Cast<Product>().ToList();
            int maxID = 0;
            foreach (Product prod in Allprods)
            {
                if (prod.ProductID > maxID)
                    maxID = prod.ProductID;
            }
            //set the new ID
            ((Product)p).ProductID = maxID++;
            //Add the new product to the system
            Allprods.Add((Product)p);
            itsDAL.WriteToFile(Allprods.Cast<object>().ToList());
        }
        public void Remove(object p)
        {
            List<Product> Allprods = itsDAL.ReadFromFile(Elements.Product).Cast<Product>().ToList();
            foreach (Product prod in Allprods)
            {
                if (prod.Equals(p))
                {
                    Allprods.Remove(prod);
                    break;
                }
            }
            itsDAL.WriteToFile(Allprods.Cast<object>().ToList());
        }
        public void Edit(object oldP, object newP)
        {
            List<Product> Allprods = itsDAL.ReadFromFile(Elements.Product).Cast<Product>().ToList();
            ((Product)newP).ProductID = ((Product)oldP).ProductID;
            Allprods.Remove((Product)oldP);
            Allprods.Add((Product)newP);
            itsDAL.WriteToFile(Allprods.Cast<object>().ToList());
        }
        public List<object> FindByName(string name, StringFields field)
        {
            if (name == null || field != StringFields.name)
                throw new System.Data.DataException("Bad Input!");
            List<object> result = itsDAL.ProductNameQuery(name).Cast<object>().ToList();
            return result;
        }


        public List<object> FindByNumber(int num, IntFields field)
        {
            List<object> result;
            if (field == IntFields.price)
            {
                result = itsDAL.ProductPriceQuery(num).Cast<object>().ToList();
            }
            else if (field == IntFields.productID)
            {
                result = itsDAL.ProductIDQuery(num).Cast<object>().ToList();
            }
            else if (field == IntFields.location)
            {
                result = itsDAL.ProductLocationQuery(num).Cast<object>().ToList();
            }
            else if (field == IntFields.stockCount)
            {
                result = itsDAL.ProductStockCountQuery(num).Cast<object>().ToList();
            }
            else
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return result;
        }
        public List<object> FindByType(object type)
        {
            if (!(type is PType))
            {
                throw new System.Data.DataException("Bad Input!");
            }
            return itsDAL.ProductTypeQuery((PType)type).Cast<object>().ToList();
        }


        public List<object> GetAll(Elements element)
        {
            return itsDAL.ReadFromFile(element);
        }
    }
}
