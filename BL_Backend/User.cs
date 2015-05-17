using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    [Serializable()]
    public class User
    {
        //Fields:
        private string username;
        private string password;
        private Object person;

        //Constructors:
        public User(string _username, string _password, Object _person=null)
        {
            username = _username;
            password = _password;
                person = _person;
        }
        //For Deep Copy
        public User(User other)
        {
            username = other.username;
            password = other.password;
            person = other.person;
        }
        public override bool Equals(object _other)
        {
            if (!(_other is User)) return false;
            User other = (User)_other;
            return (username.Equals(other.username) && password.Equals(other.password) && person.Equals(other.person));
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ username.GetHashCode();
        }
        public override string ToString()
        {
            return username;
        }

        //getters and setters:
        public string UserName
        {
            get { return username; }
            set { username = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public Object Person
        {
            get { return person; }
            set { person = value; }
        }
    }
}
