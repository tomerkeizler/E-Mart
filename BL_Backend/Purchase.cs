using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    [Serializable()]
    public class Purchase
    {
        // attributes
        private int prdID;
        private string prdName;
        private int price;
        private int amount;

        // constructors
        public Purchase(int _prdID, string _prdName, int _price, int _amount)
        {
            prdID = _prdID;
            prdName = _prdName;
            price = _price;
            amount = _amount;
        }

        public Purchase(Purchase other)
        {
            prdID = other.prdID;
            prdName = other.prdName;
            price = other.price;
            amount = other.amount;
        }

        // getters ans setters
        public int PrdID
        {
            get { return prdID; }
            set { prdID = value; }
        }

        public string PrdName
        {
            get { return prdName; }
            set { prdName = value; }
        }

        public int Price
        {
            get { return price; }
            set { price = value; }
        }

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        // methods
        public override bool Equals(object other)
        {
            if (!(other is Purchase))
                return false;
            Purchase p = (Purchase)other;
            return prdID == p.prdID && prdName.Equals(p.prdName) && price.Equals(p.price) && amount.Equals(p.amount);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ prdID;
        }



    }
}
