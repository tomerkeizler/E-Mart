﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend;
using DAL;

namespace BL
{
    public class Transaction_BL : IBL
    {
        //Fields:
        IDAL itsDAL;

        //Constructors:
        public Transaction_BL(IDAL dal)
        {
            itsDAL = dal;
        }

        public void Add(object t)
        {
            List<Transaction> Alltrans = itsDAL.ReadFromFile(Elements.Transaction).Cast<Transaction>().ToList();
            //Gene      rate the new transaction ID
            int maxID = 0;
            foreach (Transaction tran in Alltrans)
            {
                if (tran.Id > maxID)
                    maxID = tran.Id;
                if (((Transaction)t).Id != 0 && ((Transaction)t).Id == tran.Id)
                 {
                     throw new System.Data.DataException("The ID allready exist in the system");
                 }
            }
            if (((Transaction)t).Id == 0)
            {
                //set the new ID
                ((Transaction)t).Id = maxID + 1;
            }
            //Add the new transaction to the system
            Alltrans.Add((Transaction)t);
            itsDAL.WriteToFile(Alltrans.Cast<object>().ToList(), (Transaction)t);
        }

        public void Remove(object t)
        {
            List<Transaction> Alltrans = itsDAL.ReadFromFile(Elements.Transaction).Cast<Transaction>().ToList();
            if (!Alltrans.Any())
                throw new NullReferenceException("No Transactions to remove!");
            else
            {
                foreach (Transaction tran in Alltrans)
                {
                    if (tran.Equals(t))
                    {
                        Alltrans.Remove(tran);
                        break;
                    }
                }
                itsDAL.WriteToFile(Alltrans.Cast<object>().ToList(), t);
            }
        }

        public void Edit(object oldT, object newT)
        {           
            ((Transaction)newT).Id = ((Transaction)oldT).Id;
            this.Remove(oldT);
            this.Add(newT);            
        }

        public List<object> FindByName(string name, StringFields field)
        {
            throw new System.Data.DataException("transactions doesn't have names!");
        }

        public List<object> FindByNumber(IntFields field, int minNumber, int maxNumber)
        {
            return itsDAL.TransactionNumberQuery(minNumber,maxNumber, field).Cast<object>().ToList();
        }

        public List<object> FindByType(ValueType type)
        {
            return itsDAL.TransactionTypeQuery(type).Cast<object>().ToList();
        }

        public List<object> GetAll()
        {
            return itsDAL.ReadFromFile(Elements.Transaction);
        }
    }
}
