using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public enum Elements { Product, Employee, Department, ClubMember, Transaction, User };
    public enum StringFields { name, firstName, lastName, dateOfBirth, username };
    public enum IntFields { price, stockCount, location, productID, id, depID, salary, supervisiorID, memberID, transactionID, tranHistory, receipt};
    public enum Gender { Male, Female };
    class Fields
    {
    }
}