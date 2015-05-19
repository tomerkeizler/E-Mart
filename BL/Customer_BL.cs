using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Backend;
using DAL;

namespace BL
{
    public class Customer_BL
    {
         //Fields:
        IDAL itsDAL;

        //Constructors:
        public Customer_BL(IDAL dal)
        {
            itsDAL = dal;
        }

        //Methods:
        public void Add(object c)
        {
            //Add the new employee to the system
            List<Customer> Allcustomers = itsDAL.ReadFromFile(Elements.Customer).Cast<Customer>().ToList();
            foreach (Customer customer in Allcustomers)
            {
                if (((Customer)c).Equals(customer))
                {
                    throw new DataException("customer is already exists!");
                }
            }
            Allcustomers.Add((Customer)c);
            itsDAL.WriteToFile(Allcustomers.Cast<object>().ToList(), (Customer)c);
        }

        public void Remove(Object c)
        {
            List<Customer> Allcustomers = itsDAL.ReadFromFile(Elements.Customer).Cast<Customer>().ToList();
            List<User> Allusers = itsDAL.ReadFromFile(Elements.User).Cast<User>().ToList();
            if (!Allcustomers.Any())
                throw new NullReferenceException("No customers to remove!");
            foreach (Customer customer in Allcustomers)
            {
                if (((Customer)c).Equals(customer))
                {
                    Allcustomers.Remove(customer);
                    foreach (User user in Allusers)
                    {
                        if (user.Person.Equals(c))
                            Allusers.Remove(user);
                        break;
                    }
                    break;
                }
            }
            itsDAL.WriteToFile(Allcustomers.Cast<object>().ToList(), (Customer)c);
            itsDAL.WriteToFile(Allusers.Cast<object>().ToList(), new User());
        }

        public void Edit(object oldC, object newC)
        {
            this.Remove(oldC);
            this.Add(newC);
        }

        public List<object> FindByName(string name, StringFields field)
        {
            if (name == null)
                throw new DataException("Bad Input!");
            List<object> result = itsDAL.CustomerNameQuery(name, field).Cast<object>().ToList();
            return result;
        }

        public List<object> FindByNumber(IntFields field, int minNumber, int maxNumber)
        {
            //search method by number
            return itsDAL.CustomerNumberQuery(minNumber, maxNumber, field).Cast<object>().ToList();
        }

        public List<object> FindByType(ValueType type)
        {
            throw new DataException("customers doesn't have types!");
        }

        public List<object> GetAll()
        {
            //return all Customers
            return itsDAL.ReadFromFile(Elements.Customer);
        }

        public Type GetEntityType()
        {
            //return the Customer type
            return typeof(Customer);
        }
    }
}
