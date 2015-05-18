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
        public ClubMember(int _id, string _firstName, string _lastName, DateTime _dob, Gender _gender, int _memID = 0, CreditCard _creditCard=null)
            : base(_id, _firstName, _lastName, _creditCard)
        {
            memberID = _memID;
            dateOfBirth = _dob;
            gender = _gender;
        }
        public ClubMember(int _memID, DateTime _dob, Gender _gender, Customer _customer): base(_customer)
        {
            memberID = _memID;
            dateOfBirth = _dob;
            gender = _gender;
        }
        //For Deep Copy
        public ClubMember(ClubMember other)
            : base((Customer)other)
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
