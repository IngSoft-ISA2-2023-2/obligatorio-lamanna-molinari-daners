using Microsoft.EntityFrameworkCore;
using PharmaGo.DataAccess;
using PharmaGo.DataAccess.Repositories;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.Enums;
using PharmaGo.Domain.SearchCriterias;
using PharmaGo.WebApi.Models.Out;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaGo.Test.DataAccess.Test
{
    [TestClass]
    public class DrugRepositoryTests
    {
        private PharmacyGoDbContext context;
        private List<Drug> drugsSaved;
        private DrugRepository _drugRepository;
        private const int invalidId = 500;
        private Pharmacy pharmacy;
        private Drug newDrug;
        private const string invalidDrugCode = "invalidCode";

        [TestInitialize]
        public void InitTest()
        {
            drugsSaved = new List<Drug>();
            pharmacy = new Pharmacy() { Id = 1, Name = "pharmacy", Address = "address", Users = new List<User>() };
            newDrug = new Drug()
            {
                Code = "newdrugCode",
                Name = "newdrugName",
                Symptom = "pain",
                Quantity = 50,
                Price = 50,
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
                Pharmacy = new Pharmacy()
                {
                    Id = pharmacy.Id
                }
            };
        }

        [TestCleanup]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }

        private void CreateDataBase(string name)
        {
            drugsSaved = CreateDummyDrugs();
            var options = new DbContextOptionsBuilder<PharmacyGoDbContext>().UseInMemoryDatabase(databaseName: name).Options;
            context = new PharmacyGoDbContext(options);
            drugsSaved.ForEach(d => context.Set<Drug>().Add(d));
            context.Set<Drug>().Include("Pharmacy");
            context.SaveChanges();
            _drugRepository = new DrugRepository(context);
        }

        [TestMethod]
        public void GetAllDrugsOfPharmacyOk()
        {
            CreateDataBase("getDrugsTestDb");
            var retrievedDrugs = _drugRepository.GetAllByExpression(d => d.Stock == 0);
            Assert.AreEqual(10, retrievedDrugs.Count());
        }

        [TestMethod]
        public void GetEmptyDrugsOk()
        {
            CreateDataBase("getDrugsEmptyTestDb");
            context.Set<Drug>().RemoveRange(drugsSaved);
            context.SaveChanges();
            var retrievedDrugs = _drugRepository.GetAllByExpression(d => d.Pharmacy.Name == "pharmacy");
            Assert.AreEqual(0, retrievedDrugs.Count());
        }

        [TestMethod]
        public void GetDrugsByIdOk()
        {
            CreateDataBase("getDrugsByIdTestDb");
            Drug drug = drugsSaved[0];
            var retrievedDrug = _drugRepository.GetOneByExpression(d=> d.Id == drug.Id);
            Assert.AreEqual(drug.Code, retrievedDrug.Code);
        }

        [TestMethod]
        public void GetDrugsByIdNotExists()
        {
            CreateDataBase("getDrugsByIdNotExistTestDb");
            var retrievedDrug = _drugRepository.GetOneByExpression(d => d.Id == invalidId);
            Assert.AreEqual(null, retrievedDrug);
        }

        [TestMethod]
        public void InsertUserOk()
        {
            CreateDataBase("insertDrugTestDb");
            _drugRepository.InsertOne(newDrug);
            _drugRepository.Save();
            var retrievedDrug = _drugRepository.GetOneByExpression(d => d.Id == newDrug.Id);
            Assert.AreEqual(retrievedDrug.Code,newDrug.Code);
        }

        [TestMethod]
        public void ExistDrugOk()
        {
            CreateDataBase("ExistDrugTestDb");
            bool exists = _drugRepository.Exists(d => d.Code == drugsSaved[0].Code);
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void ExistDrugFalse()
        {
            CreateDataBase("ExistFalseDrugTestDb");
            bool exists = _drugRepository.Exists(d => d.Code == invalidDrugCode);
            Assert.IsFalse(exists);
        }

        [TestMethod]
        public void DeleteDrugOk()
        {
            CreateDataBase("deleteDrugTestDb");
            Drug toDelete = drugsSaved[0];
            _drugRepository.DeleteOne(toDelete);
            _drugRepository.Save();
            var drugsReturned = _drugRepository.GetAllByExpression(d => d.Code == toDelete.Code && d.Pharmacy.Name == toDelete.Pharmacy.Name);
            Assert.IsTrue(drugsReturned.Count() == 0);
        }

        [TestMethod]
        public void UpdateConcertOk()
        {
            CreateDataBase("updateDrugTestDb");
            var drugFromDb = _drugRepository.GetOneByExpression(d => d.Code == drugsSaved[0].Code);
            var dbName = drugFromDb.Name;
            drugFromDb.Name = "updatedName";
            _drugRepository.UpdateOne(drugFromDb);
            _drugRepository.Save();
            var drugUpdated = _drugRepository.GetOneByExpression(d => d.Code == drugsSaved[0].Code);
            Assert.AreNotEqual(dbName, drugUpdated.Name);
        }

        private List<Drug> CreateDummyDrugs()
        {
            Pharmacy pharmacy = new Pharmacy() { Id = 1, Name = "pharmacy", Address = "address", Users = new List<User>() };
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
                    UnitMeasure = new UnitMeasure { Id = i, Name = "g", Deleted = false },
                    Pharmacy = pharmacy
                });
            }
            return drugList;
        }
    }
}
