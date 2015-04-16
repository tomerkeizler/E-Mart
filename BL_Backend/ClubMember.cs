using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public enum Gender {Male, Female };
    class ClubMember
    {
        //Fields:
        private int memberID;
        private int id;
        private string firstName;
        private string lastName;
        private List<Transaction> tranHistory;
        private int dateOfBirth;
        private Gender gender;

        //Constructors:
        public ClubMember(int _memID, int _id, string _firstName, string _lastName, List<Transaction> _tranHistory, int dob, Gender _gender)
        {
            memberID = _memID;
            id = _id;
            firstName = _firstName;
            lastName = _lastName;
            tranHistory = _tranHistory;
            dateOfBirth = dob;
            gender = _gender;
        }

        public override string ToString()
        {
            return memberID+"";
        }

        //getters and setters:
        public int MemberID
        {
            get { return memberID; }
            set { memberID = value; }
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
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
        public List<Transaction> TransactionHistory
        {
            get { return tranHistory; }
            set { tranHistory = value; }
        }
        public int DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; }
        }
        public Gender Gender
        {
            get { return gender; }
            set { gender = value; }
        }

    }
}
