using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend;
using DAL;

namespace BL
{
    class User_BL : IBL
    {
         //Fields:
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
            Allusers.Add((User)u);
            itsDAL.WriteToFile(Allusers.Cast<object>().ToList());
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
                itsDAL.WriteToFile(Allusers.Cast<object>().ToList());
            }
        }

        public void Edit(object oldU, object newU)
        {
            List<User> Allusers = itsDAL.ReadFromFile(Elements.User).Cast<User>().ToList();
            Allusers.Remove((User)oldU);
            Allusers.Add((User)newU);
            itsDAL.WriteToFile(Allusers.Cast<object>().ToList());
        }

        public List<object> FindByName(string name, Backend.StringFields field)
        {
            if (name == null)
                throw new System.Data.DataException("Bad Input!");
            List<object> result = itsDAL.UserNameQuery(name, field).Cast<object>().ToList();
            return result;
        }

        public List<object> FindByNumber(int number, Backend.IntFields field)
        {
            throw new System.Data.DataException("users doesn't have numbers!");
        }

        public List<object> FindByType(ValueType type)
        {
            throw new System.Data.DataException("users doesn't have types!");
        }

        public List<object> GetAll(Backend.Elements element)
        {
            return itsDAL.ReadFromFile(element);
        }
        //Method for User Only
        public bool isItValidUser(User user)
        {
            List<User> Allusers = itsDAL.ReadFromFile(Elements.User).Cast<User>().ToList();
            if (!Allusers.Any())
                throw new NullReferenceException("There is no users at all!");
            if (user.UserName == null || user.Password == null)
            {
                throw new System.Data.DataException("Bad Input!");
            }
            foreach (User _user in Allusers)
            {
                if (_user.UserName.Equals(user.UserName) && _user.Password.Equals(user.Password))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
