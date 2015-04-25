using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//contains classes fields names for queries in PL
namespace Backend
{
    public enum IntFields { price, stockCount, location, productID, id, depID, salary, departmentID, supervisiorID, memberID, transactionID, tranHistory, receipt };
    public enum Elements { Product, Employee, Department, ClubMember, Transaction, User };
    public enum StringFields { name, firstName, lastName, dateOfBirth, username };
    public enum TypeFields { gender, inStock, type, is_a_return  };
    public enum Gender { Male, Female };
    class Fields
    {
    }
}