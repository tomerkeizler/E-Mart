using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    class CreditCard
    {
        //fields:
        private string firstName;
        private string lastName;
        private int creditNumber;
        private DateTime expirationDate;
        //constructors:
        public CreditCard(string _firstName, string _lastName, int _creditNumber, DateTime _expirationDate)
        {
            firstName = _firstName;
            lastName = _lastName;
            creditNumber = _creditNumber;
            expirationDate = _expirationDate;
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
