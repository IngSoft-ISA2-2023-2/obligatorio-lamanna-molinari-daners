using Moq;
using PharmaGo.BusinessLogic;
using PharmaGo.DataAccess.Repositories;
using PharmaGo.Domain.Entities;
using PharmaGo.IDataAccess;

namespace PharmaGo.Test.BusinessLogic.Test
{
    [TestClass]
    public class UnitMeasureManagerTest
    {
        private Mock<IRepository<UnitMeasure>> _unitMeasureRepository;
        private UnitMeasureManager _unitMeasureManager;
        UnitMeasure _unitMeasure;
        UnitMeasure _unitMeasure2;

        [TestInitialize]
        public void SetUp()
        {
            _unitMeasureRepository = new Mock<IRepository<UnitMeasure>>(MockBehavior.Strict);
            _unitMeasureManager = new UnitMeasureManager(_unitMeasureRepository.Object);
            _unitMeasure = new UnitMeasure { Name = "g", Deleted = false };
            _unitMeasure2 = new UnitMeasure { Name = "l", Deleted = false };
        }

        [TestCleanup]
        public void CleanUp()
        {
            _unitMeasureRepository.VerifyAll();
        }

        [TestMethod]
        public void Get_All_UnitMeasures_ShouldReturnOk()
        {
            //Arrange
            var unitMeasureList = new List<UnitMeasure>
            {
                _unitMeasure,
                _unitMeasure2
            };
            _unitMeasureRepository.Setup(y => y.GetAllByExpression(s => s.Deleted == false)).Returns(unitMeasureList);

            //Act
            var response = _unitMeasureManager.GetAll();

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.Count(), 2);
        }

    }
}
