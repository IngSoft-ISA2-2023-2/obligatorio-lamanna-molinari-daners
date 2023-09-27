
using Microsoft.EntityFrameworkCore;
using PharmaGo.DataAccess.Repositories;
using PharmaGo.DataAccess;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.Enums;

namespace PharmaGo.Test.DataAccess.Test
{
    [TestClass]
    public class PurchasesDetailRepositoryTest
    {

        private PurchasesDetailRepository _purchasesDetailRepository;
        private Pharmacy pharmacy;
        private Pharmacy pharmacy_2;
        private PurchaseDetail purchaseDetail;
        private PurchaseDetail purchaseDetail_2;
        private Drug drug1;
        private Drug drug2;
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

            purchaseDetail = new PurchaseDetail { Quantity = 2, Price = new decimal(100), Drug = drug1, Pharmacy = pharmacy };
            purchaseDetail_2 = new PurchaseDetail { Quantity = 1, Price = new decimal(250), Drug = drug2, Pharmacy = pharmacy_2 };
        }

        [TestCleanup]
        public void CleanUp()
        {
            using (var context = new PharmacyGoDbContext(options))
            {
                _purchasesDetailRepository = new PurchasesDetailRepository(context);
                if (_purchasesDetailRepository.Exists(purchaseDetail))
                {
                    _purchasesDetailRepository.DeleteOne(purchaseDetail);
                    _purchasesDetailRepository.Save();
                }
            }
        }

        [TestMethod]
        public void Test_InsertOne()
        {
            using (var context = new PharmacyGoDbContext(options))
            {
                _purchasesDetailRepository = new PurchasesDetailRepository(context);
                if (!_purchasesDetailRepository.Exists(purchaseDetail))
                {
                    _purchasesDetailRepository.InsertOne(purchaseDetail);
                    _purchasesDetailRepository.Save();
                }
                Assert.IsTrue(_purchasesDetailRepository.Exists(purchaseDetail));
            }
        }

        [TestMethod]
        public void Test_GetOneByExpression()
        {
            using (var context = new PharmacyGoDbContext(options))
            {
                _purchasesDetailRepository = new PurchasesDetailRepository(context);
                if (!_purchasesDetailRepository.Exists(purchaseDetail))
                {
                    _purchasesDetailRepository.InsertOne(purchaseDetail);
                    _purchasesDetailRepository.Save();
                }

                //Act
                var p_ = _purchasesDetailRepository.GetOneByExpression(p => p.Id == purchaseDetail.Id);

                // Assert
                Assert.IsNotNull(p_);
            }
        }
    }
}
