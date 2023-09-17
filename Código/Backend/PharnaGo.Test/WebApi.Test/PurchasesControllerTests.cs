using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.Enums;
using PharmaGo.Exceptions;
using PharmaGo.IBusinessLogic;
using PharmaGo.WebApi.Controllers;
using PharmaGo.WebApi.Converters;
using PharmaGo.WebApi.Models.In;
using PharmaGo.WebApi.Models.Out;

namespace PharmaGo.Test.WebApi.Test
{
    [TestClass]
    public class PurchasesControllerTests
    {
        private PurchasesController _purchasesController;
        private Mock<IPurchasesManager> _purchasesManagerMock;
        private PurchaseModelRequestToPurchaseConverter converter;
        private PurchaseModelRequest purchaseModel;
        private Purchase purchase;
        private Purchase purchase_2;
        private Pharmacy pharmacy;
        private Pharmacy pharmacy2;
        private ICollection<PurchaseDetail> purchaseDetail;
        private Drug drug1;
        private Drug drug2;
        private UnitMeasure unitMeasure1;
        private UnitMeasure unitMeasure2;
        private Presentation presentation1;
        private Presentation presentation2;
        string token = "f0c4ca1b-d7a8-4cf7-8eed-b6cfdce557cd";

        [TestInitialize]
        public void SetUp()
        {

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = token;

            _purchasesManagerMock = new Mock<IPurchasesManager>(MockBehavior.Strict);
            _purchasesController = new PurchasesController(_purchasesManagerMock.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };
            converter = new PurchaseModelRequestToPurchaseConverter();

            
            ICollection<PurchaseModelRequest.PurchaseDetailModelRequest> purchaseModelDetailRequest =
                new List<PurchaseModelRequest.PurchaseDetailModelRequest>
            {
                new PurchaseModelRequest.PurchaseDetailModelRequest { Code = "XF324", Quantity = 2, PharmacyId = 1 },
                new PurchaseModelRequest.PurchaseDetailModelRequest { Code = "RS546", Quantity = 1, PharmacyId = 2 }
            };

            purchaseModel = new PurchaseModelRequest()
            {
                BuyerEmail = "roberto.perez@gmail.com",
                PurchaseDate = new DateTime(2022, 09, 19, 14, 34, 44),
                Details = purchaseModelDetailRequest
            };

            unitMeasure1 = new UnitMeasure { Id = 1, Deleted = false, Name = "ml" };
            unitMeasure2 = new UnitMeasure { Id = 2, Deleted = false, Name = "mg" };
            presentation1 = new Presentation { Id = 1, Deleted = false,  Name = "liquid" };
            presentation2 = new Presentation { Id = 2, Deleted = false, Name = "capsules" };

            pharmacy = new Pharmacy { Id = 1, Name = "Farmacia 1", Address = "Av. Italia 12345", Users = new List<User>() };
            pharmacy2 = new Pharmacy { Id = 2, Name = "Farmacia 2", Address = "Av. Italia 22222", Users = new List<User>() };
            drug1 = new Drug { Id = 1, Deleted = false, Code = "XF324", Name = "Aspirina", Prescription = false, Price = 100, Stock = 50, Quantity = 10, UnitMeasure = unitMeasure1, Presentation = presentation1, Symptom = "afecciones bronquiales que cursan con tos y secreciones" };
            drug2 = new Drug { Id = 2, Deleted = false, Code = "RS546", Name = "Abrilar", Prescription = false, Price = 250, Stock = 50, Quantity = 20, UnitMeasure = unitMeasure2, Presentation = presentation2, Symptom = "acción analgésica, alivio de los dolores ocasionales leves o\r\nmoderados, como dolores de cabeza, musculares, de espalda.\r\nPresentación: comprimidos" };

            purchaseDetail = new List<PurchaseDetail> {
                new PurchaseDetail{Id = 1, Quantity = 2, Price = new decimal(100), Drug =  drug1, Pharmacy = pharmacy, Status = "Pending"},
                new PurchaseDetail{Id = 2, Quantity = 1, Price = new decimal(250), Drug = drug2, Pharmacy = pharmacy2, Status = "Pending" }
            };

            purchase = new Purchase
            {
                Id = 1,
                BuyerEmail = "roberto.perez@gmail.com",
                PurchaseDate = new DateTime(2022, 09, 19, 14, 34, 44),
                TotalAmount = new decimal(450),
                TrackingCode = "FROR7HWPUH5JWW4C",
                details = purchaseDetail
            };

            purchase_2 = new Purchase
            {
                Id = 1,
                BuyerEmail = "roberto.perez@gmail.com",
                PurchaseDate = new DateTime(2022, 08, 12, 00, 00, 00),
                TotalAmount = new decimal(450),
                TrackingCode = "MROR7HWPUH5JWW42",
                details = purchaseDetail
            };

        }

