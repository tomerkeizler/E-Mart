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
        List<Product> ProductNumberQuery(int minNumber, int maxNumber, IntFields field);
        List<Product> ProductTypeQuery(ValueType type);


        //Employees:
        List<Employee> EmployeeNameQuery(string name, StringFields field);
        List<Employee> EmployeeNumberQuery(int minNumber, int maxNumber, IntFields field);
        List<Employee> EmployeeTypeQuery(ValueType type);

        //ClubMember:
        List<ClubMember> ClubMemberNameQuery(string name, StringFields field);
        List<ClubMember> ClubMemberNumberQuery(int minNumber, int maxNumber, IntFields field);
        List<ClubMember> ClubMemberTypeQuery(ValueType type);

        //Department:
        List<Department> DepartmentNameQuery(string name, StringFields field);
        List<Department> DepartmentNumberQuery(int minNumber, int maxNumber, IntFields field);

        //Transaction:
        List<Transaction> TransactionNumberQuery(int minNumber, int maxNumber, IntFields field);
        List<Transaction> TransactionTypeQuery(ValueType type);

        //User:
        List<User> UserNameQuery(string name, StringFields field);
    }
}
