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
    public class PharmacyControllerTests
    {
        private PharmacyController _pharmacyController;
        private Mock<IPharmacyManager> _pharmacyManagerMock;
        private readonly PharmacySearchCriteria pharmacySearchCriteria = new PharmacySearchCriteria();
        private readonly string resourceNotFoundExceptionMessage = "Resource not found.";
        private readonly string invalidResourceExceptionMessage = "Invalid resource.";
        private Pharmacy pharmacy;
        private PharmacyModel pharmacyModel;


        [TestInitialize]
        public void SetUp()
        {
            _pharmacyManagerMock = new Mock<IPharmacyManager>(MockBehavior.Strict);
            _pharmacyController = new PharmacyController(_pharmacyManagerMock.Object);
            pharmacy = new Pharmacy()
            {
                Id = 20,
                Name = "newPharmacy",
                Address = "address",
                Users = new List<User>(),
                Drugs = new List<Drug>()
            };
            pharmacyModel = new PharmacyModel { Name = pharmacy.Name, Address = pharmacy.Address};
    }

    [TestCleanup]
        public void Cleanup()
        {
            _pharmacyManagerMock.VerifyAll();
        }

        [TestMethod]
        public void GetPharmaciesOk()
        {
            _pharmacyManagerMock.Setup(x => x.GetAll(pharmacySearchCriteria)).Returns(GeneratePharmacyList());
            var result = _pharmacyController.GetAll(pharmacySearchCriteria);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _pharmacyManagerMock.VerifyAll();
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]

        public void GetPharmaciesFailResourceNotFoundException()
        {
            _pharmacyManagerMock.Setup(x => x.GetAll(pharmacySearchCriteria)).Throws(new ResourceNotFoundException(resourceNotFoundExceptionMessage));
            var result = _pharmacyController.GetAll(pharmacySearchCriteria);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _pharmacyManagerMock.VerifyAll();
            Assert.AreEqual(404, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]

        public void GetPharmaciesFailInvalidResourceException()
        {
            _pharmacyManagerMock.Setup(x => x.GetAll(pharmacySearchCriteria)).Throws(new InvalidResourceException(invalidResourceExceptionMessage));
            var result = _pharmacyController.GetAll(pharmacySearchCriteria);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _pharmacyManagerMock.VerifyAll();
            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public void GetPharmacyById()
        {
            _pharmacyManagerMock.Setup(x => x.GetById(pharmacy.Id)).Returns(pharmacy);
            var result = _pharmacyController.GetById(pharmacy.Id);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _pharmacyManagerMock.VerifyAll();
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void GetPharmacyFailResourceNotFoundException()
        {
            _pharmacyManagerMock.Setup(x => x.GetById(pharmacy.Id)).Throws(new ResourceNotFoundException(resourceNotFoundExceptionMessage));
            var result = _pharmacyController.GetById(pharmacy.Id);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _pharmacyManagerMock.VerifyAll();
            Assert.AreEqual(404, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void GetPharmacyFailInvalidResourceException()
        {
            _pharmacyManagerMock.Setup(x => x.GetById(pharmacy.Id)).Throws(new InvalidResourceException(invalidResourceExceptionMessage));
            var result = _pharmacyController.GetById(pharmacy.Id);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _pharmacyManagerMock.VerifyAll();
            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        public void PostPharmacyOk()
        {
            _pharmacyManagerMock.Setup(x => x.Create(It.IsAny<Pharmacy>())).Returns(pharmacy);
            var result = _pharmacyController.Create(pharmacyModel);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _pharmacyManagerMock.VerifyAll();
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void PostPharmacyFailInvalidResourceException()
        {
            _pharmacyManagerMock.Setup(x => x.Create(It.IsAny<Pharmacy>())).Throws(new InvalidResourceException(invalidResourceExceptionMessage));
            var result = _pharmacyController.Create(pharmacyModel);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _pharmacyManagerMock.VerifyAll();
            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void PostPharmacyFailResourceNotFoundException()
        {
            _pharmacyManagerMock.Setup(x => x.Create(It.IsAny<Pharmacy>())).Throws(new ResourceNotFoundException(resourceNotFoundExceptionMessage));
            var result = _pharmacyController.Create(pharmacyModel);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _pharmacyManagerMock.VerifyAll();
            Assert.AreEqual(404, statusCode);
        }

        [TestMethod]
        public void PutPharmacyOk()
        {
            _pharmacyManagerMock.Setup(x => x.Update(pharmacy.Id, It.IsAny<Pharmacy>())).Returns(pharmacy);
            var result = _pharmacyController.Update(pharmacy.Id, pharmacyModel);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _pharmacyManagerMock.VerifyAll();
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void PutPharmacyFailInvalidResourceException()
        {
            _pharmacyManagerMock.Setup(x => x.Update(pharmacy.Id, It.IsAny<Pharmacy>())).Throws(new InvalidResourceException(invalidResourceExceptionMessage));
            var result = _pharmacyController.Update(pharmacy.Id, pharmacyModel);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _pharmacyManagerMock.VerifyAll();
            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void PutPharmacyFailResourceNotFoundException()
        {
            _pharmacyManagerMock.Setup(x => x.Update(pharmacy.Id, It.IsAny<Pharmacy>())).Throws(new ResourceNotFoundException(resourceNotFoundExceptionMessage));
            var result = _pharmacyController.Update(pharmacy.Id, pharmacyModel);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _pharmacyManagerMock.VerifyAll();
            Assert.AreEqual(404, statusCode);
        }

        [TestMethod]
        public void DeleteOk()
        {
            _pharmacyManagerMock.Setup(x => x.Delete(It.IsAny<int>()));
            var result = _pharmacyController.Delete(It.IsAny<int>());
            _pharmacyManagerMock.VerifyAll();

            // Assert
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            var value = objectResult.Value as Object;

            Assert.AreEqual(true, value);
            Assert.AreEqual(200, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void DeletePharmacyFailInvalidResourceException()
        {
            _pharmacyManagerMock.Setup(x => x.Delete(pharmacy.Id)).Throws(new InvalidResourceException(invalidResourceExceptionMessage));
            var result = _pharmacyController.Delete(pharmacy.Id);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _pharmacyManagerMock.VerifyAll();
            Assert.AreEqual(400, statusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void DeletePharmacyFailResourceNotFoundException()
        {
            _pharmacyManagerMock.Setup(x => x.Delete(pharmacy.Id)).Throws(new ResourceNotFoundException(resourceNotFoundExceptionMessage));
            var result = _pharmacyController.Delete(pharmacy.Id);
            var objectResult = result as ObjectResult;
            var statusCode = objectResult.StatusCode;
            _pharmacyManagerMock.VerifyAll();
            Assert.AreEqual(404, statusCode);
        }

        private IEnumerable<Pharmacy> GeneratePharmacyList()
        {
            var pharmacyList = new List<Pharmacy>();
            for (int i = 1; i < 11; i++)
            {
                pharmacyList.Add(new Pharmacy()
                {
                    Id = i,
                    Address = $"adress{i}",
                    Name = $"pharmacy{i}",
                    Users = new List<User>(),
                    Drugs = new List<Drug>()
                });
            }
            return pharmacyList;
        }

        [TestMethod]
        public void Test_PharmacySearchCriteria()
        {

            var searchCriteria = new PharmacySearchCriteria();
            searchCriteria.Name = "Farmacia1";
            searchCriteria.Address = "123456";
            var res1 = searchCriteria.Criteria(new Pharmacy { Address = "123456", Name = "Farmacia1" });
            searchCriteria.Name = "";
            searchCriteria.Address = "123456";
            var res2 = searchCriteria.Criteria(new Pharmacy { Address = "123456", Name = "" });
            searchCriteria.Name = "Farmacia1";
            searchCriteria.Address = "";
            var res3 = searchCriteria.Criteria(new Pharmacy { Address = "", Name = "Farmacia1" });
            searchCriteria.Name = "";
            searchCriteria.Address = "";
            var res4 = searchCriteria.Criteria(new Pharmacy { Address = "", Name = "" });
            
            // Assert
            Assert.IsNotNull(res1);
            Assert.IsNotNull(res2);
            Assert.IsNotNull(res3);
            Assert.IsNotNull(res4);
        }
    }
}
