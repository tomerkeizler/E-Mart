using System;
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
            stockCount = _stockCount;
            price = _price;
            if(stockCount==0)
                inStock = PStatus.Empty;
            else if (stockCount <= 20)
                inStock = PStatus.LowQuantity;
            else
                inStock = PStatus.InStock;
        }
        //For Deep Copy
        public Product(Product other)
        {
            name = other.name;
            type = other.type;
            productID = other.productID;
            location = other.location;
            inStock = other.inStock;
            stockCount = other.stockCount;
            price = other.price;
        }
        public override string ToString()
        {
            return name;
        }
        public override bool Equals(object _other)
        {
            if (!(_other is Product)) return false;
            Product other = (Product)_other;
            return (name.Equals(other.name) && type.Equals(other.type) && productID == other.productID && inStock.Equals(other.inStock) &&
                    stockCount == other.stockCount && price == other.price);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ name.GetHashCode();
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
            set { stockCount = value;
                if(value==0)
                    inStock = PStatus.Empty;
                else if (value <= 20)
                    inStock = PStatus.LowQuantity;
                else
                    inStock = PStatus.InStock;
            }
        }


        public int Price
        {
            get { return price; }
            set { price = value; }
        }
    }
}