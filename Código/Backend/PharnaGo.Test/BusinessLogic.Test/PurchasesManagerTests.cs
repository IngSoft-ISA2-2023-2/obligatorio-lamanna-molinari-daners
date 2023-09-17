using Microsoft.VisualBasic;
using Moq;
using PharmaGo.BusinessLogic;
using PharmaGo.Domain.Entities;
using PharmaGo.Exceptions;
using PharmaGo.IDataAccess;

namespace PharmaGo.Test.BusinessLogic.Test
{
    [TestClass]
    public class PurchasesManagerTests
    {

        private Mock<IRepository<Purchase>> _purchaseRespository;
        private Mock<IRepository<Pharmacy>> _pharmacyRespository;
        private Mock<IRepository<Drug>> _drugsRespository;
        private Mock<IRepository<User>> _userRespository;
        private Mock<IRepository<Session>> _sessionRespository;
        private Mock<IRepository<PurchaseDetail>> _purchaseDetailRespository;
        private PurchasesManager _purchasesManager;
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
        private string token = "c80da9ed-1b41-4768-8e34-b728cae25d2f";
        private Session session = null;
        private User user = null;

        [TestInitialize]
        public void SetUp()
        {
            _purchaseRespository = new Mock<IRepository<Purchase>>(MockBehavior.Strict);
            _pharmacyRespository = new Mock<IRepository<Pharmacy>>(MockBehavior.Strict);
            _drugsRespository = new Mock<IRepository<Drug>>(MockBehavior.Strict);
            _userRespository = new Mock<IRepository<User>>(MockBehavior.Strict);
            _sessionRespository = new Mock<IRepository<Session>>(MockBehavior.Strict);
            _purchaseDetailRespository = new Mock<IRepository<PurchaseDetail>>(MockBehavior.Strict);
            _purchasesManager = new PurchasesManager(_purchaseRespository.Object, _pharmacyRespository.Object, _drugsRespository.Object,
            _purchaseDetailRespository.Object, _sessionRespository.Object, _userRespository.Object);

            unitMeasure1 = new UnitMeasure { Id = 1, Deleted = false, Name = "ml" };
            unitMeasure2 = new UnitMeasure { Id = 2, Deleted = false, Name = "mg" };
            presentation1 = new Presentation { Id = 1, Deleted = false, Name = "liquid" };
            presentation2 = new Presentation { Id = 2, Deleted = false, Name = "capsules" };

            drug1 = new Drug { Id = 1, Deleted = false, Code = "XF324", Name = "Aspirina", Prescription = false, Price = 100, Stock = 50, Quantity = 10, UnitMeasure = unitMeasure1, Presentation = presentation1, Symptom = "afecciones bronquiales que cursan con tos y secreciones" };
            drug2 = new Drug { Id = 2, Deleted = false, Code = "RS546", Name = "Abrilar", Prescription = false, Price = 250, Stock = 50, Quantity = 20, UnitMeasure = unitMeasure2, Presentation = presentation2, Symptom = "acción analgésica, alivio de los dolores ocasionales leves o\r\nmoderados, como dolores de cabeza, musculares, de espalda.\r\nPresentación: comprimidos" };

            pharmacy = new Pharmacy { Id = 1, Name = "Farmacia 1", Address = "Av. Italia 12345", Users = new List<User>(), Drugs = new List<Drug> { drug1 } };
            pharmacy2 = new Pharmacy { Id = 2, Name = "Farmacia 2", Address = "Av. Italia 22222", Users = new List<User>(), Drugs = new List<Drug> { drug2 } };

            purchaseDetail = new List<PurchaseDetail> {
                new PurchaseDetail{Id = 1, Quantity = 2, Price = new decimal(100), Drug =  drug1, Pharmacy = pharmacy, Status = "Pending"},
                new PurchaseDetail{Id = 2, Quantity = 1, Price = new decimal(250), Drug = drug2, Pharmacy = pharmacy2, Status = "Approved" }
            };

            purchase = new Purchase
            {
                Id = 1,
                BuyerEmail = "roberto.perez@gmail.com",
                PurchaseDate = new DateTime(2022, 09, 19, 14, 34, 44),
                TotalAmount = new decimal(450),
                details = purchaseDetail
            };

            purchase_2 = new Purchase
            {
                Id = 2,
                BuyerEmail = "carlos.perez@gmail.com",
                PurchaseDate = new DateTime(2022, 10, 19, 14, 34, 44),
                TotalAmount = new decimal(450),
                details = purchaseDetail
            };
            session = new Session { Id = 1, Token = new Guid(token), UserId = 1 };
            user = new User { Id = 1, Email = "fernando@gmail.com", Password = "Asdfer234..", Pharmacy = pharmacy};
        }

