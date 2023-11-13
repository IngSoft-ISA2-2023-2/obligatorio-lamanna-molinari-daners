using System.Linq.Expressions;
using System.Text.RegularExpressions;
using LinqKit;
using Moq;
using PharmaGo.BusinessLogic;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;
using PharmaGo.Exceptions;
using PharmaGo.IDataAccess;

namespace PharmaGo.Test.BusinessLogic.Test
{
    [TestClass]
    public class InvitationManagerTest
    {
        private Mock<IRepository<Invitation>> _invitationMock;
        private Mock<IRepository<Pharmacy>> _pharmacyMock;
        private Mock<IRepository<Role>> _roleMock;
        private Mock<IRepository<Session>> _sessionMock;
        private Mock<IRepository<User>> _userMock;
        private InvitationManager _invitationManager;

        [TestInitialize]
        public void SetUp()
        {
            _invitationMock = new Mock<IRepository<Invitation>>(MockBehavior.Strict);
            _pharmacyMock = new Mock<IRepository<Pharmacy>>(MockBehavior.Strict);
            _roleMock = new Mock<IRepository<Role>>(MockBehavior.Strict);
            _sessionMock = new Mock<IRepository<Session>>(MockBehavior.Strict);
            _userMock = new Mock<IRepository<User>>(MockBehavior.Strict);

            _invitationManager = new InvitationManager(_invitationMock.Object,
                _pharmacyMock.Object, _roleMock.Object, _sessionMock.Object,
                _userMock.Object);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _invitationMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateInvitation_WithNullUserName_ShouldReturnException()
        {
            var token = "Test";
            var user = new User() { Id = 1, Role = new Role() { Name = "Administrator" } };

            var invitation = new Invitation()
            {
                Id = 1,
                Role = new Role() { Id = 1, Name = "Administrador" },
                Created = DateTime.Now
            };

            _sessionMock.Setup(session => session.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>())).Returns(new Session());
            _userMock.Setup(user => user.GetOneDetailByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);

            _roleMock.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Role, bool>>>())).Returns(new Role() { Id = 1, Name = "Administrator" });

            var result = _invitationManager.CreateInvitation(token, invitation);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateInvitation_WithNullRole_ShouldReturnException()
        {
            var token = "Test";
            var user = new User() { Id = 1, Role = new Role() { Name = "Administrator" } };

            var invitation = new Invitation()
            {
                Id = 1,
                UserName = "jcastro@test.com.uy",
                UserCode = "123456",
                Pharmacy = new Pharmacy() { Id = 1, Name = "PharmaGo", Address = "Montevideo" },
                Created = DateTime.Now
            };

            _sessionMock.Setup(session => session.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>())).Returns(new Session());
            _userMock.Setup(user => user.GetOneDetailByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);

            var result = _invitationManager.CreateInvitation(token, invitation);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateInvitation_WithInvalidUserName_ShouldReturnException()
        {
            var token = "Test";
            var user = new User() { Id = 1, Role = new Role() { Name = "Administrator" } };

            var invitation = new Invitation()
            {
                Id = 1,
                Created = DateTime.Now
            };

            _sessionMock.Setup(session => session.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>()))
                .Returns(new Session());
            _userMock.Setup(user => user.GetOneDetailByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);

            var result = _invitationManager.CreateInvitation(token, invitation);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateInvitation_WithInvalidRole_ShouldReturnException()
        {
            var token = "Test";
            var user = new User() { Id = 1, Role = new Role() { Name = "Administrator" } };
            Invitation nullInvitation = null;
            Role nullRole = null;
            var invitation = new Invitation()
            {
                Id = 1,
                UserName = "jcastro@test.com.uy",
                UserCode = "123456",
                Pharmacy = new Pharmacy() { Id = 1, Name = "PharmaGo", Address = "Montevideo" },
                Role = new Role() { Name = "Administrador" },
                Created = DateTime.Now
            };

            _sessionMock.Setup(session => session.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>()))
                .Returns(new Session());
            _userMock.Setup(user => user.GetOneDetailByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);

