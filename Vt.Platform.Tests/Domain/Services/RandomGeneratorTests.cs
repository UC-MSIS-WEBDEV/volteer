using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vt.Platform.Domain.Services;

namespace Vt.Platform.Tests.Domain.Services
{
    [TestClass]
    public class RandomGeneratorTests
    {
        [TestMethod]
        public async Task GetEventCode()
        {
            // ARRANGE
            var rg = new RandomGenerator();

            // ACT
            var code = await rg.GetEventCode();

            // ASSERT
            Assert.AreEqual(6, code.Length);
        }
    }
}
