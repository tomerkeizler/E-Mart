using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend;
using DAL;

namespace BL
{
    public class User_BL : IBL
    {
        //Fields:
        private const string DEFAULT_USER_NAME = "administrator";
        private const string DEFAULT_PASSWORD = "password";
        private Employee DEFAULT_ADMIN = new Employee();
        IDAL itsDAL;

        //Constructors:
        public User_BL(IDAL dal)
        {
            itsDAL = dal;
        }

        //Methods:
        public void Add(object u)
        {
            //Add the new user to the system
            List<User> Allusers = itsDAL.ReadFromFile(Elements.User).Cast<User>().ToList();
            foreach (User user in Allusers)
            {
                if (((User)u).UserName == user.UserName)
                    throw new ArgumentException("the username is already exists!");
            }
            Allusers.Add((User)u);
            itsDAL.WriteToFile(Allusers.Cast<object>().ToList(), (User)u);
        }

        public void Remove(object u)
        {
            List<User> Allusers = itsDAL.ReadFromFile(Elements.User).Cast<User>().ToList();
            if (!Allusers.Any())
                throw new NullReferenceException("No Users to remove!");

            List<object> Allclubmembers = itsDAL.ReadFromFile(Elements.ClubMember).ToList();
            List<object> Allcustomers = itsDAL.ReadFromFile(Elements.Customer).ToList();
            List<object> Allemployees = itsDAL.ReadFromFile(Elements.Employee).ToList();
            if (((User)u).Person is ClubMember)
            {
                foreach (object cm in Allclubmembers)
                    if (cm.Equals(((User)u).Person))
                    {
                        Allclubmembers.Remove(cm);
                        itsDAL.WriteToFile(Allclubmembers.Cast<object>().ToList(), (ClubMember)cm);
                        break;
                    }
            }
            else if (((User)u).Person is Customer) {
                foreach (object c in Allcustomers)
                    if (c.Equals(((User)u).Person))
                    {
                        Allcustomers.Remove(c);
                        itsDAL.WriteToFile(Allcustomers.Cast<object>().ToList(), (Customer)c);
                        break;
                    }
            }
            else if(((User)u).Person is Employee)
            {
                foreach (object e in Allemployees)
                    if (e.Equals(((User)u).Person))
                    {
                        Allemployees.Remove(e);
                        itsDAL.WriteToFile(Allemployees.Cast<object>().ToList(), (Employee)e);
                        break;
                    }
            }
                foreach (User user in Allusers)
                {
                    if (user.Equals(u))
                    {
                        Allusers.Remove(user);
                        break;
                    }
                }
            itsDAL.WriteToFile(Allusers.Cast<object>().ToList(), (User)u);
        }

        public void Edit(object oldU, object newU)
        {
            this.Remove(oldU);
            this.Add(newU);
        }

        public List<object> FindByName(string name, Backend.StringFields field)
        {
            if (name == null)
                throw new System.Data.DataException("Bad Input!");
            List<object> result = itsDAL.UserNameQuery(name, field).Cast<object>().ToList();
            return result;
        }
        public List<object> FindByNumber(IntFields field, int minNumber, int maxNumber)
        {
            throw new System.Data.DataException("users doesn't have numbers!");
        }
        public List<object> FindByType(ValueType type)
        {
            return itsDAL.UserTypeQuery(type).Cast<object>().ToList();
        }
        public List<object> FindByPerson(object person)
        {
            if (person == null)
                throw new System.Data.DataException("Bad Input!");
            List<object> result = itsDAL.UserPersonQuery(person).Cast<object>().ToList();
            return result;
        }

        public List<object> GetAll()
        {
            return itsDAL.ReadFromFile(Elements.User);
        }
        //Method for User Only
        public User isItValidUser(User user)
        {
            List<User> Allusers = itsDAL.ReadFromFile(Elements.User).Cast<User>().ToList();
            if (!Allusers.Any())
            {
                DEFAULT_ADMIN.Rank = Rank.Administrator;
                User admin = new User(DEFAULT_USER_NAME, DEFAULT_PASSWORD,DEFAULT_ADMIN);
                Allusers.Add(admin);
            }
            if (user.UserName == null || user.Password == null)
            {
                throw new System.Data.DataException("Bad Input!");
            }
            foreach (User _user in Allusers)
            {
                if (_user.UserName.Equals(user.UserName) && _user.Password.Equals(user.Password))
                {
                    return _user;
                }
            }
            throw new System.Data.DataException("The user does not exist in the Database!");
        }

        public Type GetEntityType()
        {
            return typeof(User);
        }
    }
}
