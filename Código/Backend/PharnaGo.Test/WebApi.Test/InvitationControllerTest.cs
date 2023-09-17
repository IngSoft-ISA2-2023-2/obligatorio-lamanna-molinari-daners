using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;
using PharmaGo.Exceptions;
using PharmaGo.IBusinessLogic;
using PharmaGo.WebApi.Controllers;
using PharmaGo.WebApi.Models.In;
using PharmaGo.WebApi.Models.Out;

namespace PharmaGo.Test.WebApi.Test
{
	[TestClass]
	public class InvitationControllerTest
	{
        private Invitation _invitation;
		private InvitationModelRequest _invitationModel;
		private InvitationsController _invitationController;
		private Mock<IInvitationManager> _invitationManagerMock;
        private InvitationSearchCriteriaModelRequest _searchCriteriaModelRequest;

		[TestInitialize]
		public void SetUp()
		{
			_invitationManagerMock = new Mock<IInvitationManager>(MockBehavior.Strict);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Test";
            _invitationController = new InvitationsController(_invitationManagerMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            _invitationModel = new InvitationModelRequest()
			{
				Pharmacy = "PharmaGo",
				UserName = "UserName",
				Role = "Empleado"
			};

            _invitation = new Invitation()
            {
                Id = 1,
                UserName = "UserName",
                UserCode = "123456",
                Pharmacy = new Pharmacy()
                {
                    Id = 1,
                    Name = "PharmaGo"
                },
                Role = new Role()
                {
                    Id = 1,
                    Name = "Empleado"
                }
            };

            _searchCriteriaModelRequest = new InvitationSearchCriteriaModelRequest();
		}

		[TestCleanup]
		public void CleanUp()
		{
			_invitationManagerMock.VerifyAll();
		}

		[TestMethod]
        [ExpectedException(typeof(Exception))]
		public void CreateInvitation_WithInternalError_ShouldReturnInternalError()
		{
            //Arrange
            _invitationManagerMock.Setup(i =>
			i.CreateInvitation(It.IsAny<string>(), It.IsAny<Invitation>())).Throws(new Exception());

            //Act
            var result = _invitationController.CreateInvitation(_invitationModel);
        }

		[TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
		public void CreateInvitation_WithInvalidResourceException_ShouldReturnBadRequest()
		{
            //Arrange
            _invitationManagerMock.Setup(i =>
            i.CreateInvitation(It.IsAny<string>(), It.IsAny<Invitation>())).Throws(
				new InvalidResourceException("Invalid resource."));

            //Act
            var result = _invitationController.CreateInvitation(_invitationModel);
        }

		[TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void CreateInvitation_WithResourceNotFoundException_ShouldReturnNotFound()
		{
            //Arrange
            _invitationManagerMock.Setup(i =>
            i.CreateInvitation(It.IsAny<string>(), It.IsAny<Invitation>())).Throws(
				new ResourceNotFoundException("Resource not found."));

            //Act
            var result = _invitationController.CreateInvitation(_invitationModel);
        }

		[TestMethod]
		public void CreateInvitation_ShouldReturnOk()
		{
            //Arrange
            _invitationManagerMock.Setup(i =>
            i.CreateInvitation(It.IsAny<string>(), It.IsAny<Invitation>())).Returns(_invitation);

            //Act
            var result = _invitationController.CreateInvitation(_invitationModel);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            //Assert
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void CreateInvitation_ShouldReturnInvitationModelResponse()
        {
            //Arrange
            _invitationManagerMock.Setup(i =>
            i.CreateInvitation(It.IsAny<string>(), It.IsAny<Invitation>())).Returns(_invitation);

            //Act
            var result = _invitationController.CreateInvitation(_invitationModel);
            var objectResult = result as ObjectResult;
            var invitationModelResponse = objectResult.Value as InvitationModelResponse;

            //Assert
            Assert.AreEqual(invitationModelResponse.UserName, _invitation.UserName);
            Assert.AreEqual(invitationModelResponse.UserCode, _invitation.UserCode);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetAllInvitations_WithInternalError_ShouldReturnInternalErrorException()
        {
            //Arrange
            _invitationManagerMock.Setup(manager => manager.GetAllInvitations(It.IsAny<InvitationSearchCriteria>()))
                .Throws(new Exception());

            //Act
            var result = _invitationController.GetAll(_searchCriteriaModelRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void GetAllInvitations_WithInvalidResource_ShouldReturnInvalidResourceException()
        {
            //Arrange
            _invitationManagerMock.Setup(manager => manager.GetAllInvitations(It.IsAny<InvitationSearchCriteria>()))
                .Throws(new InvalidResourceException("Invalid resource."));

            //Act
            var result = _invitationController.GetAll(_searchCriteriaModelRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void GetAllInvitations_WithNotFound_ShouldReturnNotFoundException()
        {
            //Arrange
            _invitationManagerMock.Setup(manager => manager.GetAllInvitations(It.IsAny<InvitationSearchCriteria>()))
                .Throws(new ResourceNotFoundException("Not found."));

            //Act
            var result = _invitationController.GetAll(_searchCriteriaModelRequest);
        }

        [TestMethod]
        public void GetAllInvitations_SearchCriteria_ShouldReturnAllInvitations()
        {
            //Arrange
            _invitationManagerMock.Setup(manager => manager.GetAllInvitations(It.IsAny<InvitationSearchCriteria>())).Returns(
                new List<Invitation>()
                {
                    new Invitation()
                    {
                        Id = 1,
                        Pharmacy = new Pharmacy() { Id = 1, Name = "Pharmacy" },
                        Role = new Role() { Id = 1, Name = "Administrator" },
                        UserCode = "123456",
                        UserName = "test1"
                    }
                });

            //Act
            var result = _invitationController.GetAll(_searchCriteriaModelRequest);

            var objectResult = result as OkObjectResult;
            var statusCode = objectResult.StatusCode;
            var value = objectResult.Value;

            //Assert
            Assert.IsNotNull(value);
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void GetAllInvitations_ShouldReturnPharmacyUserNameRoleUserCodeStatusInformation()
        {
            //Arrange
            _invitationManagerMock.Setup(manager => manager.GetAllInvitations(It.IsAny<InvitationSearchCriteria>())).Returns(
                new List<Invitation>()
                {
                    new Invitation()
                    {
                        Id = 1,
                        Pharmacy = new Pharmacy() { Id = 1, Name = "Pharmacy" },
                        Role = new Role() { Id = 1, Name = "Owner" },
                        UserCode = "123456",
                        UserName = "test1",
                        IsActive = true
                    }
                });

            //Act
            var result = _invitationController.GetAll(_searchCriteriaModelRequest);

            var objectResult = result as OkObjectResult;
            var statusCode = objectResult.StatusCode;
            var value = objectResult.Value as IEnumerable<InvitationSearchCriteriaModelResponse>;

            //Assert
            Assert.IsNotNull(value);
            Assert.AreEqual(200, statusCode);
            Assert.AreEqual("Pharmacy", value.ElementAt(0).Pharmacy.Name);
            Assert.AreEqual("test1", value.ElementAt(0).UserName);
            Assert.AreEqual("Owner", value.ElementAt(0).Role.Name);
            Assert.AreEqual("123456", value.ElementAt(0).UserCode);
            Assert.IsTrue(value.ElementAt(0).IsActive);
        }

        [TestMethod]
        public void GetAllInvitations_WithNullPhamaracy_ShouldReturnAllInvitations()
        {
            //Arrange
            _invitationManagerMock.Setup(manager => manager.GetAllInvitations(It.IsAny<InvitationSearchCriteria>())).Returns(
                new List<Invitation>()
                {
                    new Invitation()
                    {
                        Id = 1,
                        Role = new Role() { Id = 1, Name = "Owner" },
                        UserCode = "123456",
                        UserName = "test1",
                        IsActive = true
                    }
                });

            //Act
            var result = _invitationController.GetAll(_searchCriteriaModelRequest);

            var objectResult = result as OkObjectResult;
            var statusCode = objectResult.StatusCode;
            var value = objectResult.Value as IEnumerable<InvitationSearchCriteriaModelResponse>;

            //Assert
            Assert.IsNotNull(value);
            Assert.AreEqual(200, statusCode);
            Assert.IsNull(value.ElementAt(0).Pharmacy);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void UpdateInvitation_WithExceptionError_ShouldReturnException()
        {
            //Arrange
            _invitationManagerMock.Setup(manager => manager.UpdateInvitation(It.IsAny<int>(), It.IsAny<Invitation>()))
                .Throws(new Exception());

            //Act
            var result = _invitationController.UpdateInvitation(1, new InvitationModelRequest());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void UpdateInvitation_WithInvalidResource_ShouldReturnException()
        {
            //Arrange
            _invitationManagerMock.Setup(manager => manager.UpdateInvitation(It.IsAny<int>(), It.IsAny<Invitation>()))
                .Throws(new InvalidResourceException("Invalid resource."));

            //Act
            var result = _invitationController.UpdateInvitation(1, new InvitationModelRequest());
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void UpdateInvitation_WithNotFoundException_ShouldReturnException()
        {
            //Arrange
            _invitationManagerMock.Setup(manager => manager.UpdateInvitation(It.IsAny<int>(), It.IsAny<Invitation>()))
                .Throws(new ResourceNotFoundException("Not found."));

            //Act
            var result = _invitationController.UpdateInvitation(1, new InvitationModelRequest());
        }

        [TestMethod]
        public void UpdateInvitation_ShoudReturnUpdateInvitation()
        {
            //Arrange
            _invitationManagerMock.Setup(manager => manager.UpdateInvitation(It.IsAny<int>(), It.IsAny<Invitation>())).Returns(_invitation);

            //Act
            var result = _invitationController.UpdateInvitation(1, _invitationModel);

            //Assert
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            var value = objectResult.Value as InvitationDetailModelResponse;

            Assert.AreEqual(200, statusCode);
            Assert.IsNotNull(value);
        }

        [TestMethod]
        public void GetUserCode_ShouldReturnUserCode()
        {
            //Arrange
            var userCode = "123456";
            _invitationManagerMock.Setup(manager => manager.CreateUserCode()).Returns(userCode);

            //Act
            var result = _invitationController.UserCode();

            //Assert
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            var value = objectResult.Value as InvitationUserCodeModelResponse;

            Assert.AreEqual(200, statusCode);
            Assert.IsNotNull(value.UserCode);
        }

        [TestMethod]
        public void CreateInvitation_WithOwnerRole_ShouldCreateInvitation()
        {
            //Arrange
            _invitationManagerMock.Setup(i =>
            i.CreateInvitation(It.IsAny<string>(), It.IsAny<Invitation>())).Returns(_invitation);

            //Act
            var result = _invitationController.CreateInvitation(_invitationModel);

            //Assert
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            var value = objectResult.Value as InvitationUserCodeModelResponse;

            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void Invitation_Get_By_Id_Ok()
        {
            //Arrange
            _invitationManagerMock.Setup(i =>
            i.GetById(It.IsAny<int>())).Returns(_invitation);

            //Act
            var result = _invitationController.GetById(1);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            //Assert
            Assert.AreEqual(200, statusCode);
        }

    }
}

