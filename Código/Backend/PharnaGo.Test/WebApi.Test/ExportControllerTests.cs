using ExportationModel.ExportDomain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PharmaGo.DataAccess.Repositories;
using PharmaGo.Domain.Entities;
using PharmaGo.IBusinessLogic;
using PharmaGo.IDataAccess;
using PharmaGo.WebApi.Controllers;
using PharmaGo.WebApi.Models.In.Exports;
using PharmaGo.WebApi.Models.Out;
using System.Linq.Expressions;

namespace PharmaGo.Test.WebApi.Test
{
    [TestClass]
    public class ExportControllerTests
    {
        private ExportController _transactionController;
        private Mock<IExportManager> _transactionsManagerMock;
        private IEnumerable<string> transactions;
        private DrugsExportationModel drugExportationModel;
        private IEnumerable<Parameter> parameters;
        private IEnumerable<Parameter> emptyParameters;
        private readonly string jsonExporterName = "JSON exporter";
        private string token = "c80da9ed-1b41-4768-8e34-b728cae25d2f";
        private Session session = null;
        private User user = null;
        private Mock<IRepository<User>> _userRepository;
        private Mock<IRepository<Session>> _sessionRepository;

        [TestInitialize]
        public void SetUp()
        {
            _transactionsManagerMock = new Mock<IExportManager>(MockBehavior.Strict);
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = token;
            _transactionController = new ExportController(_transactionsManagerMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            transactions = new List<string> { "JSON", "XML", "CSV", "BD" };
            parameters = new List<Parameter> { new Parameter { InputValue = "string", InputName = "JSON exporter" } };
            drugExportationModel = new DrugsExportationModel { FormatName = "JSON exporter", Parameters = parameters };
            emptyParameters = new List<Parameter> { new Parameter { InputValue = "", InputName = "JSON exporter" } };

            _userRepository = new Mock<IRepository<User>>();
            _sessionRepository = new Mock<IRepository<Session>>();
            session = new Session { Id = 1, Token = new Guid(token), UserId = 1 };
            user = new User() { Id = 1, UserName = "test", Email = "test@gmail.com", Address = "test" };
        }

        [TestCleanup]
        public void Cleanup()
        {
            _transactionsManagerMock.VerifyAll();
        }

        [TestMethod]
        public void GetAllExportFormatsOk()
        {
            _transactionsManagerMock.Setup(u => u.GetAllExporters()).Returns(transactions);
            var result = _transactionController.GetAllExporters();
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void GetAllExportParametersOk()
        {
            _transactionsManagerMock.Setup(u => u.GetParameters(jsonExporterName)).Returns(emptyParameters);
            var result = _transactionController.GetParameters(jsonExporterName);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void PostExportDrugsOk()
        {
            _sessionRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>())).Returns(session);
            _userRepository.Setup(r => r.GetOneDetailByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);
            _transactionsManagerMock.Setup(u => u.ExportDrugs(drugExportationModel.FormatName, drugExportationModel.Parameters, token));
            
            var result = _transactionController.ExportDrugs(drugExportationModel);

            // Assert
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            var value = objectResult.Value as Object;

            Assert.AreEqual(200, statusCode);
            Assert.AreEqual(true, value);
        }
    }
}
