using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend;
using DAL;

namespace BL
{
    public class ClubMember_BL : IBL
    {
         //Fields:
        IDAL itsDAL;

        //Constructors:
        public ClubMember_BL(IDAL dal)
        {
            itsDAL = dal;
        }

        //Methods:
        public object Add(object cm)
        {
            List<ClubMember> Allclubmems = itsDAL.ReadFromFile(Elements.ClubMember).Cast<ClubMember>().ToList();
            //Generate the new clubmember ID              
            int maxID = 0;
            foreach (ClubMember clubmem in Allclubmems)
            {
                if (clubmem.MemberID > maxID)
                    maxID = clubmem.MemberID;
                if (((ClubMember)cm).Id!=0 && ((ClubMember)cm).Id == clubmem.Id)
                {
                    throw new System.Data.DataException("The ID allready exist in the system");
                }
                if (clubmem.Id == ((ClubMember)cm).Id)
                {
                    throw new Exception("This club member have duplicate ID with another club member!");
                }
            }
            if (((ClubMember)cm).MemberID == 0)
            {
                //set the new ID
                ((ClubMember)cm).MemberID = maxID + 1;
            }
            //Add the new clubmember to the system.
            Allclubmems.Add((ClubMember)cm);
            itsDAL.WriteToFile(Allclubmems.Cast<object>().ToList(), (ClubMember)cm);
            return cm;
        }

        public void Remove(object cm, Boolean isEdit = false)
        {
            List<ClubMember> Allclubmems = itsDAL.ReadFromFile(Elements.ClubMember).Cast<ClubMember>().ToList();
            List<User> Allusers = itsDAL.ReadFromFile(Elements.User).Cast<User>().ToList();
            //check if there are any clubmembers to remove
            if (!Allclubmems.Any())
                throw new NullReferenceException("No ClubMembers to remove!");
            //find and remove clubmember
            foreach (ClubMember clubmem in Allclubmems)
            {
                if (clubmem.Equals(cm))
                {
                    Allclubmems.Remove(clubmem);
                    foreach (User user in Allusers)
                    {
                        if (user.Person.Equals(cm))
                        {
                            Allusers.Remove(user);
                            break;
                        }
                    }
                    break;
                }
            }
            itsDAL.WriteToFile(Allclubmems.Cast<object>().ToList(), cm);
            itsDAL.WriteToFile(Allusers.Cast<object>().ToList(), new User());
        }

        public void Edit(object oldCM, object newCM)
        {
            //preserve the id for the edited clubmember
            ((ClubMember)newCM).MemberID = ((ClubMember)oldCM).MemberID;
            List<User> oldUserList = itsDAL.UserPersonQuery(oldCM);
            User oldUser = oldUserList.ElementAtOrDefault(0);
            if (oldUser == null)
            {
                throw new NullReferenceException("The clubmember does not exist!");
            }
            User_BL itsUserBL = new User_BL(itsDAL);
            User newUser = new User(oldUser);
            newUser.Person = newCM;
            itsUserBL.Edit(oldUser, newUser);
            this.Remove(oldCM);
            this.Add(newCM);
        }

        public List<object> FindByName(string name, StringFields field)
        {
            //search method by string
            if (name == null)
                throw new System.Data.DataException("Bad Input!");
            List<object> result = itsDAL.ClubMemberNameQuery(name, field).Cast<object>().ToList();
            return result;
        }

        public List<object> FindByNumber(IntFields field, int minNumber, int maxNumber)
        {
            //search method by number
            return itsDAL.ClubMemberNumberQuery(minNumber,maxNumber, field).Cast<object>().ToList(); 
        }

        public List<object> FindByType(ValueType type)
        {
            //search method by type
            return itsDAL.ClubMemberTypeQuery(type).Cast<object>().ToList();
        }

        public List<object> GetAll()
        {
            //return all clubmembers
            return itsDAL.ReadFromFile(Elements.ClubMember);
        }

        public Type GetEntityType()
        {
            //return the clubmember type
            return typeof(ClubMember);
        }
        public string GetEntityName()
        {
            //return the clubmember type as a string
            return "Club Member";
        }
    }
}
