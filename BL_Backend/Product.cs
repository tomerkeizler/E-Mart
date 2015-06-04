using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend
{
    public enum PType { Electronics, Clothes, Food };
    public enum PStatus {Empty, LowQuantity, InStock};
    [Serializable()]
    public class Product
    {
        //TopSeller Private Class for Generate the TopSeller Product each month
        [Serializable()]
        private class TopSeller
        {
            private string productName;
            private bool isTopSeller = false;
            private int sellCounter = 0;
            private int currentMonth;
            public TopSeller(string _productName)
            {
                productName = _productName;
                currentMonth = DateTime.Today.Month;
            }
            public int SellCounter
            {
                get { return sellCounter; }
                set { sellCounter = value; }
            }
            public bool IsTopSeller
            {
                get { return isTopSeller; }
                set { isTopSeller = value; }
            }
            public int CurrentMonth
            {
                get { return currentMonth; }
                set { currentMonth = value; }
            }
        }

        //Fields:
        private string name;
        private PType type;
        private int price;
        private int stockCount;
        private PStatus inStock;
        private int location;
        private int productID = 0;
        private TopSeller topSellerStatus;

        //Constructors:
        public Product() { }
        public Product(string _name, PType _type, int _location, int _stockCount, int _price, int _productID = 0)
        {
            topSellerStatus = new TopSeller(_name);
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
            topSellerStatus = other.topSellerStatus;
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
        //Methods
        public void Buy(int num = 1)
        {
            if (this.inStock == PStatus.Empty)
            {
                throw new InvalidOperationException("No Product Left to buy!");
            }
            this.StockCount = this.StockCount - num;

            if (stockCount == 0)
                inStock = PStatus.Empty;
            else if (stockCount <= 20)
                inStock = PStatus.LowQuantity;
            else
                inStock = PStatus.InStock;

            this.topSellerStatus.SellCounter = this.topSellerStatus.SellCounter + num;
        }
        public void ResetSells()
        {
            topSellerStatus = new TopSeller(name);
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
        public bool IsTopSeller
        {
            get { return topSellerStatus.IsTopSeller; }
            set { topSellerStatus.IsTopSeller = value; }
        }
        public int TopSellerMonth
        {
            get { return topSellerStatus.CurrentMonth; }
            set {topSellerStatus.CurrentMonth = value; }
        }
        public int SellCounter
        {
            get { return topSellerStatus.SellCounter; }
        }
        public int Price
        {
            get { return price; }
            set { price = value; }
        }
    }
}