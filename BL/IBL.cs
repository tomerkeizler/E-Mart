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
        void Add(object obj);
        void Remove(object obj);
        void Edit(object oldObj, object newObj);
        List<object> FindByName(string name, StringFields field);
        List<object> FindByNumber(IntFields field, int minNumber, int maxNumber=0);
        List<object> FindByType(ValueType type);
        List<object> GetAll();
    }
}
