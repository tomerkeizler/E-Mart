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
            //Add the new ClubMember to the system
            List<ClubMember> Allclubmems = itsDAL.ReadFromFile(Elements.ClubMember).Cast<ClubMember>().ToList();
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
            List<ClubMember> Allclubmems = itsDAL.ReadFromFile(Elements.ClubMember).Cast<ClubMember>().ToList();
            ((ClubMember)newCM).Id = ((ClubMember)oldCM).Id;
            Allclubmems.Remove((ClubMember)oldCM);
            Allclubmems.Add((ClubMember)newCM);
            itsDAL.WriteToFile(Allclubmems.Cast<object>().ToList());
        }

        public List<object> FindByName(string name, StringFields field)
        {
            if (name == null)
                throw new System.Data.DataException("Bad Input!");
            List<object> result = itsDAL.EmployeeNameQuery(name, field).Cast<object>().ToList();
            return result;
        }

        public List<object> FindByNumber(int number, IntFields field)
        {
            throw new NotImplementedException();
        }

        public List<object> FindByType(ValueType type)
        {
            throw new NotImplementedException();
        }

        public List<object> GetAll(Elements element)
        {
            throw new NotImplementedException();
        }
    }
}
