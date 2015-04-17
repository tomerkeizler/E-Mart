using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend;

namespace DAL
{
    public interface IDAL
    {
        //For all Types:
        void WriteToFile(List<object> list);
        List<object> ReadFromFile(Elements element);

        //Products:
        List<Product> ProductNameQuery(string name, StringFields field);
        List<Product> ProductNumberQuery(int number, IntFields field);
        List<Product> ProductTypeQuery(ValueType type);


        //Employees:
        List<Employee> EmployeeNameQuery(string name, StringFields field);
        List<Employee> EmployeeNumberQuery(int number, IntFields field);
        List<Employee> EmployeeTypeQuery(ValueType type);

        //ClubMember:
        List<ClubMember> ClubMemberNameQuery(string name, StringFields field);
        List<ClubMember> ClubMemberNumberQuery(int number, IntFields field);
        List<ClubMember> ClubMemberTypeQuery(ValueType type);
        List<Transaction> ClubMemberTransactionQuery(int name, ClubMember clubmember);

        //Department:
        List<Department> DepartmentNameQuery(string name, StringFields field);
        List<Department> DepartmentNumberQuery(int number, IntFields field);

        //Transaction:
        List<Transaction> TransactionNameQuery(string name, StringFields field);
        List<Transaction> TransactionTypeQuery(ValueType type);

        //User:
        List<User> UserNameQuery(string name, StringFields field);
    }
}
