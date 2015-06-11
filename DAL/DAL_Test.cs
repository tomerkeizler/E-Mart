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
        Backend.Product a = new Backend.Product("first", PType.Clothes, 2, 4, 123, 4);
        Backend.Product b = new Backend.Product("second", PType.Food, 3, 5, 1213, 5);
        Backend.Product c = new Backend.Product("Third", PType.Electronics, 3, 5, 1213, 5);
        Backend.Product d = new Backend.Product("Forth", PType.Electronics, 3, 5, 1213, 5);
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
            list.Add(a);
            list.Add(b);
            sql.WriteToFile(list, b);
            Assert.Contains(b, sql.ReadFromFile(Elements.Product));
            sql.WriteToFile(new List<object>(), b);
        }
        [Test]
        public void SQLClear()
        {
            sql.WriteToFile(list, b);
            Assert.IsEmpty(sql.ReadFromFile(Elements.Product));
            sql.WriteToFile(new List<object>(), b);

        }
        [Test]
        public void SQLProductQueryByNum()
        {
            list.Add(a);
            list.Add(b);
            sql.WriteToFile(list, b);
            List<Backend.Product> testlist = sql.ProductNumberQuery(5, 5, IntFields.productID);
            Assert.Contains(b, testlist);
            sql.WriteToFile(new List<object>(), b);
        }
        [Test]
        public void ProductNumberQuery()
        {
            list.Add(a);
            list.Add(b);
            List<Backend.Product> testlist = linq.ProductNumberQuery(5, 5, IntFields.productID);
            Assert.Contains(b, testlist);
        }
        [Test]
        public void ClubmemberTest()
        {
            ClubMember member1 = new ClubMember(123123, "asaf", "asafaa", new DateTime(2014, 09, 10), Gender.Male);
            List<object> list = new List<object>();
            list.Add(member1);
            linq.WriteToFile(list, member1);
            List<object> readlist = linq.ReadFromFile(Elements.ClubMember);
            Assert.Contains(member1, readlist);
            
        }
    }
}
