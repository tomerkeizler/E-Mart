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
            else
            {
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
            throw new System.Data.DataException("users doesn't have types!");
        }

        public List<object> GetAll()
        {
            return itsDAL.ReadFromFile(Elements.User);
        }
        //Method for User Only
        public object isItValidUser(User user)
        {
            List<User> Allusers = itsDAL.ReadFromFile(Elements.User).Cast<User>().ToList();
            if (!Allusers.Any())
            {
                User admin = new User(DEFAULT_USER_NAME, DEFAULT_PASSWORD);
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
                    return _user.Person;
                }
            }
            return null;
        }

        public Type GetEntityType()
        {
            return typeof(User);
        }
    }
}
