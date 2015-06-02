using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend
{
    [Serializable()]
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
        private Rank rank;

        //Constructors:
        public Employee()
        {
            firstName = "";
            lastName = "";
            id = -1;
            gender = Gender.Male;
            depID = -1;
            salary = -1;
            supervisiorID = -1;
            rank = Rank.Worker;
        }
        public Employee(string _firstName, string _lastName, int _id, Gender _gender, int _depID, int _salary, int _supervisiorID)
        {
            firstName = _firstName;
            lastName = _lastName;
            id = _id;
            gender = _gender;
            depID = _depID;
            salary = _salary;
            supervisiorID = _supervisiorID;
            if (SupervisiorID == 0)
            {
                rank = Rank.Administrator;
            }
            else
            {
                rank = Rank.Worker;
            }
        }
        //For Deep Copy
        public Employee(Employee other)
        {
            firstName = other.FirstName;
            lastName = other.LastName;
            id = other.Id;
            gender = other.Gender;
            depID = other.DepID;
            salary = other.Salary;
            supervisiorID = other.SupervisiorID;
            rank = other.Rank;
        }

        public override string ToString()
        {
            return this.rank.ToString() + " -" + firstName + " " + lastName;
        }
        public override bool Equals(object _other)
        {
            if (!(_other is Employee)) return false;
            Employee other = (Employee)_other;
            return (firstName.Equals(other.firstName) && lastName.Equals(other.lastName) && id == other.id && gender.Equals(other.gender)
                    && depID == other.depID && salary == other.salary && supervisiorID == other.supervisiorID && rank == other.Rank);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ firstName.GetHashCode();
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
        public Rank Rank
        {
            get { return rank; }
            set { rank = value; }
        }
    }
}
