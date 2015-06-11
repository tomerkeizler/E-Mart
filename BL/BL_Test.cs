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
        public static void TopSeller(List<Backend.Product> Allprods, int currMonth)
        {
            int currMax = 1;
            foreach (Backend.Product prod in Allprods)
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
            foreach (Backend.Product prod in Allprods)
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
            Backend.Product prod1 = new Backend.Product("Banana", PType.Electronics, 1, 21, 2);
            Backend.Product prod2 = new Backend.Product("avocado", PType.Food, 1, 21, 5);
            prod1.Buy();
            prod1.Buy();
            prod2.Buy();

            int currMonth = DateTime.Today.Month;
            List<Backend.Product> Allprods = new List<Backend.Product>();
            Allprods.Add(prod1);
            Allprods.Add(prod2);
            TopSeller(Allprods,DateTime.Today.Month);

            Assert.IsTrue(prod1.IsTopSeller);
            Assert.IsFalse(prod2.IsTopSeller);

        }
        [Test]
        public void isContain2TopSeller()
        {
            Backend.Product prod1 = new Backend.Product("Banana", PType.Electronics, 1, 21, 2);
            Backend.Product prod2 = new Backend.Product("avocado", PType.Food, 1, 21, 5);
            prod1.Buy();
            prod1.Buy();
            prod2.Buy();
            prod2.Buy();

            int currMonth = DateTime.Today.Month;
            List<Backend.Product> Allprods = new List<Backend.Product>();
            Allprods.Add(prod1);
            Allprods.Add(prod2);
            TopSeller(Allprods,DateTime.Today.Month);

            Assert.IsTrue(prod1.IsTopSeller);
            Assert.IsTrue(prod2.IsTopSeller);
        }
        [Test]
        public void isTopSellerRestarted()
        {
            Backend.Product prod1 = new Backend.Product("Banana", PType.Electronics, 1, 21, 2);
            Backend.Product prod2 = new Backend.Product("avocado", PType.Food, 1, 21, 5);
            prod1.Buy();
            prod1.Buy();
            prod2.Buy();
            List<Backend.Product> Allprods = new List<Backend.Product>();
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
