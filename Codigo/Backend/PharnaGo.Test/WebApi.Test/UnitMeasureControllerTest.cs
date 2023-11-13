using Microsoft.AspNetCore.Mvc;
using Moq;
using PharmaGo.Domain.Entities;
using PharmaGo.IBusinessLogic;
using PharmaGo.WebApi.Controllers;
using PharmaGo.WebApi.Models.Out;

namespace PharmaGo.Test.WebApi.Test
{
    [TestClass]
    public class UnitMeasureControllerTest
    {
        private UnitMeasure _unitMeasure;
        private UnitMeasuresController _unitMeasuresController;
        private UnitMeasureBasicModel _unitMeasureModel;
        private Mock<IUnitMeasureManager> _unitMeasureManagerMock;

        [TestInitialize]
        public void SetUp()
        {
            _unitMeasureManagerMock = new Mock<IUnitMeasureManager>(MockBehavior.Strict);
            _unitMeasuresController = new UnitMeasuresController(_unitMeasureManagerMock.Object);
            _unitMeasure = new UnitMeasure()
            {
                Id = 1,
                Name = "g",
                Deleted = false
            };

            _unitMeasureModel = new UnitMeasureBasicModel(_unitMeasure);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _unitMeasureManagerMock.VerifyAll();
        }

        [TestMethod]
        public void Get_All_UnitMeasure_ShouldReturnOk()
        {
            //Arrange
            ICollection<UnitMeasure> unitMeasures = new List<UnitMeasure>();
            unitMeasures.Add(_unitMeasure);
            _unitMeasureManagerMock.Setup(i => i.GetAll()).Returns(unitMeasures);

            //Act
            var result = _unitMeasuresController.GetAll();
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            var value = objectResult.Value as IEnumerable<UnitMeasureBasicModel>;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, statusCode);
            Assert.AreEqual(1, value.ElementAt(0).Id);
            Assert.AreEqual("g", value.ElementAt(0).Name);
        }
    }
}

