using Moq;
using PharmaGo.BusinessLogic;
using PharmaGo.Domain.Entities;
using PharmaGo.Exceptions;
using PharmaGo.IDataAccess;

namespace PharmaGo.Test.BusinessLogic.Test
{
    [TestClass]
    public class LoginManagerTests
    {
        private Mock<IRepository<User>> _userRespository;
        private Mock<IRepository<Session>> _sessionRespository;
        private LoginManager _loginManager;
        private Role role;

        [TestInitialize]
        public void SetUp()
        {
            _userRespository = new Mock<IRepository<User>>(MockBehavior.Strict);
            _sessionRespository = new Mock<IRepository<Session>>(MockBehavior.Strict);
            _loginManager = new LoginManager(_userRespository.Object, _sessionRespository.Object);

            role = new Role { Id = 1, Name = "Administrator"} ;
        }

        [TestCleanup]
        public void Cleanup()
        {
            _userRespository.VerifyAll();
            _sessionRespository.VerifyAll();
        }

        [TestMethod]
        public void Login_Ok()
        {
            //Arrange
            var userName = "Juan";
            var password = "12345";
            var userId = 1;
            Session session1 = null;

           _userRespository
                .Setup(x => x.GetOneDetailByExpression(x => x.UserName.ToLower().Equals(userName.ToLower())))
                .Returns(new User() { UserName = "Juan", Password = "12345", Id = userId, Role = role });

            _sessionRespository
                .Setup(x => x.GetOneByExpression(x => x.UserId == userId))
                .Returns(session1);
            _sessionRespository
                .Setup(x => x.InsertOne(It.IsAny<Session>()));
            _sessionRespository.Setup(x => x.Save());

            //Act
            var response = _loginManager.Login(userName, password);

            //Assert
             Assert.IsNotNull(response);
            Assert.AreEqual(response.Role, "Administrator");
        }

