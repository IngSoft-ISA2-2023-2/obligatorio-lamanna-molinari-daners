using Microsoft.EntityFrameworkCore;
using PharmaGo.DataAccess.Repositories;
using PharmaGo.DataAccess;
using PharmaGo.Domain.Entities;
using System.Linq.Expressions;
using PharmaGo.Domain.Enums;

namespace PharmaGo.Test.DataAccess.Test
{
    [TestClass]
    public class PurchasesRepositoryTests
    {
        private PurchasesRepository _purchasesRepository;
        private Purchase purchase;
        private Purchase purchase_2;
        private Pharmacy pharmacy;
        private Pharmacy pharmacy_2;
        private ICollection<PurchaseDetail> purchaseDetail;
        private ICollection<PurchaseDetail> purchaseDetail_2;
        private Drug drug1;
        private Drug drug2;
        private Drug drug3;
        private Drug drug4;
        private UnitMeasure unitMeasure1;
        private UnitMeasure unitMeasure2;
        private Presentation presentation1;
        private Presentation presentation2;
        private PharmacyRepository pharmacyRepository;
        DbContextOptions<PharmacyGoDbContext> options = new DbContextOptionsBuilder<PharmacyGoDbContext>()
            .UseInMemoryDatabase(databaseName: "PharmaGoDb")
            .Options;

        [TestInitialize]
        public void InitializeTest()
        {
            unitMeasure1 = new UnitMeasure { Deleted = false, Name = "ml" };
            unitMeasure2 = new UnitMeasure { Deleted = false, Name = "mg" };
            presentation1 = new Presentation { Deleted = false, Name = "liquid" };
            presentation2 = new Presentation { Deleted = false, Name = "capsules" };

            pharmacy = new Pharmacy { Name = "Farmacia 1", Address = "Av. Italia 12345", Users = new List<User>() };
            pharmacy_2 = new Pharmacy { Name = "Farmacia 2", Address = "Av. Italia 12345", Users = new List<User>() };
            drug1 = new Drug { Deleted = false, Code = "XF324", Name = "Aspirina", Prescription = false, Price = 100, Stock = 50, Quantity = 10, UnitMeasure = unitMeasure1, Presentation = presentation1, Symptom = "afecciones bronquiales que cursan con tos y secreciones" };
            drug2 = new Drug { Deleted = false, Code = "RS546", Name = "Abrilar", Prescription = false, Price = 250, Stock = 50, Quantity = 20, UnitMeasure = unitMeasure2, Presentation = presentation2, Symptom = "acción analgésica, alivio de los dolores ocasionales leves o\r\nmoderados, como dolores de cabeza, musculares, de espalda.\r\nPresentación: comprimidos" };
            drug3 = new Drug { Deleted = false, Code = "XF324", Name = "Aspirina", Prescription = false, Price = 100, Stock = 50, Quantity = 10, UnitMeasure = unitMeasure1, Presentation = presentation1, Symptom = "afecciones bronquiales que cursan con tos y secreciones" };
            drug4 = new Drug { Deleted = false, Code = "RS546", Name = "Abrilar", Prescription = false, Price = 250, Stock = 50, Quantity = 20, UnitMeasure = unitMeasure2, Presentation = presentation2, Symptom = "acción analgésica, alivio de los dolores ocasionales leves o\r\nmoderados, como dolores de cabeza, musculares, de espalda.\r\nPresentación: comprimidos" };

            purchaseDetail = new List<PurchaseDetail> {
                new PurchaseDetail{Quantity = 2, Price = new decimal(100), Drug =  drug1, Pharmacy = pharmacy},
                new PurchaseDetail{Quantity = 1, Price = new decimal(250), Drug = drug2, Pharmacy = pharmacy_2}
            };

            purchaseDetail_2 = new List<PurchaseDetail> {
                new PurchaseDetail{Quantity = 2, Price = new decimal(100), Drug =  drug3, Pharmacy = pharmacy},
                new PurchaseDetail{Quantity = 1, Price = new decimal(250), Drug = drug4, Pharmacy = pharmacy_2}
            };

            purchase = new Purchase
            {
                BuyerEmail = "roberto.perez@gmail.com",
                PurchaseDate = new DateTime(2022, 09, 19, 14, 34, 44),
                TotalAmount = new decimal(450),
                details = purchaseDetail
            };

            purchase_2 = new Purchase
            {
                BuyerEmail = "carlos.perez@gmail.com",
                PurchaseDate = new DateTime(2022, 10, 19, 14, 34, 44),
                TotalAmount = new decimal(450),
                details = purchaseDetail_2
            };


        }

