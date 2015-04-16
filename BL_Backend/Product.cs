﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend
{
    public enum PType {a, b ,c};
    public enum PStatus {Empty, LowQuantity, InStock};
    [Serializable()]
    public class Product
    {
        //Fields:
        private string name;
        private PType type;
        private int price;
        private int stockCount;
        private PStatus inStock;
        private int location;
        private int productID = 0;
        //Constructors:
        public Product() { }
        public Product(string _name, PType _type, int _location, PStatus _inStock, int _stockCount, int _price, int _productID = 0)
        {
            name = _name;
            type = _type;
            productID = _productID;
            location = _location;
            inStock = _inStock;
            stockCount = _stockCount;
            price = _price;
        }
        public override string ToString()
        {
            return name;
        }
        //Getter and Setters:
        public int ProductID
        {
            get { return productID; }
            set { productID = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public PType Type
        {
            get { return type; }
            set { type = value; }
        }

        public int Location
        {
            get { return location; }
            set { location = value; }
        }

        public PStatus InStock
        {
            get { return inStock; }
            set { inStock = value; }
        }

        public int StockCount
        {
            get { return stockCount; }
            set { stockCount = value; }
        }


        public int Price
        {
            get { return price; }
            set { price = value; }
        }
    }
}
