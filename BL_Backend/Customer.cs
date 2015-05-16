using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class Customer
    {
        //Fields:
        private int id;
        private string firstName;
        private string lastName;
        private List<Transaction> tranHistory;

        
        //Constructors:
        public Customer(int _id, string _firstName, string _lastName, List<Transaction> _tranHistory)
        {
            id = _id;
            firstName = _firstName;
            lastName = _lastName;
            tranHistory = _tranHistory;
        }
        //For Deep Copy
        public Customer(Customer other)
        {
            id = other.Id;
            firstName = other.FirstName;
            lastName = other.LastName;
            tranHistory = other.TranHistory;
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

    }
}
