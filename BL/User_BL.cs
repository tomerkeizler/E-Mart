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
        private Backend.Employee DEFAULT_ADMIN = new Backend.Employee();
        IDAL itsDAL;

        //Constructors:
        public User_BL(IDAL dal)
        {
            itsDAL = dal;
        }

        //Methods:
        public object Add(object u)
        {
            //Add the new user to the system
            List<Backend.User> Allusers = itsDAL.ReadFromFile(Elements.User).Cast<Backend.User>().ToList();
            foreach (Backend.User user in Allusers)
            {
                if (((Backend.User)u).UserName == user.UserName)
                    throw new ArgumentException("the username is already exists!");
            }
            Allusers.Add((Backend.User)u);
            itsDAL.WriteToFile(Allusers.Cast<object>().ToList(), (Backend.User)u);
            return u;
        }

        public void Remove(object u, Boolean isEdit = false)
        {
            List<Backend.User> Allusers = itsDAL.ReadFromFile(Elements.User).Cast<Backend.User>().ToList();
            if (!Allusers.Any())
                throw new NullReferenceException("No Users to remove!");
            if (!isEdit)
            {
                List<object> Allclubmembers = itsDAL.ReadFromFile(Elements.ClubMember).ToList();
                List<object> Allcustomers = itsDAL.ReadFromFile(Elements.Customer).ToList();
                List<object> Allemployees = itsDAL.ReadFromFile(Elements.Employee).ToList();
                if (((Backend.User)u).Person is Backend.ClubMember)
                {
                    foreach (object cm in Allclubmembers)
                        if (cm.Equals(((Backend.User)u).Person))
                        {
                            Allclubmembers.Remove(cm);
                            itsDAL.WriteToFile(Allclubmembers.Cast<object>().ToList(), (Backend.ClubMember)cm);
                            break;
                        }
                }
                else if (((Backend.User)u).Person is Backend.Customer)
                {
                    foreach (object c in Allcustomers)
                        if (c.Equals(((Backend.User)u).Person))
                        {
                            Allcustomers.Remove(c);
                            itsDAL.WriteToFile(Allcustomers.Cast<object>().ToList(), (Backend.Customer)c);
                            break;
                        }
                }
                else if (((Backend.User)u).Person is Backend.Employee)
                {
                    foreach (object e in Allemployees)
                        if (e.Equals(((Backend.User)u).Person))
                        {
                            Allemployees.Remove(e);
                            itsDAL.WriteToFile(Allemployees.Cast<object>().ToList(), (Backend.Employee)e);
                            break;
                        }
                }
            }
            
                foreach (Backend.User user in Allusers)
                {
                    if (user.Equals(u))
                    {
                        Allusers.Remove(user);
                        break;
                    }
                }
            itsDAL.WriteToFile(Allusers.Cast<object>().ToList(), (Backend.User)u);
        }

        public void Edit(object oldU, object newU)
        {
            if (((Backend.User)oldU).Person is Backend.Employee)
            {
                if (((Backend.Employee)((Backend.User)oldU).Person).Id == -1)
                    throw new UnauthorizedAccessException("can't edit default administrator");
            }
            this.Remove(oldU,true);
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
        public Backend.User isItValidUser(Backend.User user)
        {
            Boolean isThereAdmin = false;
            List<Backend.User> Allusers = itsDAL.ReadFromFile(Elements.User).Cast<Backend.User>().ToList();
            foreach (Backend.User _user in Allusers)
            {
                if ((_user.Person is Backend.Employee) && ((Backend.Employee)(_user.Person)).Rank == Rank.Administrator)
                {
                    isThereAdmin = true;
                }
            }
            if (!isThereAdmin)
            {
                DEFAULT_ADMIN.Rank = Rank.Administrator;
                Backend.User admin = new Backend.User(DEFAULT_USER_NAME, DEFAULT_PASSWORD, DEFAULT_ADMIN);
                Allusers.Add(admin);
            }
            if (user.UserName == null || user.Password == null)
            {
                throw new System.Data.DataException("Bad Input!");
            }
            foreach (Backend.User _user in Allusers)
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
            return typeof(Backend.User);
        }
        public string GetEntityName()
        {
            //return the User type as a string
            return "User";
        }
    }
}
