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
    public class Customer_BL : IBL
    {
         //Fields:
        IDAL itsDAL;

        //Constructors:
        public Customer_BL(IDAL dal)
        {
            itsDAL = dal;
        }

        //Methods:
        public object Add(object c)
        {
            //Add the new employee to the system
            List<Backend.Customer> Allcustomers = itsDAL.ReadFromFile(Elements.Customer).Cast<Backend.Customer>().ToList();
            foreach (Backend.Customer customer in Allcustomers)
            {
                if (customer.Equals(c))
                {
                    throw new DataException("customer is already exists!");
                }
                if (customer.Id == ((Backend.Customer)c).Id)
                {
                    throw new Exception("This customer have duplicate ID with another customer!");
                }
            }
            Allcustomers.Add((Backend.Customer)c);
            itsDAL.WriteToFile(Allcustomers.Cast<object>().ToList(), (Backend.Customer)c);
            return c;
        }

        public void Remove(Object c, Boolean isEdit = false)
        {
            List<Backend.Customer> Allcustomers = itsDAL.ReadFromFile(Elements.Customer).Cast<Backend.Customer>().ToList();
            List<Backend.User> Allusers = itsDAL.ReadFromFile(Elements.User).Cast<Backend.User>().ToList();
            if (!Allcustomers.Any())
                throw new NullReferenceException("No customers to remove!");
            foreach (Backend.Customer customer in Allcustomers)
            {
                if (customer.Equals(c))
                {
                    Allcustomers.Remove(customer);
                    foreach (Backend.User user in Allusers)
                    {
                        if (user.Person.Equals(c))
                        {
                            Allusers.Remove(user);
                            break;
                        }
                    }
                    break;
                }
            }
            itsDAL.WriteToFile(Allusers.Cast<object>().ToList(), new Backend.User());
            itsDAL.WriteToFile(Allcustomers.Cast<object>().ToList(), (Backend.Customer)c);
        }

        public void Edit(object oldC, object newC)
        {
            List<Backend.Customer> Allclubmems = itsDAL.ReadFromFile(Elements.Customer).Cast<Backend.Customer>().ToList();
            //Check for credit card conflict
            if (((Backend.Customer)newC).CreditCard != null && !((Backend.Customer)newC).CreditCard.Equals(((Backend.Customer)oldC).CreditCard))
            {
                foreach (Backend.Customer customer in Allclubmems)
                {
                    if (customer.CreditCard != null && customer.CreditCard.CreditNumber == ((Backend.Customer)newC).CreditCard.CreditNumber)
                    {
                        throw new System.Data.DataException("The Credit Card ID allready exist in the system");
                    }
                }
            }
            List<Backend.User> oldUserList = itsDAL.UserPersonQuery(oldC);
            Backend.User oldUser = oldUserList.ElementAtOrDefault(0);
            if (oldUser == null)
            {
                throw new NullReferenceException("The customer does not exist!");
            }
            User_BL itsUserBL = new User_BL(itsDAL);
            Backend.User newUser = new Backend.User(oldUser);
            newUser.Person = newC;
            itsUserBL.Remove(oldUser,true);
            this.Remove(oldC);
            this.Add(newC);
            itsUserBL.Add(newUser);
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
            return typeof(Backend.Customer);
        }
        public string GetEntityName()
        {
            //return the Customer type as a string
            return "Customer";
        }
    }
}
