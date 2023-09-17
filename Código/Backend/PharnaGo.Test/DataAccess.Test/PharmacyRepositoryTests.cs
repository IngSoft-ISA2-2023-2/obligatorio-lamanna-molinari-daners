using Microsoft.EntityFrameworkCore;
using PharmaGo.DataAccess;
using PharmaGo.DataAccess.Repositories;
using PharmaGo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PharmaGo.Test.DataAccess.Test
{
    [TestClass]
    public class PharmacyRepositoryTests
    {
        private PharmacyGoDbContext context;
        private List<Pharmacy> pharmaciesSaved;
        private PharmacyRepository _pharmacyRepository;
        private const int invalidId = 100;
        private Pharmacy newPharmacy;

        [TestInitialize]
        public void InitTest()
        {
            newPharmacy = new Pharmacy()
            {
                Id = 20,
                Name = $"newName",
                Address = $"newAddress",
                Drugs = new List<Drug>(),
                Users = new List<User>()
            };
        }

        [TestCleanup]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }

        private void CreateDataBase(string name)
        {
            pharmaciesSaved = CreateDummyPharmacies();
            var options = new DbContextOptionsBuilder<PharmacyGoDbContext>().UseInMemoryDatabase(databaseName: name).Options;
            context = new PharmacyGoDbContext(options);
            pharmaciesSaved.ForEach(p => context.Set<Pharmacy>().Add(p));
            context.SaveChanges();
            _pharmacyRepository = new PharmacyRepository(context);
        }

        [TestMethod]
        public void GetAllOk()
        {
            CreateDataBase("getPharmacysTestDb");
            var retrievedPharmacys = _pharmacyRepository.GetAllByExpression(p => p.Id > 0);
            Assert.AreEqual(10, retrievedPharmacys.Count());
        }

        [TestMethod]
        public void GetEmptyPharmacysOk()
        {
            CreateDataBase("getPharmacyEmptyTestDb");
            context.Set<Pharmacy>().RemoveRange(pharmaciesSaved);
            context.SaveChanges();
            var retrievedPharmacies = _pharmacyRepository.GetAllByExpression(p => p.Name != "");
            Assert.AreEqual(0, retrievedPharmacies.Count());
        }

        [TestMethod]
        public void GetPharmacysByIdOk()
        {
            CreateDataBase("getPharmacysByIdTestDb");
            Pharmacy pharmacy = pharmaciesSaved[0];
            var retrievedPharmacy = _pharmacyRepository.GetOneByExpression(p => p.Id == pharmacy.Id);
            Assert.AreEqual(pharmacy.Name, retrievedPharmacy.Name);
        }

        [TestMethod]
        public void GetPharmacysByIdNotExists()
        {
            CreateDataBase("getPharmacysByIdNotExistTestDb");
            var retrievedPharmacy = _pharmacyRepository.GetOneByExpression(p => p.Id == invalidId);
            Assert.AreEqual(null, retrievedPharmacy);
        }

        [TestMethod]
        public void InsertPharmacyOk()
        {
            CreateDataBase("insertPharmacyTestDb");
            _pharmacyRepository.InsertOne(newPharmacy);
            _pharmacyRepository.Save();
            var retrievedPharmacy = _pharmacyRepository.GetOneByExpression(p => p.Id == newPharmacy.Id);
            Assert.AreEqual(retrievedPharmacy.Name, newPharmacy.Name);
        }

        [TestMethod]
        public void DeletePharmacyOk()
        {
            CreateDataBase("deletePharmacyTestDb");
            Pharmacy toDelete = pharmaciesSaved[0];
            _pharmacyRepository.DeleteOne(toDelete);
            _pharmacyRepository.Save();
            var pharmacysReturned = _pharmacyRepository.GetAllByExpression(p => p.Id == toDelete.Id);
            Assert.IsTrue(pharmacysReturned.Count() == 0);
        }

        [TestMethod]
        public void UpdatePharmacyOk()
        {
            CreateDataBase("updatePharmacyTestDb");
            var pharmacyFromDb = _pharmacyRepository.GetOneByExpression(p => p.Name == pharmaciesSaved[0].Name);
            var dbName = pharmacyFromDb.Name;
            pharmacyFromDb.Name = "updatedName";
            _pharmacyRepository.UpdateOne(pharmacyFromDb);
            _pharmacyRepository.Save();
            var pharmacyUpdated = _pharmacyRepository.GetOneByExpression(p => p.Name == pharmaciesSaved[0].Name);
            Assert.AreNotEqual(dbName, pharmacyUpdated.Name);
        }

        private List<Pharmacy> CreateDummyPharmacies()
        {
            var pharmacyList = new List<Pharmacy>();
            for (int i = 1; i < 11; i++)
            {
                pharmacyList.Add(new Pharmacy()
                {
                    Id = i,
                    Name = $"name{i}",
                    Address = $"address{i}",
                    Drugs = new List<Drug>(),
                    Users = new List<User>()
                });
            }
            return pharmacyList;
        }
    }
}
