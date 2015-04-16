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
    }
}
