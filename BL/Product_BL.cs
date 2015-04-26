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
            List<Product> Allprods = itsDAL.ReadFromFile(Elements.Product).Cast<Product>().ToList();
            //Generate the new product ID
            int maxID = 0;
            List<Department> Alldeparts = itsDAL.ReadFromFile(Elements.Department).Cast<Department>().ToList();
            bool checkID = false;
            foreach (Department dep in Alldeparts)
            {
                if (((Product)p).Location == dep.DepartmentID)
                {
                    checkID = true;
                    break;
                }
            }
            if (!checkID)
                throw new Exception("department ID doesn't exist!");
            foreach (Product prod in Allprods)
            {
                if (prod.ProductID > maxID)
                    maxID = prod.ProductID;
                if (((Product)p).ProductID != 0 && ((Product)p).ProductID == prod.ProductID)
                {
                    throw new System.Data.DataException("The ID allready exist in the system");
                }
            }
            if (((Product)p).ProductID == 0)
            {
                //set the new ID
                ((Product)p).ProductID = maxID + 1;
            }
            //Add the new product to the system
            Allprods.Add((Product)p);
            itsDAL.WriteToFile(Allprods.Cast<object>().ToList(), (Product)p);
        }
        public void Remove(object p)
        {
            List<Product> Allprods = itsDAL.ReadFromFile(Elements.Product).Cast<Product>().ToList();
            if (!Allprods.Any())
                throw new NullReferenceException("No Products to remove!");
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
                itsDAL.WriteToFile(Allprods.Cast<object>().ToList(), p);
            }
        }
        public void Edit(object oldP, object newP)
        {
            ((Product)newP).ProductID = ((Product)oldP).ProductID;
            this.Remove(oldP);
            this.Add(newP);
        }
        public List<object> FindByName(string name, StringFields field)
        {
            if (name == null)
                throw new System.Data.DataException("Bad Input!");
            List<object> result = itsDAL.ProductNameQuery(name, field).Cast<object>().ToList();
            return result;
        }


        public List<object> FindByNumber(IntFields field, int minNumber, int maxNumber)
        {
            return itsDAL.ProductNumberQuery(minNumber,maxNumber, field).Cast<object>().ToList();
        }

        public List<object> FindByType(ValueType type)
        {
            return itsDAL.ProductTypeQuery(type).Cast<object>().ToList();
        }


        public List<object> GetAll()
        {
            return itsDAL.ReadFromFile(Elements.Product);
        }

        public Type GetEntityType()
        {
            return typeof(Product);
        }
    }
}
