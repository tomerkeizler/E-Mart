using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend;
using DAL;

namespace BL
{
    public interface IBL
    {
        void AddProduct(Product p);
        List<Product> FindProductByName(string name);
    }
}
