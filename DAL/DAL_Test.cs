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
        Product a = new Product("first", PType.a, 2, 4, 123, 4);
        Product b = new Product("second", PType.b, 3, 5, 1213, 5);
        List<object> list = new List<object>();
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
    }
}
