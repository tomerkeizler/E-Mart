using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Backend;

/*
 * This class is for Nunit Testing Only for Backend Project!
 */
namespace Backend
{
    [TestFixture]
    class Backend_Test
    {
        [Test]
        public void isCreatedPstatus()
        {
            Product prod = new Product("Banana", PType.a, 1, 21, 2);
            Assert.AreEqual(PStatus.InStock, prod.InStock);
        }
        [Test]
        public void isChangedPstatus()
        {
            Product prod = new Product("Banana", PType.a, 1, 21, 2);
            prod.Buy();
            Assert.AreEqual(PStatus.LowQuantity, prod.InStock);
        }
        [Test]
        public void isAddedSellCount()
        {
            Product prod = new Product("Banana", PType.a, 1, 21, 2);
            prod.Buy();
            Assert.AreEqual(1, prod.SellCounter);
        }
        [Test]
        public void isResetTopSeller()
        {
            Product prod = new Product("Banana", PType.a, 1, 21, 2);
            prod.Buy();
            prod.ResetSells();
            Assert.AreEqual(0, prod.SellCounter);
        }
    }
}