            //_invitationMock.Setup(i => i.GetOneByExpression(It.IsAny<Expression<Func<Invitation, bool>>>())).Returns(nullInvitation);
            _roleMock.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Role, bool>>>())).Returns(nullRole);

            var result = _invitationManager.CreateInvitation(token, invitation);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateInvitation_WithInvalidPharmacy_ShouldReturnException()
        {
            var token = "Test";
            var user = new User() { Id = 1, Role = new Role() { Name = "Administrator" } };
            Invitation nullInvitation = null;
            var role = new Role() { Id = 1, Name = "Administrador" };
            Pharmacy pharmacy = null;
            var invitation = new Invitation()
            {
                Id = 1,
                Pharmacy = new Pharmacy() { Name = "PharmaGo2"},
                UserName = "jcastro",
                Role = new Role() { Id = 1, Name = "Administrador" }
            };

            _sessionMock.Setup(session => session.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>()))
                .Returns(new Session());
            _userMock.Setup(user => user.GetOneDetailByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);

            _roleMock.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Role, bool>>>())).Returns(role);
            _pharmacyMock.Setup(i => i.GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns(pharmacy);

            var result = _invitationManager.CreateInvitation(token, invitation);
        }

        [TestMethod]
        public void CreaUserCode_WithInvalidValue_ShouldReturnException()
        {
            var userCode = "12345a";
            string validUserCode = @"^[0-9]{6}$";
            Regex regexUserCode = new(validUserCode);

            var result = _invitationManager.CreateUserCode();

            Assert.IsTrue(!regexUserCode.IsMatch(userCode));
            Assert.IsTrue(regexUserCode.IsMatch(result));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateInvitation_WithInvalidUserCode_ShouldReturnException()
        {
            var token = "Test";
            var user = new User() { Id = 1, Role = new Role() { Name = "Administrator" } };

            Invitation nullInvitation = null;
            var role = new Role() { Id = 1, Name = "Administrador" };
            Pharmacy pharmacy = null;
            var invitation = new Invitation()
            {
                Id = 1,
                Pharmacy = new Pharmacy() { Name = "PharmaGo2" },
                UserName = "jcastro",
                Role = new Role() { Id = 1, Name = "Administrador" }
            };

            _sessionMock.Setup(session => session.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>()))
                .Returns(new Session());
            _userMock.Setup(user => user.GetOneDetailByExpression(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user);

            _roleMock.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Role, bool>>>()))
                .Returns(role);
            _pharmacyMock.Setup(i => i.GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>()))
                .Returns(pharmacy);

            var result = _invitationManager.CreateInvitation(token, invitation);
        }

        [TestMethod]
        public void GetAllInvitations_WithNullSearchCriteria_ShouldReturnAllInvitations()
        {
            //Arrange
            _invitationMock.Setup(repository => repository.
            GetAllBasicByExpression(It.IsAny<Expression<Func<Invitation, bool>>>())).Returns( new List<Invitation>()
            {
                new Invitation() 
                    {
                        Id = 1,
                        Pharmacy = new Pharmacy() { Id = 1, Name = "Pharmacy" },
                        Role = new Role() { Id = 1, Name = "Owner" },
                        UserCode = "123456",
                        UserName = "test1",
                        IsActive = true
                    },
                new Invitation()
                    {
                        Id = 2,
                        Pharmacy = new Pharmacy() { Id = 1, Name = "Pharmacy" },
                        Role = new Role() { Id = 2, Name = "Employee" },
                        UserCode = "654321",
                        UserName = "test2",
                        IsActive = true
                    }
            });

            //Act
            var invitations = _invitationManager.GetAllInvitations(new InvitationSearchCriteria());

            //Assert
            Assert.AreEqual(2, invitations.Count());
        }

        [TestMethod]
        public void GetAllInvitation_WithSearchCriteria_ShouldReturnAllInvitations()
        {
            //Arrange
            _invitationMock.Setup(repository => repository.
            GetAllBasicByExpression(It.IsAny<Expression<Func<Invitation, bool>>>())).Returns(new List<Invitation>());
            var searchCriteria = new InvitationSearchCriteria() { Pharmacy = "Pharmacy" };

            //Act
            var invitations = _invitationManager.GetAllInvitations(searchCriteria);

            //Assert
            Assert.IsNotNull(invitations);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void UpdateInvitation_WithInvalidId_ShouldReturnException()
        {
            //Arrange
            var id = 0;
            var invitation = new Invitation();

            _invitationMock.Setup(invitation => invitation
            .GetOneDetailByExpression(It.IsAny<Expression<Func<Invitation, bool>>>())).Returns((Invitation)null);

            //Act
            var result = _invitationManager.UpdateInvitation(id, invitation);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void UpdateInvitation_WithNullRole_ShouldReturnNotFoundException()
        {
            //Arrange
            var id = 1;
            var invitation = new Invitation() { Role = new Role() { Name = "Ownerrr"} };

            _invitationMock.Setup(invitation => invitation
            .GetOneDetailByExpression(It.IsAny<Expression<Func<Invitation, bool>>>())).Returns(new Invitation());
            _roleMock.Setup(role => role.GetOneByExpression(It.IsAny<Expression<Func<Role, bool>>>())).Returns((Role)null);

            //Act
            var result = _invitationManager.UpdateInvitation(id, invitation);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void UpdateInvitation_WithNullUserName_ShouldReturnInvalidException()
        {
            //Arrange
            var id = 1;
            var invitation = new Invitation() { Role = new Role() { Name = "Owner" } };

            _invitationMock.Setup(invitation => invitation
            .GetOneDetailByExpression(It.IsAny<Expression<Func<Invitation, bool>>>())).Returns(new Invitation());
            _roleMock.Setup(role => role.GetOneByExpression(It.IsAny<Expression<Func<Role, bool>>>())).Returns(new Role());

            //Act
            var result = _invitationManager.UpdateInvitation(id, invitation);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void UpdateInvitation_WithExistingUserName_ShouldReturnInvalidException()
        {
            //Arrange
            var id = 1;
            var invitation = new Invitation() { UserName = "test" };

            _invitationMock.Setup(invitation => invitation
            .GetOneDetailByExpression(It.IsAny<Expression<Func<Invitation, bool>>>())).Returns(new Invitation());

            _roleMock.Setup(role => role.GetOneByExpression(It.IsAny<Expression<Func<Role, bool>>>())).Returns(new Role());

            _invitationMock.Setup(invitation => invitation
            .GetOneByExpression(It.IsAny<Expression<Func<Invitation, bool>>>())).Returns(new Invitation());

            //Act
            var result = _invitationManager.UpdateInvitation(id, invitation);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void UpdateInvitation_WithRoleAdministrator_ShouldNotHavePharmacy()
        {
            //Arrange
            var id = 1;
            var invitation = new Invitation() {
                Pharmacy = new Pharmacy() { Id = 1, Name = "Pharmacy" },
                Role = new Role() { Name = "Administrator" }, UserCode = "123456"};

            _invitationMock.Setup(invitation => invitation
            .GetOneDetailByExpression(It.IsAny<Expression<Func<Invitation, bool>>>())).Returns(new Invitation());

            _roleMock.Setup(role => role.GetOneByExpression(It.IsAny<Expression<Func<Role, bool>>>())).Returns(new Role());

            //Act
            var result = _invitationManager.UpdateInvitation(id, invitation);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void UpdateInvitation_WithRoleEmployee_ShouldHavePharmacy()
        {
            //Arrange
            var id = 1;
            var invitation = new Invitation()
            {
                Role = new Role() { Name = "Employee" },
                UserName = "test",
                UserCode = "123456"
            };

            _invitationMock.Setup(invitation => invitation
            .GetOneDetailByExpression(It.IsAny<Expression<Func<Invitation, bool>>>())).Returns(new Invitation());

            _roleMock.Setup(role => role.GetOneByExpression(It.IsAny<Expression<Func<Role, bool>>>())).Returns(new Role());

            //Act
            var result = _invitationManager.UpdateInvitation(id, invitation);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void UpdateInvitation_WithRoleOwner_ShouldHavePharmacy()
        {
            //Arrange
            var id = 1;
            var invitation = new Invitation()
            {
                Role = new Role() { Name = "Owner" },
                UserName = "test",
                UserCode = "123456"
            };

            _invitationMock.Setup(invitation => invitation
            .GetOneDetailByExpression(It.IsAny<Expression<Func<Invitation, bool>>>())).Returns(new Invitation());

            _roleMock.Setup(role => role.GetOneByExpression(It.IsAny<Expression<Func<Role, bool>>>())).Returns(new Role());

            //Act
            var result = _invitationManager.UpdateInvitation(id, invitation);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void UpdateInvitation_WithInvalidPharmacy_ShouldReturnException()
        {
            //Arrange
            var id = 1;
            var invitation = new Invitation()
            {
                Pharmacy = new Pharmacy() { Name = "Pharmacy" },
                Role = new Role() { Name = "Owner" },
                UserName = "test",
                UserCode = "123456"
            };

            _invitationMock.Setup(invitation => invitation
            .GetOneDetailByExpression(It.IsAny<Expression<Func<Invitation, bool>>>())).Returns(new Invitation());

            _roleMock.Setup(role => role.GetOneByExpression(It.IsAny<Expression<Func<Role, bool>>>())).Returns(new Role());

            _invitationMock.Setup(invitation => invitation
            .GetOneByExpression(It.IsAny<Expression<Func<Invitation, bool>>>())).Returns((Invitation)null);

            _pharmacyMock.Setup(pharmacy => pharmacy
            .GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns((Pharmacy)null);

            //Act
            var result = _invitationManager.UpdateInvitation(id, invitation);
        }

        [TestMethod]
        public void UpdateInvitation_ShouldReturnUpdatedInvitation()
        {
            //Arrange
            var id = 1;
            var invitationToUpdate = new Invitation()
            {
                Pharmacy = new Pharmacy() { Name = "Pharmacy" },
                Role = new Role() { Name = "Owner" },
                UserName = "test2",
                UserCode = "123456"
            };
            var invitation = new Invitation()
            {
                Id = 1,
                Pharmacy = new Pharmacy() { Name = "Pharmacy" },
                Role = new Role() { Name = "Owner" },
                UserName = "test",
                UserCode = "123456"
            };

            _invitationMock.Setup(invitation => invitation
            .GetOneDetailByExpression(It.IsAny<Expression<Func<Invitation, bool>>>())).Returns(invitation);

            _roleMock.Setup(role => role.GetOneByExpression(It.IsAny<Expression<Func<Role, bool>>>())).Returns(new Role());

            _invitationMock.Setup(invitation => invitation
            .GetOneByExpression(It.IsAny<Expression<Func<Invitation, bool>>>())).Returns((Invitation)null);

            _pharmacyMock.Setup(pharmacy => pharmacy
            .GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns(new Pharmacy());

            _invitationMock.Setup(invitation => invitation.UpdateOne(It.IsAny<Invitation>()));
            _invitationMock.Setup(invitation => invitation.Save());

            //Act
            var result = _invitationManager.UpdateInvitation(id, invitationToUpdate);

            //Assert
            Assert.IsNotNull(invitationToUpdate);
            Assert.AreEqual(id, invitation.Id);
            Assert.AreEqual(invitation.UserName, invitationToUpdate.UserName);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void UpdateInvitation_WithNotActiveInvitation_ShouldReturnException()
        {
            //Arrange
            var id = 1;
            var invitationToUpdate = new Invitation()
            {
                Pharmacy = new Pharmacy() { Name = "Pharmacy" },
                Role = new Role() { Name = "Owner" },
                UserName = "test2",
                UserCode = "123456"
            };
            var invitation = new Invitation()
            {
                Id = 1,
                Pharmacy = new Pharmacy() { Name = "Pharmacy" },
                Role = new Role() { Name = "Owner" },
                UserName = "test",
                UserCode = "123456",
                IsActive = false
            };

            _invitationMock.Setup(invitation => invitation
            .GetOneDetailByExpression(It.IsAny<Expression<Func<Invitation, bool>>>())).Returns(invitation);

            //Act
            var result = _invitationManager.UpdateInvitation(id, invitationToUpdate);
        }

        [TestMethod]
        public void UpdateInvitation_WithUserCode_ShouldUpdateInvitationUserCode()
        {
            //Arrange
            //Arrange
            var id = 1;
            var invitationToUpdate = new Invitation()
            {
                UserCode = "654321"
            };
            var invitation = new Invitation()
            {
                Id = 1,
                Pharmacy = new Pharmacy() { Name = "Pharmacy" },
                Role = new Role() { Name = "Owner" },
                UserName = "test",
                UserCode = "123456",
                IsActive = true
            };

            _invitationMock.Setup(invitation => invitation
            .GetOneDetailByExpression(It.IsAny<Expression<Func<Invitation, bool>>>())).Returns(invitation);

            _invitationMock.Setup(invitation => invitation.UpdateOne(It.IsAny<Invitation>()));
            _invitationMock.Setup(invitation => invitation.Save());

            //Act
            var result = _invitationManager.UpdateInvitation(id, invitationToUpdate);

            //Assert
            Assert.AreNotEqual("123456", invitation.UserCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void CreateInvitation_WithInvalidToken_ShouldReturnException()
        {
            //Arrange
            var token = "Test";
            var user = new User() { Id = 1, Role = new Role() { Name = "Administrator" } };
            var invitation = new Invitation() { UserName = "Test" };
            _sessionMock.Setup(session => session.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>()))
                .Returns((Session)null);

            //Act
            _invitationManager.CreateInvitation(token, invitation);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateInvitation_No_Token_Fail()
        {
            //Arrange
            var token = "";
            var user = new User() { Id = 1, Role = new Role() { Name = "Administrator" } };
            var invitation = new Invitation() { UserName = "Test" };

            //Act
            _invitationManager.CreateInvitation(token, invitation);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void CreateInvitation_No_User_Fail()
        {
            //Arrange
            var token = "Test";
            User user = null;
            var invitation = new Invitation() { UserName = "Test" };
            _sessionMock.Setup(session => session.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>()))
                .Returns(new Session { Id = 1, Token = new Guid("004fb0a6-847f-4194-9115-74f06cdac35d"), UserId = 1});
            _userMock.Setup(user => user.GetOneDetailByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);

            //Act
            _invitationManager.CreateInvitation(token, invitation);
        }


        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void CreateInvitation_WithInvalidUserRole_ShouldReturnException()
        {
            //Arrange
            var token = "Test";
            var invitation = new Invitation() { UserName = "Test" };
            _sessionMock.Setup(session => session.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>()))
                .Returns((Session)null);

            //Act
            _invitationManager.CreateInvitation(token, invitation);
        }

        [TestMethod]
        public void CreateInvitation_WithOwnerUserRole_ShouldValidateUserNameOnly()
        {
            //Arrange
            var token = "Test";
            var user = new User() { Id = 1, Role = new Role() { Name = "Owner" } };
            var invitation = new Invitation() { UserName = "Test" };
            _sessionMock.Setup(session => session.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>()))
                .Returns(new Session());

            _userMock.Setup(user => user.GetOneDetailByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);

            _invitationMock.Setup(i => i.GetOneByExpression(It.IsAny<Expression<Func<Invitation, bool>>>())).Returns((Invitation)null);
            _roleMock.Setup(role => role.GetOneByExpression(It.IsAny<Expression<Func<Role, bool>>>())).Returns(new Role());

            _invitationMock.Setup(repository => repository.InsertOne(invitation));
            _invitationMock.Setup(repository => repository.Save());

            //Act
            _invitationManager.CreateInvitation(token, invitation);
            

            //
            Assert.IsNotNull(invitation);
        }

        [TestMethod]
        public void CreateInvitation_WihtOwnerRole_ShouldSetOwnerPharmacy()
        {
            //Arrange
            var token = "Test";
            var user = new User() { Id = 1, Role = new Role() { Name = "Owner" }, Pharmacy = new Pharmacy() { Id = 1, Name = "Pharmacy" }};
            var invitation = new Invitation() { UserName = "Test" };
            _sessionMock.Setup(session => session.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>()))
                .Returns(new Session());

            _userMock.Setup(user => user.GetOneDetailByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);

            _invitationMock.Setup(i => i.GetOneByExpression(It.IsAny<Expression<Func<Invitation, bool>>>())).Returns((Invitation)null);
            _roleMock.Setup(role => role.GetOneByExpression(It.IsAny<Expression<Func<Role, bool>>>())).Returns(new Role());

            _invitationMock.Setup(repository => repository.InsertOne(invitation));
            _invitationMock.Setup(repository => repository.Save());

            //Act
            _invitationManager.CreateInvitation(token, invitation);


            //
            Assert.IsNotNull(invitation);
            Assert.AreEqual("Pharmacy", invitation.Pharmacy.Name);
        }

        [TestMethod]
        public void CreateInvitation_WithOwnerRole_ShouldSetEmployeeRole()
        {
            //Arrange
            var token = "Test";
            var user = new User() { Id = 1, Role = new Role() { Name = "Owner" }, Pharmacy = new Pharmacy() { Id = 1, Name = "Pharmacy" } };
            var invitation = new Invitation() { UserName = "Test" };

            _sessionMock.Setup(session => session.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>()))
                .Returns(new Session());
            _userMock.Setup(user => user.GetOneDetailByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);
            _invitationMock.Setup(i => i.GetOneByExpression(It.IsAny<Expression<Func<Invitation, bool>>>())).Returns((Invitation)null);
            _roleMock.Setup(role => role.GetOneByExpression(It.IsAny<Expression<Func<Role, bool>>>())).Returns(new Role() { Id = 1, Name = "Employee"});

            _invitationMock.Setup(repository => repository.InsertOne(invitation));
            _invitationMock.Setup(repository => repository.Save());

            //Act
            _invitationManager.CreateInvitation(token, invitation);


            //
            Assert.IsNotNull(invitation);
            Assert.AreEqual("Employee", invitation.Role.Name);
        }

        [TestMethod]
        public void Get_By_Id_Ok()
        {
            //Arrange
            var invitation = new Invitation() { UserName = "Test", Id = 1, IsActive = true, UserCode = "333222" };

            _invitationMock.Setup(i => i.GetOneDetailByExpression(It.IsAny<Expression<Func<Invitation, bool>>>())).Returns(invitation);

            //Act
            Invitation result = _invitationManager.GetById(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.UserName, "Test");
            Assert.AreEqual(result.Id, 1);
            Assert.AreEqual(result.IsActive, true);
            Assert.AreEqual(result.UserCode, "333222");
        }


    }
}