        [TestMethod]
        public void Login_Ok_Not_Null_Session()
        {
            //Arrange
            var userName = "Juan";
            var password = "12345";
            var userId = 1;
            var token_ = new Guid("c80da9ed-1b41-4768-8e34-b728cae25d2f");
            Session session1 = new Session { Id = 1, Token =  token_, UserId = 1};

            _userRespository
                 .Setup(x => x.GetOneDetailByExpression(x => x.UserName.ToLower().Equals(userName.ToLower())))
                 .Returns(new User() { UserName = "Juan", Password = "12345", Id = userId, Role = role });

            _sessionRespository
                .Setup(x => x.GetOneByExpression(x => x.UserId == userId))
                .Returns(session1);

            //Act
            var response = _loginManager.Login(userName, password);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.Role, "Administrator");
            Assert.AreEqual(response.Token, token_);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void Login_Fail_Empty_Username()
        {
            //Arrange
            var userName = "";
            var password = "12345";

            //Act
            var response = _loginManager.Login(userName, password);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void Login_Fail_Empty_Password()
        {
            //Arrange
            var userName = "Juan";
            var password = "";
            var userId = 1;

            _userRespository
                .Setup(x => x.GetOneDetailByExpression(x => x.UserName.ToLower().Equals(userName.ToLower())))
                .Returns(new User() { UserName = "Juan", Password = "123456788900", Id = userId, Role = role });

            //Act
            var response = _loginManager.Login(userName, password);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void Login_Fail_Null_User()
        {
            //Arrange
            var userName = "Juan";
            var password = "12345";
            User _user = null;

            _userRespository
                 .Setup(x => x.GetOneDetailByExpression(x => x.UserName.ToLower().Equals(userName.ToLower())))
                 .Returns(_user);

            //Act
            var response = _loginManager.Login(userName, password);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void Login_Invalid_Password()
        {
            //Arrange
            var userName = "Juan";
            var password = "12345";
            var userId = 1;
            Session session1 = null;

            _userRespository
                 .Setup(x => x.GetOneDetailByExpression(x => x.UserName.ToLower().Equals(userName.ToLower())))
                 .Returns(new User() { UserName = "Juan", Password = "123456788900", Id = userId, Role = role });

            //Act
            var response = _loginManager.Login(userName, password);
        }

        [TestMethod]
        public void Test_Login_Is_Valid()
        {
            //Arrange
            string token = "c80da9ed-1b41-4768-8e34-b728cae25d2f";
            Guid guidToken = new Guid("c80da9ed-1b41-4768-8e34-b728cae25d2f");

            _sessionRespository
                .Setup(x => x.GetOneByExpression(x => x.Token == guidToken))
                .Returns(new Session { Token = guidToken, UserId = 1, Id = 1});

            //Act
            var response = _loginManager.IsTokenValid(token);

            //Assert
            Assert.IsTrue(response);
        }

        [TestMethod]
        public void Test_Login_Is_Not_Valid()
        {
            //Arrange
            string token = "c80da9ed-1b41-4768-8e34-b728cae25d2f";
            Guid guidToken = new Guid("c80da9ed-1b41-4768-8e34-b728cae25d2f");
            Session session1 = null;    

            _sessionRespository
                .Setup(x => x.GetOneByExpression(x => x.Token == guidToken))
                .Returns(session1);

            //Act
            var response = _loginManager.IsTokenValid(token);

            //Assert
            Assert.IsFalse(response);
        }

        [TestMethod]
        public void Test_Login_Is_Role_Valid()
        {
            //Arrange
            string token = "c80da9ed-1b41-4768-8e34-b728cae25d2f";
            Guid guidToken = new Guid("c80da9ed-1b41-4768-8e34-b728cae25d2f");
            var userId = 1;
            string[] roles = new string[] { "Owner" };

            _sessionRespository
                .Setup(x => x.GetOneByExpression(x => x.Token == guidToken))
                .Returns(new Session { Token = guidToken, UserId = 1, Id = 1 });

            _userRespository
                 .Setup(x => x.GetOneDetailByExpression(x => x.Id == userId))
                 .Returns(new User() { UserName = "Juan", Password = "123456788900", Id = userId, Role = new Role { Id = 1, Name = "Owner" } });

            //Act
            var response = _loginManager.IsRoleValid(roles, token);

            //Assert
            Assert.IsTrue(response);
        }

        [TestMethod]
        public void Test_Login_Is_Role_Not_Valid()
        {
            //Arrange
            string token = "c80da9ed-1b41-4768-8e34-b728cae25d2f";
            Guid guidToken = new Guid("c80da9ed-1b41-4768-8e34-b728cae25d2f");
            var userId = 1;
            string[] roles = new string[] { "Owner" };

            _sessionRespository
                .Setup(x => x.GetOneByExpression(x => x.Token == guidToken))
                .Returns(new Session { Token = guidToken, UserId = 1, Id = 1 });

            _userRespository
                 .Setup(x => x.GetOneDetailByExpression(x => x.Id == userId))
                 .Returns(new User() { UserName = "Juan", Password = "123456788900", Id = userId, Role = new Role { Id = 1, Name = "Employee" } });

            //Act
            var response = _loginManager.IsRoleValid(roles, token);

            //Assert
            Assert.IsFalse(response);
        }

        [TestMethod]
        public void Test_Login_Is_Role_Not_Valid_Null_User()
        {
            //Arrange
            string token = "c80da9ed-1b41-4768-8e34-b728cae25d2f";
            Guid guidToken = new Guid("c80da9ed-1b41-4768-8e34-b728cae25d2f");
            var userId = 1;
            string[] roles = new string[] { "Owner" };
            User user_ = null; 

            _sessionRespository
                .Setup(x => x.GetOneByExpression(x => x.Token == guidToken))
                .Returns(new Session { Token = guidToken, UserId = 1, Id = 1 });

            _userRespository
                 .Setup(x => x.GetOneDetailByExpression(x => x.Id == userId))
                 .Returns(user_);

            //Act
            var response = _loginManager.IsRoleValid(roles, token);

            //Assert
            Assert.IsFalse(response);
        }
    }
}
