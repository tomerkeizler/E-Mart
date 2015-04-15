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
        List<Product> ProductNameQuery(string name);
        List<Product> ProductIDQuery(int id);
        List<Product> ProductTypeQuery(PType type);
        List<Product> ProductLocationQuery(int departID);
        List<Product> ProductPriceQuery(int price);
        List<Product> ProductStockCountQuery(int stockCount);



        //Employees:
        List<Employee> EmployeeFirstNameQuery(string firstName);
        List<Employee> EmployeeLastNameQuery(string lastName);
        List<Employee> EmployeeIDQuery(int id);
        List<Employee> EmployeeSalaryQuery(int salary);
        List<Employee> EmployeesupervisiorIDQuery(int superID);
        List<Employee> EmployeeDepartmentIDQuery(int depID);
        List<Employee> EmployeeGenderQuery(Gender gender);
    }
}
