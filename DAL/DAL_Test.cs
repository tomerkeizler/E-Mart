﻿using System;
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
        Product c = new Product("Third", PType.c, 3, 5, 1213, 5);
        Product d = new Product("Forth", PType.c, 3, 5, 1213, 5);
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
        public void mulquery()
        {
            List<Product> listp = new List<Product>();
            listp.Add(a);
            listp.Add(b);
            listp.Add(c);
            listp.Add(d);
            List<PType> filter =  new List<PType>();
            filter.Add(PType.a);
            filter.Add(PType.c);
            Assert.AreEqual(3, ProductMulTypeQuery(listp, filter).Count);
        }
    }
}