        [TestCleanup]
        public void Cleanup()
        {
            _purchasesManagerMock.VerifyAll();
        }

        [TestMethod]
        public void Create_Purchase_Ok()
        {

            //Arrange
            Purchase _purchase = converter.Convert(purchaseModel);
            _purchasesManagerMock
                .Setup(service => service.CreatePurchase(It.IsAny<Purchase>()))
                .Returns(purchase);

            //Act
            var result = _purchasesController.CreatePurchase(purchaseModel);

            //Assert
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult.StatusCode;
            var value = objectResult.Value as PurchaseModelResponse;

            //Assert
            Assert.AreEqual(200, statusCode);
            Assert.AreEqual(value.BuyerEmail, purchaseModel.BuyerEmail);
            Assert.AreEqual(value.TrackingCode, "FROR7HWPUH5JWW4C");
            Assert.AreEqual(value.Details.ElementAt(0).PharmacyName, "Farmacia 1");
            Assert.AreEqual(value.Details.ElementAt(0).PharmacyId, 1);
            Assert.AreEqual(value.Details.ElementAt(0).Status, "Pending");
            Assert.AreEqual(value.Details.Count, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void Create_Purchase_ResourceNotFoundException()
        {
            //Arrange
            Purchase _purchase = converter.Convert(purchaseModel);
            var resourceNotFoundException = "ResourceNotFoundException";
            _purchasesManagerMock.Setup(service => service.CreatePurchase(It.IsAny<Purchase>()))
                .Throws(new ResourceNotFoundException(resourceNotFoundException));

            //Act
            var result = _purchasesController.CreatePurchase(purchaseModel);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void Create_Purchase_Failed_InvalidResourceException()
        {
            //Arrange
            Purchase _purchase = converter.Convert(purchaseModel);
            var invalidResourceException = "InvalidResourceException";
            _purchasesManagerMock.Setup(service => service.CreatePurchase(It.IsAny<Purchase>()))
                .Throws(new InvalidResourceException(invalidResourceException));

            //Act
            var result = _purchasesController.CreatePurchase(purchaseModel);

        }

        [TestMethod]
        public void Get_Purchases_By_Date_No_Start_Date_No_End_Date()
        {
            //Arrange
            DateTime? start = null;
            DateTime? end = null;
            var purchaseList = new List<Purchase>();
            purchaseList.Add(purchase);
            purchaseList.Add(purchase_2);

            _purchasesManagerMock
                .Setup(service => service.GetAllPurchasesByDate(token, start, end))
                .Returns(purchaseList);

            //Act
            var result = _purchasesController.ByDate(start, end);

            //Assert
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult.StatusCode;
            var value = objectResult.Value as ICollection<PurchaseModelResponse>;

            //Assert
            Assert.AreEqual(200, statusCode);
            Assert.AreEqual(value.ElementAt(0).TotalAmount, 450);
            Assert.AreEqual(value.ElementAt(0).Details.Count, 2);
        }

        [TestMethod]
        public void Get_Purchases_All_Ok()
        {
            //Arrange
            var purchaseList = new List<Purchase>();
            purchaseList.Add(purchase);
            purchaseList.Add(purchase_2);
            string token = "f0c4ca1b-d7a8-4cf7-8eed-b6cfdce557cd";

            _purchasesManagerMock
                .Setup(service => service.GetAllPurchases(token))
                .Returns(purchaseList);

            //Act
            var result = _purchasesController.All();

            //Assert
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult.StatusCode;
            var value = objectResult.Value as ICollection<PurchaseModelResponse>;

            //Assert
            Assert.AreEqual(200, statusCode);
            Assert.AreEqual(value.ElementAt(0).TotalAmount, 450);
            Assert.AreEqual(value.ElementAt(0).PurchaseDate, purchase.PurchaseDate);
            Assert.AreEqual(value.ElementAt(1).TotalAmount, 450);
            Assert.AreEqual(value.ElementAt(0).Details.Count, 2);
            Assert.AreEqual(value.ElementAt(1).Details.Count, 2);
            Assert.AreEqual(value.ElementAt(0).Details.ElementAt(0).Code, purchase.details.ElementAt(0).Drug.Code);
            Assert.AreEqual(value.ElementAt(0).Details.ElementAt(0).Name, purchase.details.ElementAt(0).Drug.Name);
            Assert.AreEqual(value.ElementAt(0).Details.ElementAt(0).Quantity, purchase.details.ElementAt(0).Quantity);
            Assert.AreEqual(value.ElementAt(0).Details.ElementAt(0).Price, purchase.details.ElementAt(0).Drug.Price);
            Assert.AreEqual(value.ElementAt(0).Details.ElementAt(0).PharmacyName, purchase.details.ElementAt(0).Pharmacy.Name);
            Assert.AreEqual(value.ElementAt(0).Details.ElementAt(0).PharmacyId, purchase.details.ElementAt(0).Pharmacy.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void Get_Purchases_By_Year_No_Start_Date()
        {
            //Arrange
            DateTime? start = null;
            DateTime? end = DateTime.Now;
            var purchaseList = new List<Purchase>();
            purchaseList.Add(purchase);
            purchaseList.Add(purchase_2);

            var invalidResourceException = "InvalidResourceException";
            _purchasesManagerMock
                .Setup(service => service.GetAllPurchasesByDate(token, start, end))
                .Throws(new InvalidResourceException(invalidResourceException));

            //Act
            var result = _purchasesController.ByDate(start, end);   
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void Get_Purchases_By_Year_No_End_Date()
        {

            //Arrange
            DateTime? start = DateTime.Now;
            DateTime? end = null;
            var purchaseList = new List<Purchase>();
            purchaseList.Add(purchase);
            purchaseList.Add(purchase_2);

            var invalidResourceException = "InvalidResourceException";
            _purchasesManagerMock
                .Setup(service => service.GetAllPurchasesByDate(token, start, end))
                .Throws(new InvalidResourceException(invalidResourceException));

            //Act
            var result = _purchasesController.ByDate(start, end);
        }

        [TestMethod]
        public void Approve_Purchase_Detail_Ok()
        {
            //Arrange
            var model = new PurchaseAuthorizationModel { drugCode = "XF324", pharmacyId = 1};
            purchaseDetail.ElementAt(0).Status = "Approved";

            _purchasesManagerMock
                .Setup(service => service.ApprobePurchaseDetail(1, model.pharmacyId, model.drugCode))
                .Returns(purchaseDetail.ElementAt(0));

            //Act
            var result = _purchasesController.Approve(1, model);

            //Assert
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult.StatusCode;
            var value = objectResult.Value as PurchaseDetailModelResponse;

            //Assert
            Assert.IsNotNull(value);
            Assert.AreEqual(200, statusCode);
            Assert.AreEqual(value.Status, "Approved");
            Assert.AreEqual(value.DrugCode, model.drugCode);
            Assert.AreEqual(value.PharmacyId, model.pharmacyId);
        }

        [TestMethod]
        public void Reject_Purchase_Detail_Ok()
        {
            //Arrange
            var model = new PurchaseAuthorizationModel { drugCode = "XF324", pharmacyId = 1 };
            purchaseDetail.ElementAt(0).Status = "Rejected";

            _purchasesManagerMock
                .Setup(service => service.RejectPurchaseDetail(1, model.pharmacyId, model.drugCode))
                .Returns(purchaseDetail.ElementAt(0));

            //Act
            var result = _purchasesController.Reject(1, model);

            //Assert
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult.StatusCode;
            var value = objectResult.Value as PurchaseDetailModelResponse;

            //Assert
            Assert.IsNotNull(value);
            Assert.AreEqual(200, statusCode);
            Assert.AreEqual(value.Status, "Rejected");
            Assert.AreEqual(value.DrugCode, model.drugCode);
            Assert.AreEqual(value.PharmacyId, model.pharmacyId);
        }


        [TestMethod]
        public void Get_Tracking_Purchase_Ok()
        {
            //Arrange
            var model = new PurchaseAuthorizationModel { drugCode = "XF324", pharmacyId = 1 };
            string code = "FROR7HWPUH5JWW4C";

            _purchasesManagerMock
                .Setup(service => service.GetPurchaseByTrackingCode(code))
                .Returns(purchase);

            //Act
            var result = _purchasesController.Tracking(code);

            //Assert
            var objectResult = result as OkObjectResult;
            var statusCode = objectResult.StatusCode;
            var value = objectResult.Value as PurchaseModelResponse;

            //Assert
            Assert.IsNotNull(value);
            Assert.AreEqual(200, statusCode);
            Assert.AreEqual(value.TrackingCode, code);
        }
    }
}

