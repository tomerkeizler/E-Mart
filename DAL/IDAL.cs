using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend;

namespace DAL
{
    public interface IDAL
    {
         void AddProduct(Product p);
         List<Product> ProductNameQuery(string name);
         List<Product> GetAllProducts();
    }
}
