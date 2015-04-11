using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend;
using DAL;

namespace BL
{
    public interface IBL
    {
        //For Product Entities:
        void AddProduct(Product p);
        void RemoveProduct(Product p);
        void EditProduct(Product p);
        List<Product> FindProductByName(string name);
        List<Product> FindProductByPrice(int price);
        List<Product> FindProductByID(int productID);
        List<Product> FindProductByLocation(int departID);
        List<Product> FindProductByType(PType type);

        //For Employees Entities:
        void AddEmployee(Employee e);
        void RemoveEmployee(Employee e);
        void EditEmployee(Employee e);
        List<Employee> FindEmployeeByFirstName(string firstName);
        List<Employee> FindEmployeeByLastName(string lastName);
        List<Employee> FindEmployeeByID(int id);
        List<Employee> FindEmployeeByDepartmentID(int depID);
        List<Employee> FindEmployeeBySalary(int Salary);
        List<Employee> FindEmployeeByGender(Gender gender);
    }
}