        [TestCleanup]
        public void CleanUp()
        {

            using (var context = new PharmacyGoDbContext(options))
            {
                _purchasesRepository = new PurchasesRepository(context);
                if (_purchasesRepository.Exists(purchase))
                {
                    _purchasesRepository.DeleteOne(purchase);
                    _purchasesRepository.Save();
                }

                if (_purchasesRepository.Exists(purchase_2))
                {
                    _purchasesRepository.DeleteOne(purchase_2);
                    _purchasesRepository.Save();
                }
            }
        }
        
        [TestMethod]
        public void TestInsertOne()
        {

            using (var context = new PharmacyGoDbContext(options))
            {
                _purchasesRepository = new PurchasesRepository(context);
                if (!_purchasesRepository.Exists(purchase))
                {
                    _purchasesRepository.InsertOne(purchase);
                    _purchasesRepository.Save();
                }
                Assert.IsTrue(_purchasesRepository.Exists(purchase));
            }
        }

        [TestMethod]
        public void Get_All_By_Expression()
        {
            var purchaseList = new List<Purchase>();
            purchaseList.Add(purchase);
            purchaseList.Add(purchase_2);

            DateTime first = new DateTime(2022, 09, 19, 0, 0, 0);
            DateTime last = new DateTime(2022, 09, 25, 23, 59, 59);

            Expression<Func<Purchase, bool>> purchaseFilter = (purchase =>
                purchase.PurchaseDate <= last && purchase.PurchaseDate >= first);

            using (var context = new PharmacyGoDbContext(options))
            {
                _purchasesRepository = new PurchasesRepository(context);
                if (!_purchasesRepository.Exists(purchase))
                {
                    _purchasesRepository.InsertOne(purchase);
                    _purchasesRepository.Save();
                }

                if (!_purchasesRepository.Exists(purchase_2))
                {
                    _purchasesRepository.InsertOne(purchase_2);
                    _purchasesRepository.Save();
                }

                var purchasesList = _purchasesRepository.GetAllByExpression(purchaseFilter);

                // Assert
                Assert.AreEqual(purchasesList.Count(), 1);
            }
        }

        [TestMethod]
        public void Get_All_Purchases()
        {

            var purchaseList = new List<Purchase>();
            purchaseList.Add(purchase);
            purchaseList.Add(purchase_2);

            Expression<Func<Purchase, bool>> purchaseFilter = (purchase => purchase.Id != 0);

            using (var context = new PharmacyGoDbContext(options))
            {
                _purchasesRepository = new PurchasesRepository(context);
                if (!_purchasesRepository.Exists(purchase))
                {
                    _purchasesRepository.InsertOne(purchase);
                    _purchasesRepository.Save();
                }

                if (!_purchasesRepository.Exists(purchase_2))
                {
                    _purchasesRepository.InsertOne(purchase_2);
                    _purchasesRepository.Save();
                }

                var purchasesList = _purchasesRepository.GetAllBasicByExpression(purchaseFilter);

                // Assert
                Assert.AreEqual(purchasesList.Count(), 2);
            }
        }


        [TestMethod]
        public void Get_One_Detail_By_Expression()
        {
            var purchaseList = new List<Purchase>();
            purchase.TrackingCode = "f0c4ca1b-d7a8-4cf7-8eed-b6cfdce557cd";
            purchaseList.Add(purchase);
            purchaseList.Add(purchase_2);
            string token = "f0c4ca1b-d7a8-4cf7-8eed-b6cfdce557cd";

            using (var context = new PharmacyGoDbContext(options))
            {
                _purchasesRepository = new PurchasesRepository(context);
                if (!_purchasesRepository.Exists(purchase))
                {
                    _purchasesRepository.InsertOne(purchase);
                    _purchasesRepository.Save();
                }

                if (!_purchasesRepository.Exists(purchase_2))
                {
                    _purchasesRepository.InsertOne(purchase_2);
                    _purchasesRepository.Save();
                }

                var p_ = _purchasesRepository.GetOneDetailByExpression(p => p.TrackingCode == token);

                // Assert
                Assert.IsNotNull(p_);
            }
        }
    }
}
