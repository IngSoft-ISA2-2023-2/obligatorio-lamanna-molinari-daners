
using Moq;
using PharmaGo.BusinessLogic;
using PharmaGo.DataAccess.Repositories;
using PharmaGo.Domain.Entities;
using PharmaGo.IBusinessLogic;
using PharmaGo.IDataAccess;

namespace PharmaGo.Test.BusinessLogic.Test
{
    [TestClass]
    public class RoleManagerTest
    {

        private Mock<IRepository<Role>> _roleRepository;
        private RoleManager _roleManager;
        Role _role = null;

        [TestInitialize]
        public void SetUp()
        {
            _roleRepository = new Mock<IRepository<Role>>(MockBehavior.Strict);
            _roleManager = new RoleManager(_roleRepository.Object);
            _role = new Role { Name = "Administrator", Id = 1 };
        }

        [TestCleanup]
        public void CleanUp()
        {
            _roleRepository.VerifyAll();
        }

        [TestMethod]
        public void Get_All_Ok()
        {
            //Arrange
            var roleList = new List<Role>
            {
                _role
            };
            _roleRepository.Setup(y => y.GetAllByExpression(expression => true)).Returns(roleList);

            //Act
            var response = _roleManager.GetAll();

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.Count(), 1);

        }


    }
}
