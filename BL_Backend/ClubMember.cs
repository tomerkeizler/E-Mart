using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    [Serializable()]
    public class ClubMember
    {
        //Fields:
        private int memberID;
        private int id;
        private string firstName;
        private string lastName;
        private List<Transaction> tranHistory;
        private DateTime dateOfBirth;
        private Gender gender;

        //Constructors:
        public ClubMember(int _id, string _firstName, string _lastName, List<Transaction> _tranHistory, DateTime dob, Gender _gender, int _memID=0)
        {
            memberID = _memID;
            id = _id;
            firstName = _firstName;
            lastName = _lastName;
            tranHistory = _tranHistory;
            dateOfBirth = dob;
            gender = _gender;
        }
        public ClubMember(ClubMember other)
        {
            memberID = other.MemberID;
            id = other.Id;
            firstName = other.firstName;
            lastName = other.lastName;
            tranHistory = other.tranHistory;
            dateOfBirth = other.dateOfBirth;
            gender = other.gender;
        }


        public override string ToString()
        {
            return memberID+"";
        }
        public override bool Equals(object _other)
        {
            if (!(_other is ClubMember)) return false;
            ClubMember other = (ClubMember)_other;
            return (memberID == other.MemberID && id == other.Id && firstName.Equals(other.firstName) && lastName.Equals(other.lastName)
                    && dateOfBirth.Equals(other.dateOfBirth) && gender.Equals(other.gender) && tranHistory.SequenceEqual(other.tranHistory));
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ firstName.GetHashCode();
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
        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; }
        }
        public Gender Gender
        {
            get { return gender; }
            set { gender = value; }
        }
        public List<Transaction> TransactionHistory
        {
            get { return tranHistory; }
            set { tranHistory = value; }
        }

    }
}
