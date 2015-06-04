using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    [Serializable()]
    public class Buyable
    {
        // attributes
        private Product prod;
        private int amount;
        private int leftInStock;

        // constructors
        public Buyable(Product _prod, int _amount, int _leftInStock)
        {
            prod = _prod;
            amount = _amount;
            leftInStock = _leftInStock;
        }

        public Buyable(Buyable other)
        {
            prod = other.prod;
            amount = other.amount;
            leftInStock = other.leftInStock;
        }

        // getters ans setters
        public Product Prod
        {
            get { return prod; }
            set { prod = value; }
        }

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public int LeftInStock
        {
            get { return leftInStock; }
            set { leftInStock = value; }
        }

        // methods
        public override bool Equals(object other)
        {
            if (!(other is Buyable))
                return false;
            Buyable b = (Buyable)other;
            return prod.Equals(b.prod) && amount == b.amount && leftInStock == b.leftInStock;
        }

        public void ZeroAmount()
        {
            this.amount = 0;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ prod.ProductID.GetHashCode();
        }



    }
}
