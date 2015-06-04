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
        Product a = new Product("first", PType.Clothes, 2, 4, 123, 4);
        Product b = new Product("second", PType.Food, 3, 5, 1213, 5);
        Product c = new Product("Third", PType.Electronics, 3, 5, 1213, 5);
        Product d = new Product("Forth", PType.Electronics, 3, 5, 1213, 5);
        List<object> list = new List<object>();
        public static List<Product> ProductMulTypeQuery(List<Product> currentList, List<PType> typelist)
        {
            List<Product> filteredProducts = new List<Product>();
            if (currentList.ElementAtOrDefault(0) == null)
            {
                throw new InvalidDataException("There is nothing to find from.");
            }
            foreach (PType type in typelist)
            {
                filteredProducts.AddRange(currentList.Where(n => n.Type.Equals(type)).Cast<Product>().ToList());
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
        public void ProductNumberQuery()
        {
            list.Add(a);
            list.Add(b);
            List<Product> testlist = linq.ProductNumberQuery(5, 5, IntFields.productID);
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
