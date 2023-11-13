using Moq;
using PharmaGo.BusinessLogic;
using PharmaGo.DataAccess.Migrations;
using PharmaGo.Domain.Entities;
using PharmaGo.IBusinessLogic;
using PharmaGo.IDataAccess;

namespace PharmaGo.Test.BusinessLogic.Test
{
    [TestClass]
    public class PresentationManagerTest
    {
        private Mock<IRepository<Presentation>> _presentationRepository;
        private PresentationManager _presentationManager;
        Presentation _presentation;
        Presentation _presentation2;

        [TestInitialize]
        public void SetUp()
        {
            _presentationRepository = new Mock<IRepository<Presentation>>(MockBehavior.Strict);
            _presentationManager = new PresentationManager(_presentationRepository.Object);
            _presentation = new Presentation { Name = "tablet", Deleted = false};
            _presentation2 = new Presentation { Name = "liquid", Deleted = false };
        }

        [TestCleanup]
        public void CleanUp()
        {
            _presentationRepository.VerifyAll();
        }

        [TestMethod]
        public void Get_All_Presentations_ShouldReturnOk()
        {
            //Arrange
            var presentationList = new List<Presentation>
            {
                _presentation, 
                _presentation2
            };
            _presentationRepository.Setup(y => y.GetAllByExpression(s => s.Deleted == false)).Returns(presentationList);

            //Act
            var response = _presentationManager.GetAll();

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.Count(), 2);
        }
    }
}
