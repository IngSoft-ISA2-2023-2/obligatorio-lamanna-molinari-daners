using System.Linq.Expressions;
using Moq;
using PharmaGo.BusinessLogic;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;
using PharmaGo.Exceptions;
using PharmaGo.IDataAccess;

namespace PharmaGo.Test.BusinessLogic.Test
{
	[TestClass]
	public class StockManagerTest
	{
		private Mock<IRepository<StockRequest>> _stockRequestMock;
        private Mock<IRepository<User>> _employeeMock;
        private Mock<IRepository<Drug>> _drugMock;
        private Mock<IRepository<Session>> _sessionMock;
        private StockRequestManager _stockRequestManager;
        private StockRequest _stockRequest;
        private string token = "c80da9ed-1b41-4768-8e34-b728cae25d2f";
        private Session session = null;
        private User user = null;

        [TestInitialize]
		public void SetUp()
		{
            _stockRequestMock = new Mock<IRepository<StockRequest>>(MockBehavior.Strict);
            _employeeMock = new Mock<IRepository<User>>(MockBehavior.Strict);
            _drugMock = new Mock<IRepository<Drug>>(MockBehavior.Strict);
            _sessionMock = new Mock<IRepository<Session>>(MockBehavior.Strict);
            _stockRequestManager = new StockRequestManager(_stockRequestMock.Object,
            _employeeMock.Object, _drugMock.Object, _sessionMock.Object);

            _stockRequest = new StockRequest()
            {
                Id = 1,
                Employee = new User()
                {
                    Id = 1,
                    UserName = "jcastro",
                    Email = "jcastro@test.com.uy"
                },
                Details = new List<StockRequestDetail>()
                {
                  new StockRequestDetail() { Id = 1, Drug = new Drug(){ Id = 1, Code = "XF324"}, Quantity = 10 }
                },
                RequestDate = DateTime.Now,
                Status = Domain.Enums.StockRequestStatus.Pending
            };
            session = new Session { Id = 1, Token = new Guid(token), UserId = 1 };
            user = new User { Id = 1, Email = "fernando@gmail.com", Password = "Asdfer234.." };
        }

		[TestCleanup]
		public void CleanUp()
		{
			_stockRequestMock.VerifyAll();
		}

