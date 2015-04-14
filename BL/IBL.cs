using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend;
using DAL;

namespace BL
{
    public enum StringFields { name, firstName, lastName};
    public enum IntFields { price, stockCount, location, productID };
    public interface IBL
    {
        void Add(object obj);
        void Remove(object obj);
        void Edit(object oldObj, object newObj);
        List<object> FindByName(string name, StringFields field);
        List<object> FindByNumber(int number, IntFields field);
        List<object> FindByType(object type);
    }
}
