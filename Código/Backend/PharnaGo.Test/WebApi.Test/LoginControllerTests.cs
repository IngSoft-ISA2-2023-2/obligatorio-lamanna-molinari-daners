using Microsoft.AspNetCore.Mvc;
using Moq;
using PharmaGo.Domain.Entities;
using PharmaGo.Exceptions;
using PharmaGo.IBusinessLogic;
using PharmaGo.WebApi.Controllers;
using PharmaGo.WebApi.Models.In;
using PharmaGo.WebApi.Models.Out;

namespace PharmaGo.Test.WebApi.Test
{

    [TestClass]
    public class LoginControllerTests
    {
        private LoginController _loginController;
        private Mock<ILoginManager> _loginManagerMock;
        private LoginModelRequest loginModel;
        private static Guid token = new Guid("004fb0a6-847f-4194-9115-74f06cdac35d");
        private Authorization authorization;

        [TestInitialize]
        public void SetUp()
        {
            _loginManagerMock = new Mock<ILoginManager>(MockBehavior.Strict);
            _loginController = new LoginController(_loginManagerMock.Object);

            authorization = new Authorization { Token = token, Role = "Administrator", UserName = "Juan"};
            loginModel = new LoginModelRequest()
            {
                UserName = "Juan",
                Password = "123456"
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            _loginManagerMock.VerifyAll();
        }

        [TestMethod]
        public void Login_Ok()
        {
            //Arrange
            _loginManagerMock.Setup(x => x.Login(loginModel.UserName, loginModel.Password)).Returns(authorization);

            //Act
            var result = _loginController.Login(loginModel);

            //Assert
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            var value = objectResult.Value as LoginModelResponse;

            //Assert
            Assert.AreEqual(200, statusCode);
            Assert.AreEqual(token, value.token);
            Assert.AreEqual("Juan", value.userName);
            Assert.AreEqual("Administrator", value.role);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void Login_Failed_ResourceNotFoundException()
        {
            //Arrange
            var resourceNotFoundException = "ResourceNotFoundException";
            _loginManagerMock.Setup(service => service.Login(loginModel.UserName, loginModel.Password))
                .Throws(new ResourceNotFoundException(resourceNotFoundException));

            //Act
            var result = _loginController.Login(loginModel);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void Login_Failed_InvalidResourceException()
        {
            //Arrange
            var invalidResourceException = "InvalidResourceException";
            _loginManagerMock.Setup(service => service.Login(loginModel.UserName, loginModel.Password))
                .Throws(new InvalidResourceException(invalidResourceException));

            //Act
            var result = _loginController.Login(loginModel);
        }


    }
}
