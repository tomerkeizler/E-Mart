using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    [Serializable()]
    public class CreditCard
    {
        //fields:
        private string firstName;
        private string lastName;
        private int creditNumber;
        private DateTime expirationDate;
        //constructors:
        public CreditCard() { }
        public CreditCard(string _firstName, string _lastName, int _creditNumber, DateTime _expirationDate)
        {
            firstName = _firstName;
            lastName = _lastName;
            creditNumber = _creditNumber;
            expirationDate = _expirationDate;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is CreditCard)) return false;
            CreditCard other = (CreditCard)obj;
            return (firstName.Equals(other.FirstName) && lastName.Equals(other.LastName) && creditNumber == other.CreditNumber
                    && expirationDate.Equals(other.ExpirationDate));
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ creditNumber;
        }
        public override string ToString()
        {
            return "Number: " + creditNumber.ToString() + "\nExpiration Date: " + expirationDate.ToShortDateString();
        }

        //getters and setters:
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        public int CreditNumber
        {
            get { return creditNumber; }
            set { creditNumber = value; }
        }
        public DateTime ExpirationDate
        {
            get { return expirationDate; }
            set { expirationDate = value; }
        }
    }
}
