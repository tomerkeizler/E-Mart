﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public enum Is_a_return { Return, Purchase };
    public enum PaymentMethod { Cash, Check, Visa };
    [Serializable()]
    public class Transaction
    {
        //Fields:
        private int transactionID;
        private DateTime currentDate;
        private Is_a_return is_a_return;
        private List<Purchase> receipt;
        private PaymentMethod payment;

        //Constructors:
        public Transaction()
        {
            receipt = new List<Purchase>();
        }
        public Transaction(int _transactionID, Is_a_return _is_a_return, List<Purchase> _receipt, PaymentMethod _payment)
        {
            transactionID = _transactionID;
            currentDate = DateTime.Today;
            is_a_return = _is_a_return;
            receipt = new List<Purchase>(_receipt);
            payment = _payment;
        }
        //For Deep Copy
        public Transaction(Transaction other)
        {
            transactionID = other.transactionID;
            currentDate = other.currentDate;
            is_a_return = other.is_a_return;
            receipt = new List<Purchase>(other.receipt);
            payment = other.payment;
        }
        public override bool Equals(object _other)
        {
            if (!(_other is Transaction)) return false;
            Transaction other = (Transaction)_other;
            return (transactionID == other.transactionID && currentDate.Equals(other.currentDate) && is_a_return.Equals(other.is_a_return)
                    && receipt.SequenceEqual(other.receipt) && payment.Equals(other.payment));
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ transactionID.GetHashCode();
        }
        public override string ToString()
        {
            return transactionID+"";
        }
        //getters and setters:
        public int TransactionID
        {
            get { return transactionID; }
            set { transactionID = value; }
        }
        public Is_a_return Is_a_Return
        {
            get { return is_a_return; }
            set { is_a_return = value; }
        }
        public PaymentMethod Payment
        {
            get { return payment; }
            set { payment = value; }
        }
        public List<Purchase> Receipt
        {
            get { return receipt; }
            set { receipt = value; }
        }
        public DateTime CurrentDate
        {
            get { return currentDate; }
            set { currentDate = value; }
        }
    }
}
