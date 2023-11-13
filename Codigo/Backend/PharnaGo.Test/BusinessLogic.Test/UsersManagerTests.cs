using Moq;
using PharmaGo.BusinessLogic;
using PharmaGo.Domain.Entities;
using PharmaGo.Exceptions;
using PharmaGo.IDataAccess;

namespace PharmaGo.Test.BusinessLogic.Test
{
    [TestClass]
    public class UsersManagerTests
    {
        private Mock<IRepository<User>> _userRespository;
        private Mock<IRepository<Invitation>> _invitationRespository;
        private UsersManager _userManager;
        Pharmacy pharmacy;
        Role role;
        Invitation invitation;
        User user;
        User exists;

        [TestInitialize]
        public void SetUp()
        {
            _userRespository = new Mock<IRepository<User>>(MockBehavior.Strict);
            _invitationRespository = new Mock<IRepository<Invitation>>(MockBehavior.Strict);
            _userManager = new UsersManager(_userRespository.Object, _invitationRespository.Object);

            pharmacy = new Pharmacy {Id = 1, Drugs = new List<Drug>(), 
                                    Address = "Av. Rivera 1234", Name = "Farmacia 5566", 
                                    Users = new List<User>()
            };
            role = new Role { Id = 1, Name = "Administrador" };
            invitation = new Invitation()
            {
                Id = 1,
                UserName = "",
                Created = new DateTime(2022, 09, 20, 12, 00, 00),
                Pharmacy = pharmacy,
                Role = role,
                IsActive = true
            };
            user = new User
            {
                Id = 4,
                Address = "Av. Italia 4478",
                Email = "juan@gmail.com",
                Password = "12345678.",
                RegistrationDate = new DateTime(2022, 09, 20, 14, 00, 00),
                UserName = "pedro901"
            };

            exists = null;
        }

        [TestCleanup]
        public void Cleanup()
        {
            _userRespository.VerifyAll();
            _invitationRespository.VerifyAll();
        }

