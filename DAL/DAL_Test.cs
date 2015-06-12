using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Backend;
using System.IO;

/*
 * This class is for Nunit Testing Only for DAL Project!
 */
namespace DAL
{
    
    [TestFixture]
    public class DAL_Test
    {
        IDAL linq = new LINQ_DAL();
        IDAL sql = new SQL_DAL();
        Backend.Product a = new Backend.Product("first", PType.Clothes, 1, 4, 123,5);
        Backend.Product b = new Backend.Product("second", PType.Food, 3, 25, 1213, 4);
        Backend.Product c = new Backend.Product("Third", PType.Electronics, 3, 5, 1213, 5);
        Backend.Product d = new Backend.Product("Forth", PType.Electronics, 3, 5, 1213, 5);
        Backend.Employee emp1 = new Backend.Employee("Emp1", "Last1", 123123123, Gender.Male, 1, 112, 0);
        List<object> list = new List<object>();
        public static List<Backend.Product> ProductMulTypeQuery(List<Backend.Product> currentList, List<PType> typelist)
        {
            List<Backend.Product> filteredProducts = new List<Backend.Product>();
            if (currentList.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            foreach (PType type in typelist)
            {
                filteredProducts.AddRange(currentList.Where(n => n.Type.Equals(type)).Cast<Backend.Product>().ToList());
            }
            return filteredProducts;
        }
        public void clear()
        {
            List<object> emptyList = new List<object>();
            sql.WriteToFile(emptyList, new Backend.Product());
            sql.WriteToFile(emptyList, new Backend.Employee());
            sql.WriteToFile(emptyList, new Backend.Department());
        }

        [Test]
        public void WriteToFile()
        {
            list.Add(a);
            list.Add(b);
            File.Delete("Backend.Product.xml");
            linq.WriteToFile(list, a);
            Assert.IsTrue(File.Exists("Backend.Product.xml"));
        }
        [Test]
        public void ReadFromFile()
        {
            this.WriteToFile();
            List<object> testlist = linq.ReadFromFile(Elements.Product);
            Assert.AreEqual(testlist, list);
        }
        [Test]
        public void SQLReadAndWrite()
        {
            clear();
            List<object> depList = new List<object>();
            depList.Add(new Backend.Department("Dep1", 1));
            sql.WriteToFile(depList, new Backend.Department());
            list.Add(a);
            //list.Add(b);
            sql.WriteToFile(list, b);
            Assert.Contains(a, sql.ReadFromFile(Elements.Product));
            clear();
        }
        [Test]
        public void SQLClear()
        {
            clear();
            Assert.IsEmpty(sql.ReadFromFile(Elements.Product));
            clear();

        }
        [Test]
        public void SQLProductQueryByNum()
        {
            clear();
            List<object> depList = new List<object>();
            depList.Add(new Backend.Department("Dep1", 1));
            depList.Add(new Backend.Department("Dep2", 3));
            sql.WriteToFile(depList, new Backend.Department());
            list.Add(a);
            list.Add(b);
            sql.WriteToFile(list, b);
            List<Backend.Product> testlist = sql.ProductNumberQuery(5, 5, IntFields.productID);
            Assert.Contains(a, testlist);
            clear();
        }
        [Test]
        public void SQLProductQueryByType()
        {
            clear();
            List<object> depList = new List<object>();
            depList.Add(new Backend.Department("Dep1", 1));
            depList.Add(new Backend.Department("Dep2", 3));
            sql.WriteToFile(depList, new Backend.Department());
            list.Add(a);
            list.Add(b);
            sql.WriteToFile(list, b);
            List<Backend.Product> testlist = sql.ProductTypeQuery(PStatus.InStock);
            Assert.Contains(b, testlist);
            clear();
        }
        [Test]
        public void SQLEqualProduct()
        {
            clear();
            List<object> depList = new List<object>();
            depList.Add(new Backend.Department("Dep1", 1));
            sql.WriteToFile(depList,new Backend.Department());
            list.Add(a);
            sql.WriteToFile(list, a);
            Backend.Product prod = sql.ReadFromFile(Elements.Product).Cast<Backend.Product>().ToList().ElementAt(0);
            Assert.AreEqual(prod, a);
            clear();
        }
        [Test]
        public void SQLEqualEmployee()
        {
            clear();
            List<object> depList = new List<object>();
            depList.Add(new Backend.Department("Dep1", 1));
            depList.Add(new Backend.Department("Dep2", 3));
            sql.WriteToFile(depList, new Backend.Department());
            list.Add(emp1);
            sql.WriteToFile(list, emp1);
            Backend.Employee emp = sql.ReadFromFile(Elements.Employee).Cast<Backend.Employee>().ToList().ElementAt(0);
            Assert.AreEqual(emp, emp1);
            clear();
        }
            
        [Test]
        public void ProductNumberQuery()
        {

            list.Add(a);
            list.Add(b);
            linq.WriteToFile(list, b);
            List<Backend.Product> testlist = linq.ProductNumberQuery(5, 5, IntFields.productID);
            Assert.Contains(a, testlist);
        }
        [Test]
        public void ClubmemberTest()
        {
            Backend.ClubMember member1 = new Backend.ClubMember(123123, "asaf", "asafaa", new DateTime(2014, 09, 10), Gender.Male);
            List<object> list = new List<object>();
            list.Add(member1);
            linq.WriteToFile(list, member1);
            List<object> readlist = linq.ReadFromFile(Elements.ClubMember);
            Assert.Contains(member1, readlist);
            
        }
    }
}
