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
    public class UsersControllerTests
    {
        private UsersController _userController;
        private Mock<IUsersManager> _userManagerMock;
        private UserModelRequest userModel;
        private User user;

        [TestInitialize]
        public void SetUp()
        {
            _userManagerMock = new Mock<IUsersManager>(MockBehavior.Strict);
            _userController = new UsersController(_userManagerMock.Object);

            userModel = new UserModelRequest()
            {
                UserName = "pedro901",
                UserCode = "980357",
                Email    = "pedro@gmail.com",
                Password = "12345678.",
                Address  = "Av. Italia 1256, Montevideo",
                RegistrationDate = new DateTime(2022, 09, 19, 14, 34, 44)
            };

            user = new User()
            {
                UserName = "pedro901",
                Email = "pedro@gmail.com",
                Password = "12345678.",
                Address = "Av. Italia 1256, Montevideo",
                RegistrationDate = new DateTime(2022, 09, 19, 14, 34, 44)
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            _userManagerMock.VerifyAll();
        }

        [TestMethod]
        public void Create_User_Ok()
        {
            //Arrange
            _userManagerMock
                .Setup(service => service.CreateUser(userModel.UserName, userModel.UserCode, userModel.Email, userModel.Password, userModel.Address, userModel.RegistrationDate))
                .Returns(user);

            //Act
            var result = _userController.CreateUser(userModel);

            //Assert
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult.StatusCode;
            var value = objectResult.Value as UserModelResponse;

            //Assert
            Assert.AreEqual(200, statusCode);
            Assert.AreEqual(value.UserName, userModel.UserName);
            Assert.AreEqual(value.Email, userModel.Email);
            Assert.AreEqual(value.Address, userModel.Address);
            Assert.AreEqual(value.RegistrationDate, userModel.RegistrationDate);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void Create_User_ResourceNotFoundException()
        {
            //Arrange
            var resourceNotFoundException = "ResourceNotFoundException";
            _userManagerMock.Setup(service => service.CreateUser(userModel.UserName, userModel.UserCode, userModel.Email, userModel.Password, userModel.Address, userModel.RegistrationDate))
                .Throws(new ResourceNotFoundException(resourceNotFoundException));

            //Act
            var result = _userController.CreateUser(userModel);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void Create_User_Failed_InvalidResourceException()
        {
            //Arrange
            var invalidResourceException = "InvalidResourceException";
            _userManagerMock.Setup(service => service.CreateUser(userModel.UserName, userModel.UserCode, userModel.Email, userModel.Password, userModel.Address, userModel.RegistrationDate))
                .Throws(new InvalidResourceException(invalidResourceException));

            //Act
            var result = _userController.CreateUser(userModel);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Create_User_InternalServerErrorException()
        {
            //Arrange
            var resourceNotFoundException = "Generic Exception";
            _userManagerMock.Setup(service => service.CreateUser(userModel.UserName, userModel.UserCode, userModel.Email, userModel.Password, userModel.Address, userModel.RegistrationDate))
                .Throws(new Exception(resourceNotFoundException));

            //Act
             var result = _userController.CreateUser(userModel);
        }

        [TestMethod]
        public void Test_UserBasicModel()
        {
            UserBasicModel userModel = new UserBasicModel(user);

            // Assert
            Assert.AreEqual(userModel.UserName, user.UserName);
            Assert.AreEqual(userModel.Id, 0);
        }

        

    }
}
