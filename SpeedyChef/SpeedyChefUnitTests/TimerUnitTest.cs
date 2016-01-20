using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeedyChef;

namespace SpeedyChefUnitTests
{
    [TestClass]
    public class TimerUnitTest
    {
        [TestMethod]
        public void TestCreateTimer()
        {
            RecipeStepTimerHandler handler = new RecipeStepTimerHandler("testTimer", 5);
            Assert.AreEqual("testTimer", handler.GetTimerName());
            Assert.AreEqual(5, handler.GetFullTime());
           
        }
    }
}
