using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public enum Is_a_return { Return, Purchase };
    public enum PaymentMethod { Cash, Check, Visa };
    public class Transaction
    {
        //Fields:
        private int transactionID;
        private int dateTime;
        private Is_a_return is_a_return;
        private Receipt receipt;
        private PaymentMethod payment;

        //Constructors:
        public Transaction(int _transactionID, int _dateTime, Is_a_return _is_a_return, Receipt _receipt, PaymentMethod _payment)
        {
            transactionID = _transactionID;
            dateTime = _dateTime;
            is_a_return = _is_a_return;
            receipt = _receipt;
            payment = _payment;
        }
        public string ToString()
        {
            return transactionID+"";
        }
        //getters and setters:
        public int Id
        {
            get { return transactionID; }
            set { transactionID = value; }
        }
        public int DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }
        public Is_a_return Is_a_Return
        {
            get { return is_a_return; }
            set { is_a_return = value; }
        }
        public Receipt Receipt
        {
            get { return receipt; }
            set { receipt = value; }
        }
        public PaymentMethod Payment
        {
            get { return payment; }
            set { payment = value; }
        }
    }
}
