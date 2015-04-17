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
            if (!Allprods.Any())
                throw new NullReferenceException("No Employees to remove!");
            else
            {
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
            if (name == null)
                throw new System.Data.DataException("Bad Input!");
            List<object> result = itsDAL.ProductNameQuery(name, field).Cast<object>().ToList();
            return result;
        }


        public List<object> FindByNumber(int number, IntFields field)
        {
            return itsDAL.ProductNumberQuery(number, field).Cast<object>().ToList();
        }

        public List<object> FindByType(ValueType type)
        {
            return itsDAL.ProductTypeQuery(type).Cast<object>().ToList();
        }


        public List<object> GetAll(Elements element)
        {
            return itsDAL.ReadFromFile(element);
        }
    }
}
