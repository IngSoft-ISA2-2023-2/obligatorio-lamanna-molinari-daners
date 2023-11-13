using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;
using PharmaGo.Exceptions;
using PharmaGo.IBusinessLogic;
using PharmaGo.WebApi.Controllers;
using PharmaGo.WebApi.Models.In;
using PharmaGo.WebApi.Models.Out;

namespace PharmaGo.Test.WebApi.Test
{
    [TestClass]
    public class DrugControllerTests
    {
        private DrugController _drugController;
        private Mock<IDrugManager> _drugManagerMock;
        private readonly string resourceNotFoundExceptionMessage = "Resource not found.";
        private readonly string invalidResourceExceptionMessage = "Invalid resource.";
        private readonly int drugId = 1;
        private DrugModel drugModel;
        private DrugBasicModel drugBasicModel;
        private Drug drug;
        private Pharmacy pharmacy;
        private DrugSearchCriteria drugSearch;
        private UpdateDrugModel updatedrugModel;
        private string token = "c80da9ed-1b41-4768-8e34-b728cae25d2f";

        [TestInitialize]
        public void SetUp()
        {

            _drugManagerMock = new Mock<IDrugManager>(MockBehavior.Strict);
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = token;
            _drugController = new DrugController(_drugManagerMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };

            pharmacy = new Pharmacy() { Id = 1, Name = "pharmacy", Address = "address", Users = new List<User>() };
            drugModel = new DrugModel()
            {
                Code = "drugCode",
                Name = "drugName",
                Prescription = false,
                Price = 500,
                Quantity = 10,
                Symptom = "headache",
                PharmacyName = pharmacy.Name
            };
            drug = new Drug()
            {
                Id = 1,
                Code = "drugCode",
                Name = "drugName",
                Symptom = "headache",
                Quantity = 10,
                Price = 500,
                Stock = 0,
                Prescription = false,
                Deleted = false,
                UnitMeasure = new UnitMeasure()
                {
                    Id = 1,
                    Name = "g",
                    Deleted = false
                },
                Presentation = new Presentation()
                {
                    Id = 1,
                    Name = "capsules",
                    Deleted = false
                },
                Pharmacy = pharmacy
            };
            drugBasicModel = new DrugBasicModel(drug);
            drugSearch = new DrugSearchCriteria { PharmacyId = pharmacy.Id, Name = drug.Name };
            updatedrugModel = new UpdateDrugModel
            {
                Code = "upCode",
                Name = "upName",
                Prescription = true,
                Price = 200,
                Quantity = 500,
                Symptom = "upSymptom"
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            _drugManagerMock.VerifyAll();
        }

        [TestMethod]
        public void GetDrugsOk()
        {
            _drugManagerMock.Setup(x => x.GetAll(drugSearch)).Returns(GenerateDrugList());
            var result = _drugController.GetAll(drugSearch);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _drugManagerMock.VerifyAll();
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void GetDrugsFailResourceNotFoundException()
        {
            _drugManagerMock.Setup(x => x.GetAll(drugSearch)).Throws(new ResourceNotFoundException(resourceNotFoundExceptionMessage));
            var result = _drugController.GetAll(drugSearch);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _drugManagerMock.VerifyAll();
            Assert.AreEqual(404, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void GetDrugsFailInvalidResourceException()
        {
            _drugManagerMock.Setup(x => x.GetAll(drugSearch)).Throws(new InvalidResourceException(invalidResourceExceptionMessage));
            var result = _drugController.GetAll(drugSearch);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _drugManagerMock.VerifyAll();
            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public void GetDrugById()
        {
            _drugManagerMock.Setup(x => x.GetById(drugId)).Returns(drug);
            var result = _drugController.GetById(drugId);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _drugManagerMock.VerifyAll();
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        public void PostDrugOk()
        {
            _drugManagerMock.Setup(x => x.Create(It.IsAny<Drug>(), token)).Returns(drug);
            var result = _drugController.Create(drugModel);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            _drugManagerMock.VerifyAll();
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void PostDrugFailInvalidResourceException()
        {
            _drugManagerMock.Setup(x => x.Create(It.IsAny<Drug>(), token)).Throws(new InvalidResourceException(invalidResourceExceptionMessage));
            
            var result = _drugController.Create(drugModel);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _drugManagerMock.VerifyAll();
            
            // Assert
            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void PostDrugFailResourceNotFoundException()
        {
            _drugManagerMock.Setup(x => x.Create(It.IsAny<Drug>(), token)).Throws(new ResourceNotFoundException(resourceNotFoundExceptionMessage));
            var result = _drugController.Create(drugModel);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _drugManagerMock.VerifyAll();
            Assert.AreEqual(404, statusCode);
        }

        [TestMethod]
        public void PutDrugOk()
        {
            drug.Name = "newName";
            _drugManagerMock.Setup(x => x.Update(drug.Id, It.IsAny<Drug>())).Returns(drug);
            var result = _drugController.Update(drug.Id, updatedrugModel);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            _drugManagerMock.VerifyAll();
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void PutDrugFailInvalidResourceException()
        {
            _drugManagerMock.Setup(x => x.Update(drug.Id, It.IsAny<Drug>())).Throws(new InvalidResourceException(invalidResourceExceptionMessage));
            var result = _drugController.Update(drug.Id, updatedrugModel);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _drugManagerMock.VerifyAll();
            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void PutDrugFailResourceNotFoundException()
        {
            _drugManagerMock.Setup(x => x.Update(drug.Id, It.IsAny<Drug>())).Throws(new ResourceNotFoundException(resourceNotFoundExceptionMessage));
            var result = _drugController.Update(drug.Id, updatedrugModel);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _drugManagerMock.VerifyAll();
            Assert.AreEqual(404, statusCode);
        }

        [TestMethod]
        public void DeleteOk()
        {
            _drugManagerMock.Setup(x => x.Delete(It.IsAny<int>()));
            var result = _drugController.Delete(It.IsAny<int>());

            // Assert
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            var value = objectResult.Value as Object;

            Assert.AreEqual(true, value);
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void DeleteDrugFailInvalidResourceException()
        {
            _drugManagerMock.Setup(x => x.Delete(drug.Id)).Throws(new InvalidResourceException(invalidResourceExceptionMessage));
            var result = _drugController.Delete(drug.Id);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _drugManagerMock.VerifyAll();
            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void DeleteDrugFailResourceNotFoundException()
        {
            _drugManagerMock.Setup(x => x.Delete(drug.Id)).Throws(new ResourceNotFoundException(resourceNotFoundExceptionMessage));
            var result = _drugController.Delete(drug.Id);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _drugManagerMock.VerifyAll();
            Assert.AreEqual(404, statusCode);
        }

        private IEnumerable<Drug> GenerateDrugList()
        {
            var drugList = new List<Drug>();
            for (int i = 1; i < 11; i++)
            {
                drugList.Add(new Drug()
                {
                    Id = i,
                    Code = $"drugCode{i}",
                    Name = $"drugName{i}",
                    Price = 100,
                    Prescription = false,
                    Deleted = false,
                    Stock = 0,
                    Symptom = "headache",
                    Quantity = i,
                    Presentation = new Presentation { Id = i, Name = "capsules", Deleted = false },
                    UnitMeasure = new UnitMeasure { Id = i, Name = "g", Deleted = false }
                    });
            }
            return drugList;
        }

        [TestMethod]
        public void Test_Drug_Basic_Detail_Model()
        {
            DrugDetailModel detailModel = new DrugDetailModel(drug);

            // Assert
            Assert.IsNotNull(drugBasicModel);
            Assert.AreEqual(drugBasicModel.Name, "drugName");
            Assert.AreEqual(drugBasicModel.Id, 1);
            Assert.AreEqual(drugBasicModel.Code, "drugCode");

            Assert.IsNotNull(detailModel);
            Assert.AreEqual(detailModel.Name, "drugName");
            Assert.AreEqual(drugBasicModel.Id, 1);
            Assert.AreEqual(detailModel.Code, "drugCode"); 
            Assert.AreEqual(detailModel.Quantity, 10);
            Assert.AreEqual(detailModel.Stock, 0);
            Assert.AreEqual(detailModel.Price, 500);
            Assert.AreEqual(detailModel.Symptom, "headache");
            Assert.AreEqual(detailModel.Prescription, false);
            Assert.AreEqual(detailModel.Pharmacy.Name, pharmacy.Name);
            Assert.AreEqual(detailModel.Pharmacy.Id, pharmacy.Id);            
        }

        [TestMethod]
        public void Test_DrugSearchCriteria()
        {

            var searchCriteria = new DrugSearchCriteria();
            searchCriteria.Name = "Drug1";
            searchCriteria.PharmacyId = 123456;
            var res1 = searchCriteria.Criteria(new Drug { Pharmacy = new Pharmacy() { Id = 123456 }, Name = "Drug1" });
            searchCriteria.Name = "";
            searchCriteria.PharmacyId = 123456;
            var res2 = searchCriteria.Criteria(new Drug { Pharmacy = new Pharmacy() { Id = 123456 }, Name = "" });
            searchCriteria.Name = "Drug1";
            searchCriteria.PharmacyId = null;
            var res3 = searchCriteria.Criteria(new Drug { Pharmacy = null, Name = "Drug1" });
            searchCriteria.Name = "";
            searchCriteria.PharmacyId = null;
            var res4 = searchCriteria.Criteria(new Drug { Pharmacy = null, Name = "" });

            // Assert
            Assert.IsNotNull(res1);
            Assert.IsNotNull(res2);
            Assert.IsNotNull(res3);
            Assert.IsNotNull(res4);
        }


        [TestMethod]
        public void Test_DrugModelResponse()
        {
            DrugModelResponse drugModel = new DrugModelResponse(drug);
            
            // Assert
            Assert.AreEqual(drugModel.Name, "drugName");
            Assert.AreEqual(drugModel.Code, "drugCode");
        }


        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void GetDrugsByEmployee_ShouldReturnNotFound()
        {
            //Arrange
            _drugManagerMock.Setup(i =>
            i.GetAllByUser(It.IsAny<string>()))
                .Throws(new ResourceNotFoundException("Resource not found."));

            //Act
            var result = _drugController.User();
        }

        [TestMethod]
        public void GetDrugsByEmployee_ShouldReturnOk()
        {
            //Arrange
            IEnumerable<Drug> drugs = new List<Drug> { new Drug { Id = 1, Code = "ABC", Deleted = false, 
                Name = "test", Price = 123, Prescription = false, Quantity = 10, Stock = 1000, Symptom = "test test", 
            Pharmacy = pharmacy, Presentation = drug.Presentation, UnitMeasure = drug.UnitMeasure} };
            _drugManagerMock.Setup(i =>
            i.GetAllByUser(It.IsAny<string>())).Returns(drugs);

            //Act
            var result = _drugController.User();
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;

            //Assert
            Assert.AreEqual(200, statusCode);
        }


    }
}