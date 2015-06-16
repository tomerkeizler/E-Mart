﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Backend
{
    [Serializable()]
    public class Customer
    {
        //Fields:
        protected int id;
        protected string firstName;
        protected string lastName;
        protected List<Transaction> tranHistory;
        protected CreditCard creditCard;
        
        //Constructors:
        public Customer()
        {
            creditCard = new CreditCard();
            tranHistory = new List<Transaction>();
        }
        public Customer(int _id, string _firstName, string _lastName, CreditCard _creditCard = null)
        {
            id = _id;
            firstName = _firstName;
            lastName = _lastName;
            tranHistory = new List<Transaction>();
            creditCard = _creditCard;
        }
        //For Deep Copy
        public Customer(Customer other)
        {
            id = other.Id;
            firstName = other.FirstName;
            lastName = other.LastName;
            tranHistory = new List<Transaction>(other.TranHistory);
            creditCard = other.creditCard;
        }

        //Getters and Setters:
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
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
        public List<Transaction> TranHistory
        {
            get { return tranHistory; }
            set { tranHistory = value; }
        }
        public CreditCard CreditCard
        {
            get { return creditCard; }
            set { creditCard = value; }
        }

        public override bool Equals(object _other)
        {
            if (!(_other is Customer)) return false;
            Customer other = (Customer)_other;
            if (creditCard != null)
                return (id == other.Id && firstName.Equals(other.firstName) && lastName.Equals(other.lastName)
                        && creditCard.Equals(other.CreditCard));
            else
                return (id == other.Id && firstName.Equals(other.firstName) && lastName.Equals(other.lastName)
                        &&  other.creditCard == null);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ id.GetHashCode();
        }
        public override string ToString()
        {
            return "Customer -" + firstName + " " + lastName;
        }
    }
}
