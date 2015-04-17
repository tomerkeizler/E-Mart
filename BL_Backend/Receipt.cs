using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    class Receipt
    {
        //Fields:
        private List<int> productsIDs;
        private List<int> prices;

        //Constructors:
        public Receipt(List<Product> _products)
        {
            foreach(Product prod in _products)
            {
                productsIDs.Add(prod.ProductID);
                prices.Add(prod.Price);
            }
        }
        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < productsIDs.Count; i++ )
            {
                str = str + productsIDs[i] + "    " + prices[i] + "\n";
            }
            return str;
        }

        //getters and setters:
        public List<int> ProductsIDs
        {
            get { return productsIDs; }
            set { productsIDs = value; }
        }
        public List<int> Prices
        {
            get { return prices; }
            set { prices = value; }
        }

        //methods:
        public void Add(Product product)
        {
            productsIDs.Add(product.ProductID);
            prices.Add(product.Price);
        }
        public void Remove(Product product)
        {
            if (productsIDs.Contains(product.ProductID))
            {
                int index = productsIDs.IndexOf(product.ProductID);
                productsIDs.Remove(index);
                prices.Remove(index);
            }
            else
            {
                throw new IndexOutOfRangeException("There is not products like this in the Reciept");
            }
        }
    }
}
