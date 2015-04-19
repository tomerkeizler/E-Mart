using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class User
    {
        //Fields:
        private string username;
        private string password;

        //Constructors:
        public User(string _username, string _password)
        {
            username = _username;
            password = _password;
        }
        public User(User other)
        {
            username = other.username;
            password = other.password;
        }
        public override bool Equals(object _other)
        {
            if (!(_other is User)) return false;
            User other = (User)_other;
            return (username.Equals(other.username) && password.Equals(other.password));
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
    }
}
