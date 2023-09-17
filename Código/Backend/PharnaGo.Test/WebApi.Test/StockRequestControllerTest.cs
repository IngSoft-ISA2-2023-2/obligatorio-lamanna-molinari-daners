using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;
using PharmaGo.Exceptions;
using PharmaGo.IBusinessLogic;
using PharmaGo.WebApi.Controllers;
using PharmaGo.WebApi.Models.In;

namespace PharmaGo.Test.WebApi.Test
{
    [TestClass]
    public class StockRequestControllerTest
    {
        private Mock<IStockRequestManager> _stockRequestManagerMock;
        private StockRequestController _stockRequestController;
        private StockRequestModelRequest _stockRequestModelRequest;
        private string token = "c80da9ed-1b41-4768-8e34-b728cae25d2f";

        [TestInitialize]
        public void SetUp()
        {
            _stockRequestManagerMock = new Mock<IStockRequestManager>(MockBehavior.Strict);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = token;
            _stockRequestController = new StockRequestController(_stockRequestManagerMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            _stockRequestModelRequest = new StockRequestModelRequest()
            {
                Details = new List<StockRequestDetailModelRequest>()
                {
                    new StockRequestDetailModelRequest() { Drug = new DrugModelRequest() { Code = "XF324" }, Quantity = 50 }
                }
            };
        }

        [TestCleanup]
        public void CleanUp()
        {
            _stockRequestManagerMock.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CreateStockRequest_WithInternalError_ShouldReturnInternalError()
        {
            //Arrange
            _stockRequestManagerMock.Setup(i =>
            i.CreateStockRequest(It.IsAny<StockRequest>(), token)).Throws(new Exception());

            //Act
            var result = _stockRequestController.CreateStockRequest(_stockRequestModelRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateStockRequest_WithInvalidResourceException_ShouldReturnBadRequest()
        {
            //Arrange
            _stockRequestManagerMock.Setup(i =>
            i.CreateStockRequest(It.IsAny<StockRequest>(), token)).Throws(
                new InvalidResourceException("Invalid resource."));

            //Act
            var result = _stockRequestController.CreateStockRequest(_stockRequestModelRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void CreateStockRequest_WithResourceNotFoundException_ShouldReturnNotFound()
        {
            //Arrange
            _stockRequestManagerMock.Setup(i =>
            i.CreateStockRequest(It.IsAny<StockRequest>(), token)).Throws(
                new ResourceNotFoundException("Resource not found."));

            //Act
            var result = _stockRequestController.CreateStockRequest(_stockRequestModelRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ApproveStockRequest_WithInternalError_ShouldReturnInternalError()
        {
            //Arrange
            //Arrange
            var stockRequestId = 1;
            _stockRequestManagerMock.Setup(i =>
            i.ApproveStockRequest(It.IsAny<int>())).Throws(new Exception());

            //Act
            var result = _stockRequestController.ApproveStockRequest(stockRequestId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void ApproveStockRequest_WithInvalidResourceException_ShouldReturnBadRequest()
        {
            //Arrange
            //Arrange
            var stockRequestId = 1;
            _stockRequestManagerMock.Setup(i =>
            i.ApproveStockRequest(It.IsAny<int>())).Throws(
                new InvalidResourceException("Invalid resource."));

            //Act
            var result = _stockRequestController.ApproveStockRequest(stockRequestId);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void ApproveStockRequest_WithResourceNotFoundException_ShouldReturnNotFound()
        {
            //Arrange
            var stockRequestId = 1;
            _stockRequestManagerMock.Setup(i =>
            i.ApproveStockRequest(It.IsAny<int>())).Throws(
                new ResourceNotFoundException("Resource not found."));

            //Act
            var result = _stockRequestController.ApproveStockRequest(stockRequestId);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void RejectStockRequest_WithInternalError_ShouldReturnInternalError()
        {
            //Arrange
            //Arrange
            var stockRequestId = 1;
            _stockRequestManagerMock.Setup(i =>
            i.RejectStockRequest(It.IsAny<int>())).Throws(new Exception());

            //Act
            var result = _stockRequestController.RejectStockRequest(stockRequestId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void RejectStockRequest_WithInvalidResourceException_ShouldReturnBadRequest()
        {
            //Arrange
            //Arrange
            var stockRequestId = 1;
            _stockRequestManagerMock.Setup(i =>
            i.RejectStockRequest(It.IsAny<int>())).Throws(
                new InvalidResourceException("Invalid resource."));

            //Act
            var result = _stockRequestController.RejectStockRequest(stockRequestId);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void RejectStockRequest_WithResourceNotFoundException_ShouldReturnNotFound()
        {
            //Arrange
            var stockRequestId = 1;
            _stockRequestManagerMock.Setup(i =>
            i.RejectStockRequest(It.IsAny<int>())).Throws(
                new ResourceNotFoundException("Resource not found."));

            //Act
            var result = _stockRequestController.RejectStockRequest(stockRequestId);
        }

        [TestMethod]
        public void CreateStockRequest_ShouldReturnOk()
        {
            //Arrange
            _stockRequestManagerMock.Setup(i =>
            i.CreateStockRequest(It.IsAny<StockRequest>(), token)).Returns(_stockRequestModelRequest.ToEntity());

            //Act
            var result = _stockRequestController.CreateStockRequest(_stockRequestModelRequest);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            //Assert
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void ApproveStockRequest_ShouldReturnOk()
        {
            //Arrange
            var stockRequestId = 1;
            _stockRequestManagerMock.Setup(i =>
            i.ApproveStockRequest(It.IsAny<int>())).Returns(true);

            //Act
            var result = _stockRequestController.ApproveStockRequest(stockRequestId);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            //Assert
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void RejectStockRequest_ShouldReturnOk()
        {
            //Arrange
            var stockRequestId = 1;
            _stockRequestManagerMock.Setup(i =>
            i.RejectStockRequest(It.IsAny<int>())).Returns(true);

            //Act
            var result = _stockRequestController.RejectStockRequest(stockRequestId);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            //Assert
            Assert.AreEqual(200, statusCode);
        }


        [TestMethod]
        public void GetAll_ShouldReturnOk()
        {
            var searchCriteria = new StockRequestSearchCriteriaModelRequest();
            ICollection<StockRequest> list = new List<StockRequest>();
            list.Add(new StockRequest { Id = 1, RequestDate = DateTime.Now, Status = Domain.Enums.StockRequestStatus.Approved});

            //Arrange
            _stockRequestManagerMock.Setup(i =>
                i.GetStockRequestsByOwner(It.IsAny<string>()))
                .Returns(list);

            //Act
            var result = _stockRequestController.GetAll();
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            //Assert
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetStockRequest_WithInternalError_ShouldReturnException()
        {
            //Arrange
            var searchCriteria = new StockRequestSearchCriteriaModelRequest();

            //Act
            _stockRequestManagerMock.Setup(i =>
            i.GetStockRequestsByOwner(It.IsAny<string>()))
                .Throws(new Exception());

            _stockRequestController.GetAll();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetStockRequestByEmployee_WithInternalError_ShouldReturnException_By_Employee()
        {
            //Arrange
            var searchCriteria = new StockRequestSearchCriteriaModelRequest();

            //Act
            _stockRequestManagerMock.Setup(i =>
            i.GetStockRequestsByEmployee(It.IsAny<string>(), It.IsAny<StockRequestSearchCriteria>()))
                .Throws(new Exception());

            _stockRequestController.ByEmployee(searchCriteria);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void GetStockRequest_WithInvalidResourceException_ShouldReturnException()
        {
            //Arrange
            var searchCriteria = new StockRequestSearchCriteriaModelRequest();

            
            _stockRequestManagerMock.Setup(i =>
            i.GetStockRequestsByOwner(It.IsAny<string>()))
                .Throws(new InvalidResourceException("Invalid Employee."));

            _stockRequestController.GetAll();
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void GetStockRequestByEmployee_WithInvalidResourceException_ShouldReturnException_By_Employee()
        {
            //Arrange
            var searchCriteria = new StockRequestSearchCriteriaModelRequest();


            _stockRequestManagerMock.Setup(i =>
            i.GetStockRequestsByEmployee(It.IsAny<string>(), It.IsAny<StockRequestSearchCriteria>()))
                .Throws(new InvalidResourceException("Invalid Employee."));

            _stockRequestController.ByEmployee(searchCriteria);
        }


        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void GetStockRequest_WithResourceNotFoundException_ShouldReturnNotFound()
        {
            //Arrange
            var searchCriteria = new StockRequestSearchCriteriaModelRequest();

            _stockRequestManagerMock.Setup(i =>
            i.GetStockRequestsByOwner(It.IsAny<string>()))
                .Throws(new ResourceNotFoundException("Resource not found."));

            //Act
            var result = _stockRequestController.GetAll();
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void GetStockRequestByEmployee_WithResourceNotFoundException_ShouldReturnNotFound_By_Employee()
        {
            //Arrange
            var searchCriteria = new StockRequestSearchCriteriaModelRequest();

            _stockRequestManagerMock.Setup(i =>
            i.GetStockRequestsByEmployee(It.IsAny<string>(), It.IsAny<StockRequestSearchCriteria>()))
                .Throws(new ResourceNotFoundException("Resource not found."));

            //Act
            var result = _stockRequestController.ByEmployee(searchCriteria);
        }

        [TestMethod]
        public void GetStockRequestByEmployee_Ok()
        {
            //Arrange
            var searchCriteria = new StockRequestSearchCriteriaModelRequest();
            IEnumerable<StockRequest> stockRequests = new List<StockRequest> { new StockRequest { Id = 1,
            Status = Domain.Enums.StockRequestStatus.Pending, RequestDate = DateTime.Now }};
            _stockRequestManagerMock.Setup(i =>
            i.GetStockRequestsByEmployee(It.IsAny<string>(), It.IsAny<StockRequestSearchCriteria>())).Returns(stockRequests);

            //Act
            var result = _stockRequestController.ByEmployee(searchCriteria);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            //Assert
            Assert.AreEqual(200, statusCode);
        }


        [TestMethod]
        public void Test_StockRequestSearchCriteria()
        {

            var searchCriteria = new StockRequestSearchCriteria();
            searchCriteria.FromDate = DateTime.Now;
            searchCriteria.ToDate = DateTime.Now;
            var res1 = searchCriteria.Criteria();
            searchCriteria.FromDate = null;
            searchCriteria.ToDate = null;
            searchCriteria.Code = "123456";
            var res2 = searchCriteria.Criteria();
            searchCriteria.FromDate = null;
            searchCriteria.ToDate = null;
            searchCriteria.Code = "";
            searchCriteria.Status = "approved";
            var res3 = searchCriteria.Criteria();
            searchCriteria.FromDate = null;
            searchCriteria.ToDate = null;
            searchCriteria.Code = "";
            searchCriteria.Status = "";
            var res4 = searchCriteria.Criteria();

            // Assert
            Assert.IsNotNull(res1);
            Assert.IsNotNull(res2);
            Assert.IsNotNull(res3);
            Assert.IsNotNull(res4);
        }

    }
}

