using Lab2;
using Lab2.model;
using Lab2.viewmodel;
using System.Collections.ObjectModel;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void FindExpressionTest1()
        {
            ObservableCollection<int> nums = new ObservableCollection<int>();

            Assert.AreEqual(Model.FindExpression(1, nums), null);
        }

        [TestMethod]
        public void FindExpressionTest2()
        {
            ObservableCollection<int> nums = new ObservableCollection<int>();

            Assert.AreEqual(Model.FindExpression(0, nums), null);
        }

        [TestMethod]
        public void FindExpressionTest3()
        {
            ObservableCollection<int> nums = new ObservableCollection<int> { 1, 2, 3, 4, 5 };

            Assert.AreEqual(Model.FindExpression(15, nums), "3 * 5");
        }
    }
}