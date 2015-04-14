using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend
{
    public enum Gender {Male, Female };
    public class Employee
    {
        //Fields:
        private int id;
        private string lastName;
        private string firstName;
        private int depID;
        private int salary;
        private int supervisiorID;
        private Gender gender;

        //Constructors:
        public Employee(string _firstName, string _lastName, int _id, Gender _gender, int _depID, int _salary, int _supervisiorID)
        {
            firstName = _firstName;
            lastName = _lastName;
            id = _id;
            gender = _gender;
            depID = _depID;
            salary = _salary;
            supervisiorID = _supervisiorID;
        }

        public int ToString()
        {
            return id;
        }

        //getters and setters:
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public Gender Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public int SupervisiorID
        {
            get { return supervisiorID; }
            set { supervisiorID = value; }
        }

        public int Salary
        {
            get { return salary; }
            set { salary = value; }
        }

        public int DepID
        {
            get { return depID; }
            set { depID = value; }
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

    }
}
