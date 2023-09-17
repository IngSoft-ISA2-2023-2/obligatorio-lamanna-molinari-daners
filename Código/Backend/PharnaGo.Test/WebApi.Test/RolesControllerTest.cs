
using ExportationModel.ExportDomain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PharmaGo.DataAccess.Repositories;
using PharmaGo.Domain.Entities;
using PharmaGo.IBusinessLogic;
using PharmaGo.WebApi.Controllers;
using PharmaGo.WebApi.Models.Out;

namespace PharmaGo.Test.WebApi.Test
{
    [TestClass]
    public class RolesControllerTest
    {

        private RolesController _rolesController;
        private Mock<IRoleManager> _rolesManagerMock;

        [TestInitialize]
        public void SetUp()
        {
            _rolesManagerMock = new Mock<IRoleManager>(MockBehavior.Strict);
            _rolesController = new RolesController(_rolesManagerMock.Object);

        }

        [TestCleanup]
        public void Cleanup()
        {
            _rolesManagerMock.VerifyAll();
        }

        [TestMethod]
        public void GetAll_Ok()
        {

            IEnumerable<Role> list = new List<Role> { 
                new Role { Id = 1, Name = "Administrator" }, 
                new Role { Id = 2, Name = "Employee" }, 
                new Role { Id = 3, Name = "Owner" } };
            _rolesManagerMock.Setup(u => u.GetAll()).Returns(list);

            var result = _rolesController.GetAll();

            // Assert
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            var value = objectResult.Value as IEnumerable<RoleModelResponse>;

            Assert.AreEqual(200, statusCode);
            Assert.AreEqual(3, value.ElementAt(2).Id);
            Assert.AreEqual("Owner", value.ElementAt(2).Name);
            Assert.AreEqual(3, value.Count());
        }
    }

}
