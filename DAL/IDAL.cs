using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Backend;

namespace DAL
{
    public interface IDAL
    {
        //For all Types:
        void WriteToFile(List<object> list, object obj);
        List<object> ReadFromFile(Elements element);

        //Products:
        List<Backend.Product> ProductNameQuery(string name, StringFields field);
        List<Backend.Product> ProductNumberQuery(int minNumber, int maxNumber, IntFields field);
        List<Backend.Product> ProductTypeQuery(ValueType type);
        void FilterProducts(ObservableCollection<Buyable> currentList, PType type, bool isAdd);


        //Employees:
        List<Employee> EmployeeNameQuery(string name, StringFields field);
        List<Employee> EmployeeNumberQuery(int minNumber, int maxNumber, IntFields field);
        List<Employee> EmployeeTypeQuery(ValueType type);

        //ClubMember:
        List<ClubMember> ClubMemberNameQuery(string name, StringFields field);
        List<ClubMember> ClubMemberNumberQuery(int minNumber, int maxNumber, IntFields field);
        List<ClubMember> ClubMemberTypeQuery(ValueType type);

        //Customer:
        List<Customer> CustomerNameQuery(string name, StringFields field);
        List<Customer> CustomerNumberQuery(int minNumber, int maxNumber, IntFields field);

        //Department:
        List<Department> DepartmentNameQuery(string name, StringFields field);
        List<Department> DepartmentNumberQuery(int minNumber, int maxNumber, IntFields field);

        //Transaction:
        List<Transaction> TransactionNumberQuery(int minNumber, int maxNumber, IntFields field);
        List<Transaction> TransactionTypeQuery(ValueType type);

        //User:
        List<User> UserNameQuery(string name, StringFields field);
        List<User> UserTypeQuery(ValueType type);
        List<User> UserPersonQuery(object person);
    }
}