        [TestCleanup]
        public void Cleanup()
        {
            _purchaseRespository.VerifyAll();
            _pharmacyRespository.VerifyAll();
            _drugsRespository.VerifyAll();
            _userRespository.VerifyAll();
            _sessionRespository.VerifyAll();
            _purchaseDetailRespository.VerifyAll();
        }

        [TestMethod]
        public void Create_Purchase_Ok()
        {
            //Arrange
            pharmacy.Drugs = new List<Drug>
            {
                drug1,
                drug2
            };
            int pharmacyId = 1;
            int pharmacyId2 = 2;

            _pharmacyRespository.Setup(y => y.GetOneByExpression(x => x.Id == pharmacyId)).Returns(pharmacy);
            _pharmacyRespository.Setup(y => y.GetOneByExpression(x => x.Id == pharmacyId2)).Returns(pharmacy2);
            _purchaseRespository.Setup(x => x.InsertOne(purchase));
            _purchaseRespository.Setup(x => x.Save());

            //Act
            var response = _purchasesManager.CreatePurchase(purchase);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.details.ElementAt(0).Status, "Pending");
            Assert.AreEqual(response.details.ElementAt(1).Status, "Pending");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void Create_Purchase_Fail_Empty_Email()
        {
            //Arrange
            purchase.BuyerEmail = "";

            //Act
           var response = _purchasesManager.CreatePurchase(purchase);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void Create_Purchase_Fail_Invalid_Email()
        {
            //Arrange
            purchase.BuyerEmail = "carlos@@gmail.com";

            //Act
            var response = _purchasesManager.CreatePurchase(purchase);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void Create_Purchase_Fail_No_Items()
        {
            //Arrange
            purchase.details = new List<PurchaseDetail>();

            //Act
           var response = _purchasesManager.CreatePurchase(purchase);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void Create_Purchase_Fail_Null_Items()
        {
            //Arrange
            purchase.details = null;

            //Act
            var response = _purchasesManager.CreatePurchase(purchase);
        }


        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void Create_Purchase_Fail_Invalid_Pharmacy()
        {
            //Arrange
            Pharmacy pharmacy = null;
            int pharmacyId = 1;
            _pharmacyRespository.Setup(y => y.GetOneByExpression(x => x.Id == pharmacyId)).Returns(pharmacy);

            //Act
            var response = _purchasesManager.CreatePurchase(purchase);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void Create_Purchase_Fail_Invalid_Drug()
        {
            //Arrange
            pharmacy.Drugs = new List<Drug>
            {
                drug1,
                drug2
            };
            Drug drug3 = new Drug { Id = 3, Deleted = false, Code = "RS500", Name = "Perifar", Prescription = false, Price = 250, Stock = 50, Quantity = 20, UnitMeasure = unitMeasure2, Presentation = presentation2, Symptom = "acción analgésica, alivio de los dolores ocasionales leves o\r\nmoderados, como dolores de cabeza, musculares, de espalda.\r\nPresentación: comprimidos" };
            Pharmacy pharmacy3 = new Pharmacy { Id = 3, Name = "Farmacia 3", Address = "Av. Italia 3333", Users = new List<User>(), Drugs = new List<Drug> { drug3 } };
            PurchaseDetail purchaseDetail3 = new PurchaseDetail { Id = 1, Quantity = 2, Price = new decimal(100), Drug = drug2, Pharmacy = pharmacy3, Status = "Pending" };
            purchase.details.Add(purchaseDetail3);

            _pharmacyRespository.Setup(y => y.GetOneByExpression(x => x.Id == 1)).Returns(pharmacy);
            _pharmacyRespository.Setup(y => y.GetOneByExpression(x => x.Id == 2)).Returns(pharmacy2);
            _pharmacyRespository.Setup(y => y.GetOneByExpression(x => x.Id == 3)).Returns(pharmacy3);

            //Act
            var response = _purchasesManager.CreatePurchase(purchase);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void Create_Purchase_Fail_Invalid_Purchase_Date()
        {
            //Arrange
            purchase.PurchaseDate = DateTime.MinValue;
            pharmacy.Drugs = new List<Drug>();
            pharmacy.Drugs.Add(drug1);
            pharmacy.Drugs.Add(drug2);
            purchaseDetail = new List<PurchaseDetail> {
                new PurchaseDetail{Id = 1, Quantity = 2, Price = new decimal(100), Drug =  drug1},
                new PurchaseDetail{Id = 2, Quantity = 51, Price = new decimal(250), Drug = drug2 }
            };
            purchase.details = purchaseDetail;
            
            //Act
            var response = _purchasesManager.CreatePurchase(purchase);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void Create_Purchase_Fail_Invalid_Pharmacy_Id()
        {
            //Arrange
            pharmacy.Drugs = new List<Drug>
            {
                drug1,
                drug2
            };
            purchaseDetail = new List<PurchaseDetail> {
                new PurchaseDetail{Id = 1, Quantity = 2, Price = new decimal(100), Drug =  drug1, Pharmacy =  new Pharmacy { Id = 0}},
                new PurchaseDetail{Id = 2, Quantity = 51, Price = new decimal(250), Drug = drug2, Pharmacy = pharmacy }
            };
            purchase.details = purchaseDetail;

            //Act
            var response = _purchasesManager.CreatePurchase(purchase);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void Create_Purchase_Fail_Invalid_Quantity()
        {
            //Arrange
            pharmacy.Drugs = new List<Drug>();
            pharmacy.Drugs.Add(drug1);
            pharmacy.Drugs.Add(drug2);
            purchaseDetail = new List<PurchaseDetail> {
                new PurchaseDetail{Id = 1, Quantity = 0, Price = new decimal(100), Drug =  drug1, Pharmacy = pharmacy},
                new PurchaseDetail{Id = 2, Quantity = 51, Price = new decimal(250), Drug = drug2, Pharmacy = pharmacy2 }
            };
            purchase.details = purchaseDetail;

            _pharmacyRespository.Setup(y => y.GetOneByExpression(x => x.Id == 1)).Returns(pharmacy);

            //Act
            var response = _purchasesManager.CreatePurchase(purchase);
        }

        [TestMethod]
        public void Get_Purchases_All_Ok()
        {
            //Arrange
            string token = "f0c4ca1b-d7a8-4cf7-8eed-b6cfdce557cd";
            var purchaseList = new List<Purchase>
            {
                purchase,
                purchase_2
            };
            var guidToken = new Guid(token);
            _sessionRespository.Setup(z => z.GetOneByExpression(s => s.Token == guidToken))
                .Returns(new Session {Id = 1, Token = guidToken, UserId = 1 });
            _userRespository.Setup(u => u.GetOneDetailByExpression(u => u.Id == 1))
                .Returns(user);
            _purchaseRespository.Setup(y => y.GetAllByExpression(s => s.Id > 0))
                .Returns(purchaseList);

            //Act
            var response = _purchasesManager.GetAllPurchases(token);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.ElementAt(0).details.ElementAt(0).Id, 1);
            Assert.AreEqual(response.ElementAt(0).details.ElementAt(0).Price, 100);
            Assert.AreEqual(response.Count, 2);
        }



        [TestMethod]
        public void Get_Purchases_All_Ok___()
        {
            //Arrange
            string token = "f0c4ca1b-d7a8-4cf7-8eed-b6cfdce557cd";

            pharmacy2 = new Pharmacy { Id = 100, Name = "Farmacia 100", Address = "Av. Italia 100", Users = new List<User>(), Drugs = new List<Drug> { drug1 } };

            purchaseDetail = new List<PurchaseDetail> {
                new PurchaseDetail{Id = 1, Quantity = 2, Price = new decimal(100), Drug =  drug1, Pharmacy = pharmacy, Status = "Pending"},
                new PurchaseDetail{Id = 2, Quantity = 1, Price = new decimal(250), Drug = drug2, Pharmacy = pharmacy2, Status = "Approved" }
            };

            ICollection<PurchaseDetail> purchaseDetail2 = new List<PurchaseDetail> {
                new PurchaseDetail{Id = 1, Quantity = 2, Price = new decimal(100), Drug =  drug1, Pharmacy = pharmacy, Status = "Rejected"},
                new PurchaseDetail{Id = 2, Quantity = 1, Price = new decimal(250), Drug = drug2, Pharmacy = pharmacy2, Status = "Approved" }
            };

            purchase = new Purchase
            {
                Id = 1,
                BuyerEmail = "roberto.perez@gmail.com",
                PurchaseDate = new DateTime(2022, 09, 19, 14, 34, 44),
                TotalAmount = new decimal(450),
                details = purchaseDetail
            };

            purchase_2 = new Purchase
            {
                Id = 2,
                BuyerEmail = "carlos.perez@gmail.com",
                PurchaseDate = new DateTime(2022, 10, 19, 14, 34, 44),
                TotalAmount = new decimal(450),
                details = purchaseDetail2
            };

            var purchaseList = new List<Purchase>
            {
                purchase,
                purchase_2
            };
            var guidToken = new Guid(token);
            _sessionRespository.Setup(z => z.GetOneByExpression(s => s.Token == guidToken))
                .Returns(new Session { Id = 1, Token = guidToken, UserId = 1 });
            _userRespository.Setup(u => u.GetOneDetailByExpression(u => u.Id == 1))
                .Returns(user);
            _purchaseRespository.Setup(y => y.GetAllByExpression(s => s.Id > 0))
                .Returns(purchaseList);

            //Act
            var response = _purchasesManager.GetAllPurchases(token);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.Count, 1);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void Get_All_Purchases_By_Date_Start_Date_Bigger_Than_End_Date()
        {
            //Arrange
            DateTime? start = DateTime.Now.AddDays(1);
            DateTime? end = DateTime.Now;
            var purchaseList = new List<Purchase>();
            purchaseList.Add(purchase);
            var guidToken = new Guid(token);
            _sessionRespository.Setup(z => z.GetOneByExpression(s => s.Token == guidToken))
                .Returns(new Session { Id = 1, Token = guidToken, UserId = 1 });
            _userRespository.Setup(u => u.GetOneDetailByExpression(u => u.Id == 1))
                .Returns(user);

            //Act
            var response = _purchasesManager.GetAllPurchasesByDate(token, start, end);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void Get_All_Purchases_By_Date_Invalid_End_Date()
        {
            //Arrange
            DateTime? start = DateTime.Now;
            DateTime? end = null;
            var purchaseList = new List<Purchase>
            {
                purchase
            };
            var guidToken = new Guid(token);
            _sessionRespository.Setup(z => z.GetOneByExpression(s => s.Token == guidToken))
                .Returns(new Session { Id = 1, Token = guidToken, UserId = 1 });
            _userRespository.Setup(u => u.GetOneDetailByExpression(u => u.Id == 1))
                .Returns(user);

            //Act
            var response = _purchasesManager.GetAllPurchasesByDate(token, start, end);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void Get_All_Purchases_By_Date_Invalid_Start_Date()
        {
            //Arrange
            DateTime? start = null;
            DateTime? end = DateTime.Now;
            var purchaseList = new List<Purchase>
            {
                purchase
            };

            var guidToken = new Guid(token);
            _sessionRespository.Setup(z => z.GetOneByExpression(s => s.Token == guidToken))
                .Returns(new Session { Id = 1, Token = guidToken, UserId = 1 });
            _userRespository.Setup(u => u.GetOneDetailByExpression(u => u.Id == 1))
                .Returns(user);

            //Act
            var response = _purchasesManager.GetAllPurchasesByDate(token, start, end);
        }

        [TestMethod]
        public void Get_All_Purchases_By_Date_No_Start_Date_And_No_End_Date()
        {
            //Arrange
            DateTime? start = null;
            DateTime? end = null;
            var purchaseList = new List<Purchase>
            {
                purchase
            };
            var today = DateTime.Now;
            var lastDayOfMonth = DateTime.DaysInMonth(today.Year, today.Month);
            var lastDateOfMonth = new DateTime(today.Year, today.Month, lastDayOfMonth, 23, 59, 59);
            var firstDateOfMonth = new DateTime(today.Year, today.Month, 1, 0, 0, 0);

            var guidToken = new Guid(token);
            _sessionRespository.Setup(z => z.GetOneByExpression(s => s.Token == guidToken))
                .Returns(new Session { Id = 1, Token = guidToken, UserId = 1 });
            _userRespository.Setup(u => u.GetOneDetailByExpression(u => u.Id == 1))
                .Returns(user);
            _purchaseRespository.Setup(y => y.GetAllByExpression(p =>
                p.PurchaseDate <= lastDateOfMonth && p.PurchaseDate >= firstDateOfMonth))
                .Returns(purchaseList);

            //Act
            var response = _purchasesManager.GetAllPurchasesByDate(token, start, end);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.Count, 1);
        }


        [TestMethod]
        public void Get_All_Purchases_By_Date()
        {
            //Arrange
            DateTime? start = DateTime.Now;
            DateTime? end = DateTime.Now.AddDays(12);
            var purchaseList = new List<Purchase>();
            purchaseList.Add(purchase);

            var guidToken = new Guid(token);
            _sessionRespository.Setup(z => z.GetOneByExpression(s => s.Token == guidToken))
                .Returns(new Session { Id = 1, Token = guidToken, UserId = 1 });
            _userRespository.Setup(u => u.GetOneDetailByExpression(u => u.Id == 1))
                .Returns(user);
            _purchaseRespository
                .Setup(y => y.GetAllByExpression(purchase =>
                purchase.PurchaseDate <= end && purchase.PurchaseDate >= start))
                .Returns(purchaseList);

            //Act
            var response = _purchasesManager.GetAllPurchasesByDate(token, start, end);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.Count, 1);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void Get_Purchase_By_TrackingCode_Fail()
        {
            //Arrange
            string trackingCode = "";

            //Act
            var response = _purchasesManager.GetPurchaseByTrackingCode(trackingCode);
        }

        [TestMethod]
        public void Get_Purchase_By_TrackingCode_Ok()
        {
            //Arrange
            string trackingCode = "1234565";
            _purchaseRespository
                .Setup(y => y.GetOneDetailByExpression(p => p.TrackingCode == trackingCode)).Returns(purchase);

            //Act
            var response = _purchasesManager.GetPurchaseByTrackingCode(trackingCode);
        }

        [TestMethod]
        public void Approve_Purchase_Ok()
        {
            //Arrange
            _purchaseRespository
                .Setup(y => y.GetOneDetailByExpression(p => p.Id == 1))
                .Returns(purchase);
            _pharmacyRespository
                .Setup(f => f.GetOneByExpression(p => p.Id == 1))
                .Returns(pharmacy);
            _drugsRespository.Setup(d => d.UpdateOne(drug1));
            _drugsRespository.Setup(d => d.Save());
            _purchaseDetailRespository.Setup(c => c.UpdateOne(purchaseDetail.ElementAt(0)));
            _purchaseDetailRespository.Setup(c => c.Save());

            //Act
            var response = _purchasesManager.ApprobePurchaseDetail(1, 1, "XF324");

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.Status, "Approved");
            Assert.AreEqual(response.Quantity, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void Approve_Purchase_Fail_Purchase_Not_Found()
        {
            //Arrange
            Purchase p = null; 
            _purchaseRespository
                .Setup(y => y.GetOneDetailByExpression(p => p.Id == 0))
                .Returns(p);

            //Act
            var response = _purchasesManager.ApprobePurchaseDetail(0, 1, "XF324");
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void Approve_Purchase_Fail_Purchase_Detail_Not_Found()
        {
            //Arrange
            PurchaseDetail p = null;
            _purchaseRespository
                .Setup(y => y.GetOneDetailByExpression(p => p.Id == 1))
                .Returns(purchase);

            //Act
            var response = _purchasesManager.ApprobePurchaseDetail(1, 1, "XF32000");
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void Approve_Purchase_Fail_Pharmacy_Not_Found()
        {
            //Arrange
            pharmacy = null;
            _purchaseRespository
                .Setup(y => y.GetOneDetailByExpression(p => p.Id == 1))
                .Returns(purchase);
            _pharmacyRespository
                .Setup(f => f.GetOneByExpression(p => p.Id == 1))
                .Returns(pharmacy);

            //Act
            var response = _purchasesManager.ApprobePurchaseDetail(1, 1, "XF324");
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void Approve_Purchase_Fail_Drug_Not_Found()
        {
            pharmacy.Drugs = new List<Drug> { new Drug { Deleted = true, Code = "XF32500" } };
            //Arrange
            _purchaseRespository
                .Setup(y => y.GetOneDetailByExpression(p => p.Id == 1))
                .Returns(purchase);
            _pharmacyRespository
                .Setup(f => f.GetOneByExpression(p => p.Id == 1))
                .Returns(pharmacy);

            //Act
            var response = _purchasesManager.ApprobePurchaseDetail(1, 1, "XF324");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void Approve_Purchase_Fail_No_Stock()
        {
            //Arrange
            purchaseDetail.ElementAt(0).Quantity = 10000;

            _purchaseRespository
                .Setup(y => y.GetOneDetailByExpression(p => p.Id == 1))
                .Returns(purchase);
            _pharmacyRespository
                .Setup(f => f.GetOneByExpression(p => p.Id == 1))
                .Returns(pharmacy);

            //Act
            var response = _purchasesManager.ApprobePurchaseDetail(1, 1, "XF324");
        }

        // reject

        [TestMethod]
        public void Reject_Purchase_Ok()
        {
            //Arrange
            _purchaseRespository
                .Setup(y => y.GetOneDetailByExpression(p => p.Id == 1))
                .Returns(purchase);
            _pharmacyRespository
                .Setup(f => f.GetOneByExpression(p => p.Id == 1))
                .Returns(pharmacy);
            _purchaseDetailRespository.Setup(c => c.UpdateOne(purchaseDetail.ElementAt(0)));
            _purchaseDetailRespository.Setup(c => c.Save());
            _purchaseRespository.Setup(y => y.UpdateOne(purchase));
            _purchaseRespository.Setup(y => y.Save());

            //Act
            var response = _purchasesManager.RejectPurchaseDetail(1, 1, "XF324");

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.Status, "Rejected");
            Assert.AreEqual(response.Quantity, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void Reject_Purchase_Fail_Purchase_Not_Found()
        {
            //Arrange
            Purchase p = null;
            _purchaseRespository
                .Setup(y => y.GetOneDetailByExpression(p => p.Id == 0))
                .Returns(p);

            //Act
            var response = _purchasesManager.RejectPurchaseDetail(0, 1, "XF324");
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void Reject_Purchase_Fail_Purchase_Detail_Not_Found()
        {
            //Arrange
            _purchaseRespository
                .Setup(y => y.GetOneDetailByExpression(p => p.Id == 1))
                .Returns(purchase);

            //Act
            var response = _purchasesManager.RejectPurchaseDetail(1, 1, "XF32000");
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void Reject_Purchase_Fail_Pharmacy_Not_Found()
        {
            //Arrange
            pharmacy = null;
            _purchaseRespository
                .Setup(y => y.GetOneDetailByExpression(p => p.Id == 1))
                .Returns(purchase);
            _pharmacyRespository
                .Setup(f => f.GetOneByExpression(p => p.Id == 1))
                .Returns(pharmacy);

            //Act
            var response = _purchasesManager.RejectPurchaseDetail(1, 1, "XF324");
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void Reject_Purchase_Fail_Drug_Not_Found()
        {
            //Arrange
            pharmacy.Drugs = new List<Drug> { new Drug { Deleted = true, Code = "XF32500" } }; ;
            _purchaseRespository
                .Setup(y => y.GetOneDetailByExpression(p => p.Id == 1))
                .Returns(purchase);
            _pharmacyRespository
                .Setup(f => f.GetOneByExpression(p => p.Id == 1))
                .Returns(pharmacy);

            //Act
            var response = _purchasesManager.RejectPurchaseDetail(1, 1, "XF324");
        }

    }
}


