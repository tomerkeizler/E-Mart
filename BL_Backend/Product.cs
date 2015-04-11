using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend
{
    public class Product
    {
        public int ProductID = 0;
        public string Name;
        public string Type;

        public Product(string _name, string _type, int _productid=0)
        {
            Name = _name;
            Type = _type;
            ProductID = _productid;
        }

    }
}
