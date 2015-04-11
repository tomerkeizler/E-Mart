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

        public void AddProduct(Backend.Product p)
        {
            //First generate the new product ID
            List<Product> Allprods = itsDAL.GetAllProducts();
            int maxID = 0;
            foreach (Product prod in Allprods)
            {
                if (prod.ProductID > maxID)
                    maxID = prod.ProductID;
            }
            //set the new ID
            p.ProductID = maxID++;
            //Add the new product to the system
            itsDAL.AddProduct(p);
        }

        public List<Product> FindProductByName(string name)
        {
            return itsDAL.ProductNameQuery(name);
        }
    }
}
