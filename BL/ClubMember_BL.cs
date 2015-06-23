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
            List<Backend.ClubMember> Allclubmems = itsDAL.ReadFromFile(Elements.ClubMember).Cast<Backend.ClubMember>().ToList();
            //Generate the new clubmember ID              
            int maxID = 0;
            foreach (Backend.ClubMember clubmem in Allclubmems)
            {
                if (clubmem.MemberID > maxID)
                    maxID = clubmem.MemberID;
                if (((Backend.ClubMember)cm).Id!=0 && ((Backend.ClubMember)cm).Id == clubmem.Id)
                {
                    throw new System.Data.DataException("The ID allready exist in the system");
                }
            }
            if (((Backend.ClubMember)cm).MemberID == 0)
            {
                //set the new ID
                ((Backend.ClubMember)cm).MemberID = maxID + 1;
            }
            //Add the new clubmember to the system.
            Allclubmems.Add((Backend.ClubMember)cm);
            itsDAL.WriteToFile(Allclubmems.Cast<object>().ToList(), (Backend.ClubMember)cm);
            return cm;
        }

        public void Remove(object cm, Boolean isEdit = false)
        {
            List<Backend.ClubMember> Allclubmems = itsDAL.ReadFromFile(Elements.ClubMember).Cast<Backend.ClubMember>().ToList();
            List<Backend.User> Allusers = itsDAL.ReadFromFile(Elements.User).Cast<Backend.User>().ToList();
            //check if there are any clubmembers to remove
            if (!Allclubmems.Any())
                throw new NullReferenceException("No ClubMembers to remove!");
            //find and remove clubmember
            foreach (Backend.ClubMember clubmem in Allclubmems)
            {
                if (clubmem.Equals(cm))
                {
                    Allclubmems.Remove(clubmem);
                    foreach (Backend.User user in Allusers)
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
            itsDAL.WriteToFile(Allusers.Cast<object>().ToList(), new Backend.User());
            itsDAL.WriteToFile(Allclubmems.Cast<object>().ToList(), cm);
        }

        public void Edit(object oldCM, object newCM)
        {
            List<Backend.ClubMember> Allclubmems = itsDAL.ReadFromFile(Elements.ClubMember).Cast<Backend.ClubMember>().ToList();
            //Check for credit card conflict
            if (((Backend.ClubMember)newCM).CreditCard != null)
            {
                foreach (Backend.ClubMember clubmem in Allclubmems)
                {
                    if (clubmem.CreditCard != null && clubmem.CreditCard.CreditNumber == ((Backend.ClubMember)newCM).CreditCard.CreditNumber)
                    {
                        throw new System.Data.DataException("The Credit Card ID allready exist in the system");
                    }
                }
            }
            //preserve the id for the edited clubmember
            ((Backend.ClubMember)newCM).MemberID = ((Backend.ClubMember)oldCM).MemberID;
            List<Backend.User> oldUserList = itsDAL.UserPersonQuery(oldCM);
            Backend.User oldUser = oldUserList.ElementAtOrDefault(0);
            if (oldUser == null)
            {
                throw new NullReferenceException("The clubmember does not exist!");
            }
            User_BL itsUserBL = new User_BL(itsDAL);
            Backend.User newUser = new Backend.User(oldUser);
            newUser.Person = newCM;
            itsUserBL.Remove(oldUser, true);
            this.Remove(oldCM);
            this.Add(newCM);
            itsUserBL.Add(newUser);
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
            return typeof(Backend.ClubMember);
        }
        public string GetEntityName()
        {
            //return the clubmember type as a string
            return "Club Member";
        }
    }
}
