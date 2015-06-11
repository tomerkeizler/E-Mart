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
        List<Backend.Employee> EmployeeNameQuery(string name, StringFields field);
        List<Backend.Employee> EmployeeNumberQuery(int minNumber, int maxNumber, IntFields field);
        List<Backend.Employee> EmployeeTypeQuery(ValueType type);

        //ClubMember:
        List<Backend.ClubMember> ClubMemberNameQuery(string name, StringFields field);
        List<Backend.ClubMember> ClubMemberNumberQuery(int minNumber, int maxNumber, IntFields field);
        List<Backend.ClubMember> ClubMemberTypeQuery(ValueType type);

        //Customer:
        List<Backend.Customer> CustomerNameQuery(string name, StringFields field);
        List<Backend.Customer> CustomerNumberQuery(int minNumber, int maxNumber, IntFields field);

        //Department:
        List<Backend.Department> DepartmentNameQuery(string name, StringFields field);
        List<Backend.Department> DepartmentNumberQuery(int minNumber, int maxNumber, IntFields field);

        //Transaction:
        List<Backend.Transaction> TransactionNumberQuery(int minNumber, int maxNumber, IntFields field);
        List<Backend.Transaction> TransactionTypeQuery(ValueType type);

        //User:
        List<Backend.User> UserNameQuery(string name, StringFields field);
        List<Backend.User> UserTypeQuery(ValueType type);
        List<Backend.User> UserPersonQuery(object person);
    }
}
