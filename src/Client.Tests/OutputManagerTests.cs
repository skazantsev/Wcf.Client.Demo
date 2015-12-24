using System;
using System.Threading.Tasks;
using Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Client.Tests
{
    /// <summary>
    /// Example of testing IServiceInvoker.
    /// </summary>
    [TestClass]
    public class OutputManagerTests
    {
        [TestMethod]
        public void Test_GettingOutput()
        {
            var invokerMock = GetMockedInvoker();
            var sut = new OutputManager(invokerMock.Object);

            var result = sut.GetOutput().Result;

            invokerMock.Verify();
            Assert.AreEqual("Result is: 5", result);
        }

        private static Mock<IServiceInvoker> GetMockedInvoker()
        {
            var serviceMock = new Mock<ICalculatorService>();
            serviceMock.Setup(x => x.Multiply(It.IsAny<double>(), It.IsAny<double>()))
                .ReturnsAsync(5);

            var invokerMock = new Mock<IServiceInvoker>();
            invokerMock.Setup(x => x.InvokeService(It.IsAny<Func<ICalculatorService, Task<double>>>()))
                .Returns((Func<ICalculatorService, Task<double>> x) => x(serviceMock.Object))
                .Verifiable();

            return invokerMock;
        }
    }
}
