using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    [Serializable()]
    public class ClubMember : Customer
    {
        //Fields:
        private int memberID;
        private DateTime dateOfBirth;
        private Gender gender;

        //Constructors:
        public ClubMember(int _id, string _firstName, string _lastName, List<Transaction> _tranHistory, DateTime dob, Gender _gender, int _memID=0) : base(_id,_firstName,_lastName,_tranHistory)
        {
            memberID = _memID;
            dateOfBirth = dob;
            gender = _gender;
        }
        //For Deep Copy
        public ClubMember(ClubMember other) : base(other.Id,other.FirstName,other.LastName,other.TranHistory)
        {
            memberID = other.MemberID;
            dateOfBirth = other.DateOfBirth;
            gender = other.Gender;
        }


        public override string ToString()
        {
            return memberID+"";
        }
        public override bool Equals(object _other)
        {
            if (!(_other is ClubMember)) return false;
            ClubMember other = (ClubMember)_other;
            return (base.Equals((Customer)_other) && memberID == other.MemberID && dateOfBirth.Equals(other.dateOfBirth) && gender.Equals(other.gender));
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() ^ memberID.GetHashCode();
        }
        //getters and setters:
        public int MemberID
        {
            get { return memberID; }
            set { memberID = value; }
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
    }
}