        [TestMethod]
        public void Create_User_Ok()
        {
            //Arrange
            var UserName = "pedro901";
            var UserCode = "980357";
            var Address = "Av. Italia 4478";
            var Email = "juan@gmail.com";
            var Password = "Abcdef12345678.";
            var RegistrationDate = new DateTime(2022, 09, 20, 14, 00, 00);

           _userRespository.Setup(x => x.GetOneByExpression(u => u.UserName.ToLower() == UserName.ToLower())).Returns(exists);
           _userRespository.Setup(x => x.GetOneByExpression(u => u.Email.ToLower() == Email.ToLower())).Returns(exists);

            _invitationRespository
                 .Setup(x => x.GetOneDetailByExpression(i => i.UserName.ToLower() == UserName.ToLower() && i.UserCode == UserCode && i.IsActive)).Returns(invitation);
            user.Pharmacy = invitation.Pharmacy;
            user.Role = invitation.Role;

            _userRespository.Setup(x => x.InsertOne(It.IsAny<User>()));
            _userRespository.Setup(x => x.Save());

            invitation.IsActive = false;
            _invitationRespository.Setup(x => x.UpdateOne(invitation));
            _invitationRespository.Setup(x => x.Save());

            //Act
            var response = _userManager.CreateUser(UserName, UserCode, Email, Password, Address, RegistrationDate);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.Pharmacy.Id, 1);
            Assert.AreEqual(response.Role.Id, 1);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void Create_User_Fail_Invalid_UserName_Empty()
        {
            //Arrange
            var UserName = "";
            var UserCode = "980357";
            var Address = "Av. Italia 4478";
            var Email = "juan@gmail.com";
            var Password = "Abcdef12345678.";
            var RegistrationDate = new DateTime(2022, 09, 20, 14, 00, 00);

            //Act
            var response = _userManager.CreateUser(UserName, UserCode, Email, Password, Address, RegistrationDate);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void Create_User_Fail_Invalid_UserCode_Empty()
        {
            //Arrange
            var UserName = "pedro901";
            var UserCode = "";
            var Address = "Av. Italia 4478";
            var Email = "juan@gmail.com";
            var Password = "Abcdef12345678.";
            var RegistrationDate = new DateTime(2022, 09, 20, 14, 00, 00);

            //Act
            var response = _userManager.CreateUser(UserName, UserCode, Email, Password, Address, RegistrationDate);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]

        public void Create_User_Fail_Invalid_UserCode_Format()
        {
            //Arrange
            var UserName = "pedro901";
            var UserCode = "12345A";
            var Address = "Av. Italia 4478";
            var Email = "juan@gmail.com";
            var Password = "Abcdef12345678.";
            var RegistrationDate = new DateTime(2022, 09, 20, 14, 00, 00);

            //Act
            var response = _userManager.CreateUser(UserName, UserCode, Email, Password, Address, RegistrationDate);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void Create_User_Fail_Invalid_Email_Empty()
        {
            //Arrange
            var UserName = "pedro901";
            var UserCode = "123456";
            var Address = "Av. Italia 4478";
            var Email = "";
            var Password = "Abcdef12345678.";
            var RegistrationDate = new DateTime(2022, 09, 20, 14, 00, 00);

            //Act
            var response = _userManager.CreateUser(UserName, UserCode, Email, Password, Address, RegistrationDate);
        }

        [TestMethod]
        public void Create_User_Fail_Invalid_Email_Format()
        {
            //Arrange
            var UserName = "pedro901";
            var UserCode = "123456";
            var Address = "Av. Italia 4478";
            var Email = "pedrogmail.com";
            var Password = "Abcdef12345678.";
            var RegistrationDate = new DateTime(2022, 09, 20, 14, 00, 00);

            //Act
            try
            {
                var response = _userManager.CreateUser(UserName, UserCode, Email, Password, Address, RegistrationDate);
            }
            catch (InvalidResourceException ex)
            {
                //Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual(ex.Message, "Invalid Email");
            }

        }

        [TestMethod]
        public void Create_User_Fail_Invalid_Password_Empty()
        {
            //Arrange
            var UserName = "pedro901";
            var UserCode = "123456";
            var Address = "Av. Italia 4478";
            var Email = "pedro@gmail.com";
            var Password = "";
            var RegistrationDate = new DateTime(2022, 09, 20, 14, 00, 00);

            //Act
            try
            {
                var response = _userManager.CreateUser(UserName, UserCode, Email, Password, Address, RegistrationDate);
            }
            catch (InvalidResourceException ex)
            {
                //Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual(ex.Message, "Invalid Password");
            }

        }

        [TestMethod]
        public void Create_User_Fail_Invalid_Password_Format()
        {
            //Arrange
            var UserName = "pedro901";
            var UserCode = "123456";
            var Address = "Av. Italia 4478";
            var Email = "pedro@gmail.com";
            var Password = "abcd12345";
            var RegistrationDate = new DateTime(2022, 09, 20, 14, 00, 00);

            //Act
            try
            {
                var response = _userManager.CreateUser(UserName, UserCode, Email, Password, Address, RegistrationDate);
            }
            catch (InvalidResourceException ex)
            {
                //Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual(ex.Message, "Invalid Password");
            }

        }

        [TestMethod]
        public void Create_User_Fail_Invalid_Address_Empty()
        {
            //Arrange
            var UserName = "pedro901";
            var UserCode = "123456";
            var Address = "";
            var Email = "pedro@gmail.com";
            var Password = "Abcdef12345678.";
            var RegistrationDate = new DateTime(2022, 09, 20, 14, 00, 00);

            //Act
            try
            {
                var response = _userManager.CreateUser(UserName, UserCode, Email, Password, Address, RegistrationDate);
            }
            catch (InvalidResourceException ex)
            {
                //Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual(ex.Message, "Invalid Address");
            }

        }

        [TestMethod]
        public void Create_User_Fail_Invalid_Not_Active_Invitation()
        {
            //Arrange
            var UserName = "pedro901";
            var UserCode = "980357";
            var Address = "Av. Italia 4478";
            var Email = "juan@gmail.com";
            var Password = "Abcdef12345678.";
            var RegistrationDate = new DateTime(2022, 09, 20, 14, 00, 00);

            Invitation invitationNull = null;
            _invitationRespository
                 .Setup(x => x.GetOneDetailByExpression(i => i.UserName.ToLower() == UserName.ToLower() && i.UserCode == UserCode && i.IsActive)).Returns(invitationNull);

            //Act
            try
            {
                var response = _userManager.CreateUser(UserName, UserCode, Email, Password, Address, RegistrationDate);
            }
            catch (ResourceNotFoundException ex)
            {
                //Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual(ex.Message, "Invitation not found or is not currently active");
            }
        }

        [TestMethod]
        public void Create_User_Invalid_Username()
        {
            //Arrange
            var UserName = "pedro901";
            var UserCode = "980357";
            var Address = "Av. Italia 4478";
            var Email = "juan@gmail.com";
            var Password = "Abcdef12345678.";
            var RegistrationDate = new DateTime(2022, 09, 20, 14, 00, 00);

            User oldUser = new User { UserName = "pedro901", Email = "juan@gmail.com"};

            _invitationRespository
                 .Setup(x => x.GetOneDetailByExpression(i => i.UserName.ToLower() == UserName.ToLower() && i.UserCode == UserCode && i.IsActive)).Returns(invitation);
            _userRespository.Setup(x => x.GetOneByExpression(u => u.UserName.ToLower() == UserName.ToLower())).Returns(oldUser);
            
            //Act
            try
            {
                var response = _userManager.CreateUser(UserName, UserCode, Email, Password, Address, RegistrationDate);
            }
            catch (InvalidResourceException ex)
            {
                //Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual(ex.Message, "Invalid Username, Username already exists");
            }
        }


        [TestMethod]
        public void Create_User_Invalid_Email()
        {
            //Arrange
            var UserName = "pedro901";
            var UserCode = "980357";
            var Address = "Av. Italia 4478";
            var Email = "juan@gmail.com";
            var Password = "Abcdef12345678.";
            var RegistrationDate = new DateTime(2022, 09, 20, 14, 00, 00);

            User oldUser = new User { UserName = "pedro901", Email = "juan@gmail.com" };

            _invitationRespository
                 .Setup(x => x.GetOneDetailByExpression(i => i.UserName.ToLower() == UserName.ToLower() && i.UserCode == UserCode && i.IsActive)).Returns(invitation);
            _userRespository.Setup(x => x.GetOneByExpression(u => u.UserName.ToLower() == UserName.ToLower())).Returns(exists);
            _userRespository.Setup(x => x.GetOneByExpression(u => u.Email.ToLower() == Email.ToLower())).Returns(oldUser);

            //Act
            try
            {
                var response = _userManager.CreateUser(UserName, UserCode, Email, Password, Address, RegistrationDate);
            }
            catch (InvalidResourceException ex)
            {
                //Assert
                Assert.IsNotNull(ex);
                Assert.AreEqual(ex.Message, "Invalid Email, Email already exists");
            }
        }
    }

}
