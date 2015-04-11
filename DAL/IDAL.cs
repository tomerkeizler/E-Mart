using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend;

namespace DAL
{
    public interface IDAL
    {
        //Products:
        List<Product> GetAllProducts();
        void AddProduct(Product p);
        void RemoveProduct(Product p);
        void EditProduct(Product p);
        List<Product> ProductNameQuery(string name);
        List<Product> ProductIDQuery(int id);
        List<Product> ProductTypeQuery(PType type);
        List<Product> ProductLocationQuery(int departID);
        List<Product> ProductPriceQuery(int price);



        //Employees:
        List<Employee> GetAllEmployees();
        void AddEmployee(Employee e);
        void RemoveEmployee(Employee e);
        void EditEmployee(Employee e);
        List<Employee> EmployeeFirstNameQuery(string firstName);
        List<Employee> EmployeeLastNameQuery(string lastName);
        List<Employee> EmployeeIDQuery(int id);
        List<Employee> EmployeeSalaryQuery(int salary);
        List<Employee> EmployeeDepartmentIDQuery(int depID);
        List<Employee> EmployeeGenderQuery(Gender gender);
    }
}
