using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_Backend
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
        public string ToString()
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
