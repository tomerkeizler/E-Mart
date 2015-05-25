using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Backend;
using DAL;
using BL;

/*
 * This class is for Nunit Testing Only for BL Project!
 */
namespace BL
{
    [TestFixture]
    class BL_Test
    {
        public static void TopSeller(List<Product> Allprods, int currMonth)
        {
            int currMax = 1;
            foreach (Product prod in Allprods)
            {
                if (prod.TopSellerMonth != currMonth)
                {
                    prod.ResetSells();
                }
                if (prod.SellCounter >= currMax)
                {
                    currMax = prod.SellCounter;
                }
            }
            foreach (Product prod in Allprods)
            {
                if (prod.SellCounter == currMax)
                {
                    prod.IsTopSeller = true;
                }
            }
        }
        [Test]
        public void isContainTopSeller()
        {
            Product prod1 = new Product("Banana", PType.Electronics, 1, 21, 2);
            Product prod2 = new Product("avocado", PType.Food, 1, 21, 5);
            prod1.Buy();
            prod1.Buy();
            prod2.Buy();

            int currMonth = DateTime.Today.Month;
            List<Product> Allprods = new List<Product>();
            Allprods.Add(prod1);
            Allprods.Add(prod2);
            TopSeller(Allprods,DateTime.Today.Month);

            Assert.IsTrue(prod1.IsTopSeller);
            Assert.IsFalse(prod2.IsTopSeller);

        }
        [Test]
        public void isContain2TopSeller()
        {
            Product prod1 = new Product("Banana", PType.Electronics, 1, 21, 2);
            Product prod2 = new Product("avocado", PType.Food, 1, 21, 5);
            prod1.Buy();
            prod1.Buy();
            prod2.Buy();
            prod2.Buy();

            int currMonth = DateTime.Today.Month;
            List<Product> Allprods = new List<Product>();
            Allprods.Add(prod1);
            Allprods.Add(prod2);
            TopSeller(Allprods,DateTime.Today.Month);

            Assert.IsTrue(prod1.IsTopSeller);
            Assert.IsTrue(prod2.IsTopSeller);
        }
        [Test]
        public void isTopSellerRestarted()
        {
            Product prod1 = new Product("Banana", PType.Electronics, 1, 21, 2);
            Product prod2 = new Product("avocado", PType.Food, 1, 21, 5);
            prod1.Buy();
            prod1.Buy();
            prod2.Buy();
            List<Product> Allprods = new List<Product>();
            Allprods.Add(prod1);
            Allprods.Add(prod2);
            TopSeller(Allprods,DateTime.Today.Month);
            Assert.IsTrue(prod1.IsTopSeller);
            Assert.IsFalse(prod2.IsTopSeller);
            TopSeller(Allprods,1);
            Assert.IsFalse(prod1.IsTopSeller);
            Assert.IsFalse(prod2.IsTopSeller);
        }
    }
}
