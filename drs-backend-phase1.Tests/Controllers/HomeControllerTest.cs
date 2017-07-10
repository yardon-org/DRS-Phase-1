using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using drs_backend_phase1;
using drs_backend_phase1.Controllers;

namespace drs_backend_phase1.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            EventLogController controller = new EventLogController();

            // Act
            //ViewResult result = controller.Index() as ViewResult;

            // Assert
            //Assert.IsNotNull(result);
            //Assert.AreEqual("Home Page", result.ViewBag.Title);
        }
    }
}
