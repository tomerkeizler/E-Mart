using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//contains classes fields names for queries in PL
namespace Backend
{
    public enum Elements { Product, Employee, Department, ClubMember,Customer, Transaction, User };
    public enum IntFields { price, stockCount, location, productID, id, depID, salary, departmentID, supervisiorID, memberID, transactionID, tranHistory, receipt };
    public enum StringFields { name, firstName, lastName, dateOfBirth, currentDate, username };
    public enum TypeFields { gender, inStock, type, is_a_return, payment, rank };
    public enum Gender { Male, Female };
    public enum Rank { Administrator, Manager, Worker, Customer };
    class Fields
    {
    }
}