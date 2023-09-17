using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using PharmaGo.DataAccess;
using PharmaGo.DataAccess.Repositories;
using PharmaGo.Domain.Entities;

namespace PharmaGo.Test.DataAccess.Test
{
	[TestClass]
	public class StockRequestRepositoryTest
	{
        private StockRequest _stockRequest;
        private PharmacyGoDbContext _context;
        private DbContextOptions<PharmacyGoDbContext> _options;
        private StockRequestRepository _stockRequestRepository;

        [TestInitialize]
        public void Setup()
        {
            this._options = new DbContextOptionsBuilder<PharmacyGoDbContext>()
                .UseInMemoryDatabase(databaseName: "PharmaGo").Options;
            this._context = new PharmacyGoDbContext(this._options);

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
        }

        [TestCleanup]
        public void CleanUp()
        {
            this._context.Database.EnsureDeleted();
        }

        [TestMethod]
        public void InsertStockRequest_ShouldAddEntryInDataBase()
        {
            //Arrange
            _stockRequestRepository = new StockRequestRepository(this._context);

            //Act
            _stockRequestRepository.InsertOne(_stockRequest);
            _stockRequestRepository.Save();

            //Assert
            var invitationsQuantity = _stockRequestRepository.CountAsync();
            Assert.AreEqual(1, invitationsQuantity);
        }

        [TestMethod]
        public void UpdateStockRequest_ShouldUpdateEntryInDataBase()
        {
            //Arrange
            var stockRequestStatus = _stockRequest.Status;
            _stockRequestRepository = new StockRequestRepository(this._context);
            //Act
            _stockRequestRepository.InsertOne(_stockRequest);
            _stockRequestRepository.Save();

            _stockRequest.Status = Domain.Enums.StockRequestStatus.Approved;
            _stockRequestRepository.UpdateOne(_stockRequest);
            _stockRequestRepository.Save();

            //Assert
            Assert.AreNotEqual(_stockRequest.Status, stockRequestStatus);
        }

        [TestMethod]
        public void DeleteStockRequest_ShouldDeleteEntryInDataBase()
        {
            //Arrange
            _stockRequestRepository = new StockRequestRepository(this._context);
            //Act
            _stockRequestRepository.InsertOne(_stockRequest);
            _stockRequestRepository.Save();

            _stockRequestRepository.DeleteOne(_stockRequest);
            _stockRequestRepository.Save();

            //Asert
            var stockRequestQuantity = _stockRequestRepository.CountAsync();
            Assert.AreEqual(0, stockRequestQuantity);
        }

        [TestMethod]
        public void GetOneByExpression_ShouldReturnOnStockRequest()
        {
            //Arrange
            _stockRequestRepository = new StockRequestRepository(this._context);
            //Act
            _stockRequestRepository.InsertOne(_stockRequest);
            _stockRequestRepository.Save();

            var stockRequestEntity = _stockRequestRepository
                .GetOneByExpression(p => p.Id == _stockRequest.Id);

            //Asert
            Assert.IsNotNull(stockRequestEntity);
        }

        [TestMethod]
        public void GetAllByExpression_ShouldReturnListOnStockRequest()
        {
            //Arrange
            _stockRequestRepository = new StockRequestRepository(this._context);
            //Act
            _stockRequestRepository.InsertOne(_stockRequest);
            _stockRequestRepository.Save();

            var stockRequestEntities = _stockRequestRepository
                .GetAllByExpression(p => p.Status == Domain.Enums.StockRequestStatus.Pending);

            //Asert
            Assert.AreEqual(1, stockRequestEntities.Count());
        }

        [TestMethod]
        public void GetAllBasicByExpression_ShouldReturnListOnStockRequest()
        {
            //Arrange
            _stockRequestRepository = new StockRequestRepository(this._context);
            //Act
            _stockRequestRepository.InsertOne(_stockRequest);
            _stockRequestRepository.Save();

            var stockRequestEntities = _stockRequestRepository
                .GetAllBasicByExpression(p => p.Status == Domain.Enums.StockRequestStatus.Pending);

            //Asert
            Assert.AreEqual(1, stockRequestEntities.Count());
        }
    }
}

