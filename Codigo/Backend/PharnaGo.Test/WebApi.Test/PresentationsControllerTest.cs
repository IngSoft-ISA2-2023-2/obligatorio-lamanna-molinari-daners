using Microsoft.AspNetCore.Mvc;
using Moq;
using PharmaGo.Domain.Entities;
using PharmaGo.IBusinessLogic;
using PharmaGo.WebApi.Controllers;
using PharmaGo.WebApi.Models.Out;

namespace PharmaGo.Test.WebApi.Test
{
    [TestClass]
    public class PresentationsControllerTest
    {
        private Presentation _presentation;
        private PresentationsController _presentationsController;
        private PresentationBasicModel _presentationModel;
        private Mock<IPresentationManager> _presentationManagerMock;

        [TestInitialize]
        public void SetUp()
        {
            _presentationManagerMock = new Mock<IPresentationManager>(MockBehavior.Strict);
            _presentationsController = new PresentationsController(_presentationManagerMock.Object);
            _presentation = new Presentation()
            {
                Id = 1,
                Name = "tablet",
                Deleted = false
            };

            _presentationModel = new PresentationBasicModel(_presentation);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _presentationManagerMock.VerifyAll();
        }

        [TestMethod]
        public void Get_All_Presentations_ShouldReturnOk()
        {
            //Arrange
            ICollection<Presentation> presentations = new List<Presentation>();
            presentations.Add(_presentation);
            _presentationManagerMock.Setup(i => i.GetAll()).Returns(presentations);

            //Act
            var result = _presentationsController.GetAll();
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            var value = objectResult.Value as IEnumerable<PresentationBasicModel>;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, statusCode);
            Assert.AreEqual(1, value.ElementAt(0).Id);
            Assert.AreEqual("tablet", value.ElementAt(0).Name);
        }
    }
}
