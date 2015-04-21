using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend;
using DAL;

namespace BL
{
    class ClubMember_BL : IBL
    {
         //Fields:
        IDAL itsDAL;

        //Constructors:
        public ClubMember_BL(IDAL dal)
        {
            itsDAL = dal;
        }

        //Methods:
        public void Add(object cm)
        {
            List<ClubMember> Allclubmems = itsDAL.ReadFromFile(Elements.ClubMember).Cast<ClubMember>().ToList();
            //Generate the new clubmember ID              
            int maxID = 0;
            foreach (ClubMember clubmem in Allclubmems)
            {
                if (clubmem.Id > maxID)
                    maxID = clubmem.Id;
                if (((ClubMember)cm).Id == clubmem.Id)
                {
                    throw new System.Data.DataException("The ID allready exist in the system");
                }
            }
            if (((ClubMember)cm).MemberID == 0)
            {
                //set the new ID
                ((ClubMember)cm).Id = maxID++;
            }
            //Add the new clubmember to the system.
            Allclubmems.Add((ClubMember)cm);
            itsDAL.WriteToFile(Allclubmems.Cast<object>().ToList());
        }

        public void Remove(object cm)
        {
            List<ClubMember> Allclubmems = itsDAL.ReadFromFile(Elements.ClubMember).Cast<ClubMember>().ToList();
            if (!Allclubmems.Any())
                throw new NullReferenceException("No ClubMembers to remove!");
            else
            {
                foreach (ClubMember clubmem in Allclubmems)
                {
                    if (clubmem.Equals(cm))
                    {
                        Allclubmems.Remove(clubmem);
                        break;
                    }
                }
                itsDAL.WriteToFile(Allclubmems.Cast<object>().ToList());
            }
        }

        public void Edit(object oldCM, object newCM)
        {
            ((ClubMember)newCM).MemberID = ((ClubMember)oldCM).MemberID;
            this.Remove(oldCM);
            this.Add(newCM);
        }

        public List<object> FindByName(string name, StringFields field)
        {
            if (name == null)
                throw new System.Data.DataException("Bad Input!");
            List<object> result = itsDAL.ClubMemberNameQuery(name, field).Cast<object>().ToList();
            return result;
        }

        public List<object> FindByNumber(IntFields field, int minNumber, int maxNumber)
        {
            return itsDAL.ClubMemberNumberQuery(minNumber,maxNumber, field).Cast<object>().ToList(); 
        }

        public List<object> FindByType(ValueType type)
        {
            return itsDAL.ClubMemberTypeQuery(type).Cast<object>().ToList();
        }

        public List<object> GetAll()
        {
            return itsDAL.ReadFromFile(Elements.ClubMember);
        }
    }
}