		[TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateStockRequest_WithNulEmployee_SouldReturnException()
		{
            //Arrange
            User user_ = null;
            _sessionMock.Setup(session => session.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>()))
                .Returns(session);
            _employeeMock.Setup(d => d.GetOneDetailByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(user_);
            
            var stockRequest = new StockRequest()
			{
				Id = 1,
				Status = Domain.Enums.StockRequestStatus.Pending,
				Details = new List<StockRequestDetail>()
				{
					new StockRequestDetail() { Id = 1, Drug = new Drug() { Id = 1, Code = "XF324" }, Quantity = 50 },
					new StockRequestDetail() { Id = 2, Drug = new Drug() { Id = 2, Code = "RS546" }, Quantity = 25 }
				},
				RequestDate = DateTime.Now
			};

			//Act
			_stockRequestManager.CreateStockRequest(stockRequest, token);
        }

		[TestMethod]
		[ExpectedException(typeof(InvalidResourceException))]
		public void CreateStockRequest_WithNullDetails_ShouldReturnException()
		{

            //Arrange
            var stockRequest = new StockRequest()
            {
                Id = 1,
                Status = Domain.Enums.StockRequestStatus.Pending,
				Employee = new User() { Id = 1, UserName = "jcastro" },
                RequestDate = DateTime.Now
            };

            //Act
            _stockRequestManager.CreateStockRequest(stockRequest, token);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateStockRequest_WithCeroDetails_ShouldReturnException()
        {
            //Arrange
            var stockRequest = new StockRequest()
            {
                Id = 1,
                Status = Domain.Enums.StockRequestStatus.Pending,
                Employee = new User() { Id = 1, UserName = "jcastro" },
                Details = new List<StockRequestDetail>() { },
                RequestDate = DateTime.Now
            };

            //Act
            _stockRequestManager.CreateStockRequest(stockRequest, token);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateStockRequest_WithInvalidEmployee_ShouldReturnException()
        {
            //Arrange
            User user = null;
            var stockRequest = new StockRequest()
            {
                Id = 1,
                Status = Domain.Enums.StockRequestStatus.Pending,
                Employee = new User() { Id = 1, UserName = "jcastro" },
                Details = new List<StockRequestDetail>() { },
                RequestDate = DateTime.Now
            };

            //Act
            _employeeMock.Setup(u => u.GetOneByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);
            _stockRequestManager.CreateStockRequest(stockRequest, token);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void RejectStockRequest_WithInvalidStockRequest_ShouldReturnException()
        {
            //Arrange
            StockRequest stockRequest = null;
            var stockRequestId = 1;
            _stockRequestMock.Setup(m => m.GetOneByExpression(It.IsAny<Expression<Func<StockRequest, bool>>>())).Returns(stockRequest);

            //Act
            var result = _stockRequestManager.RejectStockRequest(stockRequestId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void RejectStockRequest_WithApprovedStatus_ShouldReturnException()
        {
            //Arrange
            var stockRequest = new StockRequest()
            {
                Id = 1,
                Status = Domain.Enums.StockRequestStatus.Approved,
                Employee = new User() { Id = 1, UserName = "jcastro" },
                Details = new List<StockRequestDetail>()
                {
                    new StockRequestDetail() { Id = 1, Drug = new Drug() { Id = 1, Code = "XF324" }, Quantity = 50 },
                    new StockRequestDetail() { Id = 2, Drug = new Drug() { Id = 2, Code = "RS546" }, Quantity = 25 }
                },
                RequestDate = DateTime.Now
            };
            _stockRequestMock.Setup(m => m.GetOneByExpression(It.IsAny<Expression<Func<StockRequest, bool>>>())).Returns(stockRequest);

            //Act
            var result = _stockRequestManager.RejectStockRequest(stockRequest.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void RejectStockRequest_WithRejectedStatus_ShouldReturnException()
        {
            //Arrange
            var stockRequest = new StockRequest()
            {
                Id = 1,
                Status = Domain.Enums.StockRequestStatus.Rejected,
                Employee = new User() { Id = 1, UserName = "jcastro" },
                Details = new List<StockRequestDetail>()
                {
                    new StockRequestDetail() { Id = 1, Drug = new Drug() { Id = 1, Code = "XF324" }, Quantity = 50 },
                    new StockRequestDetail() { Id = 2, Drug = new Drug() { Id = 2, Code = "RS546" }, Quantity = 25 }
                },
                RequestDate = DateTime.Now
            };
            _stockRequestMock.Setup(m => m.GetOneByExpression(It.IsAny<Expression<Func<StockRequest, bool>>>())).Returns(stockRequest);

            //Act
            var result = _stockRequestManager.RejectStockRequest(stockRequest.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void ApproveStockRequest_WithInvalidStockRequest_ShouldReturnException()
        {
            //Arrange
            StockRequest stockRequest = null;
            var stockRequestId = 1;
            _stockRequestMock.Setup(m => m.GetOneByExpression(It.IsAny<Expression<Func<StockRequest, bool>>>())).Returns(stockRequest);

            //Act
            var result = _stockRequestManager.ApproveStockRequest(stockRequestId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void ApproveStockRequest_WithApprovedStatus_ShouldReturnException()
        {
            //Arrange
            var stockRequest = new StockRequest()
            {
                Id = 1,
                Status = Domain.Enums.StockRequestStatus.Approved,
                Employee = new User() { Id = 1, UserName = "jcastro" },
                Details = new List<StockRequestDetail>()
                {
                    new StockRequestDetail() { Id = 1, Drug = new Drug() { Id = 1, Code = "XF324" }, Quantity = 50 },
                    new StockRequestDetail() { Id = 2, Drug = new Drug() { Id = 2, Code = "RS546" }, Quantity = 25 }
                },
                RequestDate = DateTime.Now
            };
            _stockRequestMock.Setup(m => m.GetOneByExpression(It.IsAny<Expression<Func<StockRequest, bool>>>())).Returns(stockRequest);

            //Act
            var result = _stockRequestManager.RejectStockRequest(stockRequest.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void ApproveStockRequest_WithRejectedStatus_ShouldReturnException()
        {
            //Arrange
            var stockRequest = new StockRequest()
            {
                Id = 1,
                Status = Domain.Enums.StockRequestStatus.Rejected,
                Employee = new User() { Id = 1, UserName = "jcastro" },
                Details = new List<StockRequestDetail>()
                {
                    new StockRequestDetail() { Id = 1, Drug = new Drug() { Id = 1, Code = "XF324" }, Quantity = 50 },
                    new StockRequestDetail() { Id = 2, Drug = new Drug() { Id = 2, Code = "RS546" }, Quantity = 25 }
                },
                RequestDate = DateTime.Now
            };
            _stockRequestMock.Setup(m => m.GetOneByExpression(It.IsAny<Expression<Func<StockRequest, bool>>>())).Returns(stockRequest);

            //Act
            var result = _stockRequestManager.RejectStockRequest(stockRequest.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void ApproveStockRequest_WithDeletedDrugs_ShouldReturnException()
        {
            //Arrange
            var stockRequest = new StockRequest()
            {
                Id = 1,
                Status = Domain.Enums.StockRequestStatus.Rejected,
                Employee = new User() { Id = 1, UserName = "jcastro" },
                Details = new List<StockRequestDetail>()
                {
                    new StockRequestDetail() { Id = 1, Drug = new Drug() { Id = 1, Code = "XF324" }, Quantity = 50 },
                    new StockRequestDetail() { Id = 2, Drug = new Drug() { Id = 2, Code = "RS546", Deleted = true }, Quantity = 25 }
                },
                RequestDate = DateTime.Now
            };

            _stockRequestMock.Setup(m => m.GetOneByExpression(It.IsAny<Expression<Func<StockRequest, bool>>>())).Returns(stockRequest);

            //Act
            var result = _stockRequestManager.RejectStockRequest(stockRequest.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateStockRequest_WithInvalidDrugs_ShouldReturnException()
        {
            //Arrange
            Drug drug = null;
            User employee = new User() { Id = 1, UserName = "jcastro" };
            var stockRequest = new StockRequest()
            {
                Id = 1,
                Status = Domain.Enums.StockRequestStatus.Rejected,
                Employee = new User() { Id = 1, UserName = "jcastro" },
                Details = new List<StockRequestDetail>()
                {
                    new StockRequestDetail() { Id = 1, Drug = new Drug() { Id = 1, Code = "XF324" }, Quantity = 50 },
                    new StockRequestDetail() { Id = 2, Drug = new Drug() { Id = 2, Code = "RS546" }, Quantity = 25 }
                },
                RequestDate = DateTime.Now
            };

            _employeeMock.Setup(u => u.GetOneDetailByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(employee);
            _drugMock.Setup(d => d.GetOneByExpression(It.IsAny<Expression<Func<Drug, bool>>>())).Returns(drug);
            _sessionMock.Setup(d => d.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>())).Returns(session);

            //Act
            var result = _stockRequestManager.CreateStockRequest(stockRequest, token);
        }

        [TestMethod]
        public void CreateStockRequest_ShouldCreateStockRequest()
        {
            //Arrange
            var drug = new Drug() { Id = 1, Code = "XF324" };
            User emplotyee = new User() { Id = 1, UserName = "jcastro" };
            var stockRequest = new StockRequest()
            {
                Id = 1,
                Status = Domain.Enums.StockRequestStatus.Pending,
                Employee = new User() { Id = 1, UserName = "jcastro" },
                Details = new List<StockRequestDetail>()
                {
                    new StockRequestDetail() { Id = 1, Drug = new Drug() { Id = 1, Code = "XF324" }, Quantity = 50 }
                },
                RequestDate = DateTime.Now
            };

            //Act
            _employeeMock.Setup(u => u.GetOneByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(emplotyee);
            _drugMock.Setup(d => d.GetOneByExpression(It.IsAny<Expression<Func<Drug, bool>>>())).Returns(drug);
            _sessionMock.Setup(d => d.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>())).Returns(session);
            _employeeMock.Setup(d => d.GetOneDetailByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);

            _stockRequestMock.Setup(s => s.InsertOne(It.IsAny<StockRequest>()));
            _stockRequestMock.Setup(s => s.Save());

            var result = _stockRequestManager.CreateStockRequest(stockRequest, token);

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void GetStockByEmployee_WithNullEmployee_ShouldReturnException()
        {
            //Arrange
            var token = "";
            var searchCriteria = new StockRequestSearchCriteria();

            //Act
            _stockRequestManager.GetStockRequestsByEmployee(token, searchCriteria);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void GetStockByEmployee_WithInvalidEmployee_ShouldReturnException()
        {
            //Arrange
            var token = Guid.NewGuid().ToString();
            var searchCriteria = new StockRequestSearchCriteria();
            _sessionMock.Setup(session => session.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>()))
                .Returns((Session)null);
            //Act
            _stockRequestManager.GetStockRequestsByEmployee(token, searchCriteria);
        }

        [TestMethod]
        public void GetStockByEmployee_WithNullFilters_ShouldReturnIEnumerableStockRequest()
        {
            //Arrange
            var token = Guid.NewGuid().ToString();
            var session = new Session() { Id = 1, Token = new Guid(token), UserId = 1 };
            var searchCriteria = new StockRequestSearchCriteria() { EmployeeId = session.UserId };
            var stockRequestList = new List<StockRequest>()
            {
                    new StockRequest() {
                        Id = 1,
                        RequestDate = new DateTime(2022, 09, 20),
                        Status = Domain.Enums.StockRequestStatus.Pending,
                        Employee = new User()
                        {
                            Id = 1,
                            UserName = "jcastro"
                        },
                        Details = new List<StockRequestDetail>()
                        {
                            new StockRequestDetail() { Id = 1, Drug = new Drug() { Id = 1, Code = "XF324" } }
                        }
                    },
                    new StockRequest()
                    {
                        Id = 2,
                        RequestDate = new DateTime(2022, 09, 25),
                        Status = Domain.Enums.StockRequestStatus.Pending,
                        Employee = new User()
                        {
                            Id = 1,
                            UserName = "jcastro"
                        },
                        Details = new List<StockRequestDetail>()
                        {
                            new StockRequestDetail() { Id = 1, Drug = new Drug() { Id = 1, Code = "XF324" }, Quantity = 3 },
                            new StockRequestDetail() { Id = 2, Drug = new Drug() { Id = 2, Code = "RS546" }, Quantity = 2 }
                        }
                    }
             };
            
            _sessionMock.Setup(session => session.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>()))
                .Returns(session);

            _stockRequestMock.Setup(stock => stock.GetAllBasicByExpression(It.IsAny<Expression<Func<StockRequest, bool>>>()))
                .Returns(stockRequestList);

            //Act
            var result = _stockRequestManager.GetStockRequestsByEmployee(token, searchCriteria);

            //Asert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetStockByEmployee_WithAllFilters_ShouldReturnIEnumerableStockRequest()
        {
            //Arrange
            var token = Guid.NewGuid().ToString();
            var session = new Session() { Id = 1, Token = new Guid(token), UserId = 1 };
            var searchCriteria = new StockRequestSearchCriteria() { 
                EmployeeId = session.UserId, 
                Code = "XF324", 
                Status = "Pending", 
                FromDate = new DateTime(2022, 09, 10), 
                ToDate = new DateTime(2022, 10, 20) };

            var stockRequestList = new List<StockRequest>()
            {
                    new StockRequest() {
                        Id = 1,
                        RequestDate = new DateTime(2022, 09, 20),
                        Status = Domain.Enums.StockRequestStatus.Pending,
                        Employee = new User()
                        {
                            Id = 1,
                            UserName = "jcastro"
                        },
                        Details = new List<StockRequestDetail>()
                        {
                            new StockRequestDetail() { Id = 1, Drug = new Drug() { Id = 1, Code = "XF324" } }
                        }
                    },
                    new StockRequest()
                    {
                        Id = 2,
                        RequestDate = new DateTime(2022, 09, 25),
                        Status = Domain.Enums.StockRequestStatus.Pending,
                        Employee = new User()
                        {
                            Id = 1,
                            UserName = "jcastro"
                        },
                        Details = new List<StockRequestDetail>()
                        {
                            new StockRequestDetail() { Id = 1, Drug = new Drug() { Id = 1, Code = "XF324" }, Quantity = 3 },
                            new StockRequestDetail() { Id = 2, Drug = new Drug() { Id = 2, Code = "RS546" }, Quantity = 2 }
                        }
                    }
             };

            _sessionMock.Setup(session => session.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>()))
                .Returns(session);

            _stockRequestMock.Setup(stock => stock.GetAllBasicByExpression(It.IsAny<Expression<Func<StockRequest, bool>>>()))
                .Returns(stockRequestList);

            //Act
            var result = _stockRequestManager.GetStockRequestsByEmployee(token, searchCriteria);

            //Asert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetStockByEmployee_WithDateAndCode_ShouldReturnIEnumerableStockRequest()
        {
            //Arrange
            var token = Guid.NewGuid().ToString();
            var session = new Session() { Id = 1, Token = new Guid(token), UserId = 1 };
            var searchCriteria = new StockRequestSearchCriteria()
            {
                EmployeeId = session.UserId,
                Code = "XF324",
                Status = null,
                FromDate = new DateTime(2022, 09, 10),
                ToDate = new DateTime(2022, 10, 20)
            };

            var stockRequestList = new List<StockRequest>()
            {
                    new StockRequest() {
                        Id = 1,
                        RequestDate = new DateTime(2022, 09, 20),
                        Status = Domain.Enums.StockRequestStatus.Pending,
                        Employee = new User()
                        {
                            Id = 1,
                            UserName = "jcastro"
                        },
                        Details = new List<StockRequestDetail>()
                        {
                            new StockRequestDetail() { Id = 1, Drug = new Drug() { Id = 1, Code = "XF324" } }
                        }
                    },
                    new StockRequest()
                    {
                        Id = 2,
                        RequestDate = new DateTime(2022, 09, 25),
                        Status = Domain.Enums.StockRequestStatus.Pending,
                        Employee = new User()
                        {
                            Id = 1,
                            UserName = "jcastro"
                        },
                        Details = new List<StockRequestDetail>()
                        {
                            new StockRequestDetail() { Id = 1, Drug = new Drug() { Id = 1, Code = "XF324" }, Quantity = 3 },
                            new StockRequestDetail() { Id = 2, Drug = new Drug() { Id = 2, Code = "RS546" }, Quantity = 2 }
                        }
                    }
             };

            _sessionMock.Setup(session => session.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>()))
                .Returns(session);

            _stockRequestMock.Setup(stock => stock.GetAllBasicByExpression(It.IsAny<Expression<Func<StockRequest, bool>>>()))
                .Returns(stockRequestList);

            //Act
            var result = _stockRequestManager.GetStockRequestsByEmployee(token, searchCriteria);

            //Asert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetStockByEmployee_WithDateAndStatus_ShouldReturnIEnumerableStockRequest()
        {
            //Arrange
            var token = Guid.NewGuid().ToString();
            var session = new Session() { Id = 1, Token = new Guid(token), UserId = 1 };
            var searchCriteria = new StockRequestSearchCriteria()
            {
                EmployeeId = session.UserId,
                Code = null,
                Status = "Pending",
                FromDate = new DateTime(2022, 09, 10),
                ToDate = new DateTime(2022, 10, 20)
            };

            var stockRequestList = new List<StockRequest>()
            {
                    new StockRequest() {
                        Id = 1,
                        RequestDate = new DateTime(2022, 09, 20),
                        Status = Domain.Enums.StockRequestStatus.Pending,
                        Employee = new User()
                        {
                            Id = 1,
                            UserName = "jcastro"
                        },
                        Details = new List<StockRequestDetail>()
                        {
                            new StockRequestDetail() { Id = 1, Drug = new Drug() { Id = 1, Code = "XF324" } }
                        }
                    },
                    new StockRequest()
                    {
                        Id = 2,
                        RequestDate = new DateTime(2022, 09, 25),
                        Status = Domain.Enums.StockRequestStatus.Pending,
                        Employee = new User()
                        {
                            Id = 1,
                            UserName = "jcastro"
                        },
                        Details = new List<StockRequestDetail>()
                        {
                            new StockRequestDetail() { Id = 1, Drug = new Drug() { Id = 1, Code = "XF324" }, Quantity = 3 },
                            new StockRequestDetail() { Id = 2, Drug = new Drug() { Id = 2, Code = "RS546" }, Quantity = 2 }
                        }
                    }
             };

            _sessionMock.Setup(session => session.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>()))
                .Returns(session);

            _stockRequestMock.Setup(stock => stock.GetAllBasicByExpression(It.IsAny<Expression<Func<StockRequest, bool>>>()))
                .Returns(stockRequestList);

            //Act
            var result = _stockRequestManager.GetStockRequestsByEmployee(token, searchCriteria);

            //Asert
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetStockByEmployee_WithCodeAndStatus_ShouldReturnIEnumerableStockRequest()
        {
            //Arrange
            var token = Guid.NewGuid().ToString();
            var session = new Session() { Id = 1, Token = new Guid(token), UserId = 1 };
            var searchCriteria = new StockRequestSearchCriteria()
            {
                EmployeeId = session.UserId,
                Code = "XF324",
                Status = "Pending",
                FromDate = null,
                ToDate = null
            };

            var stockRequestList = new List<StockRequest>()
            {
                    new StockRequest() {
                        Id = 1,
                        RequestDate = new DateTime(2022, 09, 20),
                        Status = Domain.Enums.StockRequestStatus.Pending,
                        Employee = new User()
                        {
                            Id = 1,
                            UserName = "jcastro"
                        },
                        Details = new List<StockRequestDetail>()
                        {
                            new StockRequestDetail() { Id = 1, Drug = new Drug() { Id = 1, Code = "XF324" } }
                        }
                    },
                    new StockRequest()
                    {
                        Id = 2,
                        RequestDate = new DateTime(2022, 09, 25),
                        Status = Domain.Enums.StockRequestStatus.Pending,
                        Employee = new User()
                        {
                            Id = 1,
                            UserName = "jcastro"
                        },
                        Details = new List<StockRequestDetail>()
                        {
                            new StockRequestDetail() { Id = 1, Drug = new Drug() { Id = 1, Code = "XF324" }, Quantity = 3 },
                            new StockRequestDetail() { Id = 2, Drug = new Drug() { Id = 2, Code = "RS546" }, Quantity = 2 }
                        }
                    }
             };

            _sessionMock.Setup(session => session.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>()))
                .Returns(session);

            _stockRequestMock.Setup(stock => stock.GetAllBasicByExpression(It.IsAny<Expression<Func<StockRequest, bool>>>()))
                .Returns(stockRequestList);

            //Act
            var result = _stockRequestManager.GetStockRequestsByEmployee(token, searchCriteria);

            //Asert
            Assert.AreEqual(2, result.Count());
        }



        [TestMethod]
        public void ApproveStockRequest_ShouldUpdateDrugStock()
        {
            //Arrange
            var stockRequest = new StockRequest()
            {
                Id = 1,
                Status = Domain.Enums.StockRequestStatus.Pending,
                Employee = new User() { Id = 1, UserName = "jcastro" },
                Details = new List<StockRequestDetail>()
                {
                    new StockRequestDetail() { Id = 1, Drug = new Drug() { Id = 1, Code = "XF324" }, Quantity = 50 },
                    new StockRequestDetail() { Id = 2, Drug = new Drug() { Id = 2, Code = "RS546" }, Quantity = 25 }
                },
                RequestDate = DateTime.Now
            };

            _stockRequestMock.Setup(s => s.GetOneByExpression(It.IsAny<Expression<Func<StockRequest, bool>>>()))
                .Returns(stockRequest);

            _drugMock.Setup(d => d.GetOneByExpression(It.IsAny<Expression<Func<Drug, bool>>>()))
                .Returns(new Drug() { Id = 1, Code = "XF324" , Quantity = 50 });
            _drugMock.Setup(d => d.UpdateOne(It.IsAny<Drug>()));

            _stockRequestMock.Setup(s => s.UpdateOne(It.IsAny<StockRequest>()));

            _drugMock.Setup(d => d.Save());
            _stockRequestMock.Setup(s => s.Save());

            //Act
            var result = _stockRequestManager.ApproveStockRequest(stockRequest.Id);
        }

        // ------

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void GetStock_WithNullEmployee_ShouldReturnException_By_Owner()
        {
            //Arrange
            var token = "";

            //Act
            _stockRequestManager.GetStockRequestsByOwner(token);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void GetStock_WithInvalidEmployee_ShouldReturnException_By_Owner()
        {
            //Arrange
            var token = Guid.NewGuid().ToString();
            var searchCriteria = new StockRequestSearchCriteria();
            _sessionMock.Setup(session => session.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>()))
                .Returns((Session)null);

            //Act
            _stockRequestManager.GetStockRequestsByOwner(token);

        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void GetStock_WithInvalidUser_ShouldReturnException_By_Owner()
        {
            //Arrange
            var token = Guid.NewGuid().ToString();
            var searchCriteria = new StockRequestSearchCriteria();
            _sessionMock.Setup(session => session.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>()))
                .Returns((Session)null);

            _employeeMock.Setup(e => e.GetOneDetailByExpression(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns((User)null);

            //Act
            _stockRequestManager.GetStockRequestsByOwner(token);
        }


        [TestMethod]
        public void GetStock_WithNullFilters_ShouldReturnIEnumerableStockRequest_By_Owner()
        {
            //Arrange
            var token = Guid.NewGuid().ToString();
            var session = new Session() { Id = 1, Token = new Guid(token), UserId = 1 };
            var searchCriteria = new StockRequestSearchCriteria() { EmployeeId = session.UserId };
            var stockRequestList = new List<StockRequest>()
            {
                    new StockRequest() {
                        Id = 1,
                        RequestDate = new DateTime(2022, 09, 20),
                        Status = Domain.Enums.StockRequestStatus.Pending,
                        Employee = new User()
                        {
                            Id = 1,
                            UserName = "jcastro"
                        },
                        Details = new List<StockRequestDetail>()
                        {
                            new StockRequestDetail() { Id = 1, Drug = new Drug() { Id = 1, Code = "XF324" } }
                        }
                    },
                    new StockRequest()
                    {
                        Id = 2,
                        RequestDate = new DateTime(2022, 09, 25),
                        Status = Domain.Enums.StockRequestStatus.Pending,
                        Employee = new User()
                        {
                            Id = 1,
                            UserName = "jcastro"
                        },
                        Details = new List<StockRequestDetail>()
                        {
                            new StockRequestDetail() { Id = 1, Drug = new Drug() { Id = 1, Code = "XF324" }, Quantity = 3 },
                            new StockRequestDetail() { Id = 2, Drug = new Drug() { Id = 2, Code = "RS546" }, Quantity = 2 }
                        }
                    }
             };

            _sessionMock.Setup(session => session.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>()))
                .Returns(session);

            _employeeMock.Setup(e => e.GetOneDetailByExpression(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(It.IsAny<User>);

            _stockRequestMock.Setup(stock => stock.GetAllBasicByExpression(It.IsAny<Expression<Func<StockRequest, bool>>>()))
                .Returns(stockRequestList);


            //Act
            var result = _stockRequestManager.GetStockRequestsByEmployee(token, searchCriteria);

            //Asert
            Assert.AreEqual(2, result.Count());
        }

    }
}

