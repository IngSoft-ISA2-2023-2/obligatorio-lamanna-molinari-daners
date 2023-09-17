using Moq;
using PharmaGo.BusinessLogic;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;
using PharmaGo.Exceptions;
using PharmaGo.IDataAccess;
using PharmaGo.WebApi.Models.In;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PharmaGo.Test.BusinessLogic.Test
{
    [TestClass]
    public class PharmacyManagerTests
    {
        private PharmacyManager _pharmacyManager;
        private Mock<IRepository<Pharmacy>> _pharmacyRepository;
        private PharmacySearchCriteria pharmacySearch;
        private Pharmacy pharmacy;
        private Pharmacy nullPharmacy = null;
        private const string InvalidPharmacyName = "This is an invalid large name for a pharmacy with more than 50 characters.";

        [TestInitialize]
        public void InitTest()
        {
            _pharmacyRepository = new Mock<IRepository<Pharmacy>>();
            _pharmacyManager = new PharmacyManager(_pharmacyRepository.Object);
            pharmacySearch = new PharmacySearchCriteria();
            pharmacy = new Pharmacy()
            {
                Id = 50,
                Name = "newPharmacy",
                Address = "newAddress",
                Users = new List<User>(),
                Drugs = new List<Drug>()
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            _pharmacyRepository.VerifyAll();
        }

        [TestMethod]
        public void GetPharmaciesOk()
        {
            IEnumerable<Pharmacy> pharmacyList = GeneratePharmacyList();
            _pharmacyRepository.Setup(x => x.GetAllByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns(pharmacyList);
            var pharmaciesReturned = _pharmacyManager.GetAll(pharmacySearch);
            _pharmacyRepository.VerifyAll();
            Assert.AreEqual(pharmaciesReturned, pharmacyList);
        }

        [TestMethod]
        public void GetPharmacyByIdOk()
        {
            _pharmacyRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns(pharmacy);
            var pharmacyReturned = _pharmacyManager.GetById(pharmacy.Id);
            _pharmacyRepository.VerifyAll();
            Assert.AreEqual(pharmacy, pharmacyReturned);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void GetPharmacyByIdNotExists()
        {
            _pharmacyRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns(nullPharmacy);
            var pharmacyReturned = _pharmacyManager.GetById(pharmacy.Id);
            _pharmacyRepository.VerifyAll();
        }

        [TestMethod]
        public void CreatePharmacyOk()
        {
            _pharmacyRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns(nullPharmacy);
            _pharmacyRepository.Setup(x => x.InsertOne(It.IsAny<Pharmacy>()));
            _pharmacyRepository.Setup(x => x.Save());
            var pharmacyReturned = _pharmacyManager.Create(pharmacy);
            _pharmacyRepository.VerifyAll();
            Assert.AreEqual(pharmacyReturned.Id, pharmacy.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateExistentPharmacy()
        {
            _pharmacyRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns(pharmacy);
            var pharmacyReturned = _pharmacyManager.Create(pharmacy);
            _pharmacyRepository.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateNullPharmacy()
        {
            var pharmacyReturned = _pharmacyManager.Create(nullPharmacy);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateEmptyNamePharmacy()
        {
            pharmacy.Name = "";
            var pharmacyReturned = _pharmacyManager.Create(pharmacy);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateLargeNamePharmacy()
        {
            pharmacy.Name = InvalidPharmacyName;
            var pharmacyReturned = _pharmacyManager.Create(pharmacy);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateEmptyAddressPharmacy()
        {
            pharmacy.Address = "";
            var pharmacyReturned = _pharmacyManager.Create(pharmacy);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateNullNamePharmacy()
        {
            pharmacy.Name = null;
            var pharmacyReturned = _pharmacyManager.Create(pharmacy);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateNullAddressPharmacy()
        {
            pharmacy.Address = null;
            var pharmacyReturned = _pharmacyManager.Create(pharmacy);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateNullDrugsPharmacy()
        {
            pharmacy.Drugs = null;
            var pharmacyReturned = _pharmacyManager.Create(pharmacy);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateNullUsersPharmacy()
        {
            pharmacy.Users = null;
            var pharmacyReturned = _pharmacyManager.Create(pharmacy);
        }

        [TestMethod]
        public void UpdatePharmacyOk()
        {
            Pharmacy updatedPharmacy = new Pharmacy() { Name = "PharmacyName", Address = "PharmacyAddress", Users = new List<User>(), Drugs = new List<Drug>() };
            _pharmacyRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns(pharmacy);
            _pharmacyRepository.Setup(x => x.UpdateOne(pharmacy));
            _pharmacyRepository.Setup(x => x.Save());
            var pharmacyReturned = _pharmacyManager.Update(pharmacy.Id, updatedPharmacy);
            _pharmacyRepository.VerifyAll();
            Assert.AreNotEqual(pharmacyReturned.Id, updatedPharmacy.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void UpdateNullPharmacy()
        {
            var pharmacyReturned = _pharmacyManager.Update(pharmacy.Id, nullPharmacy);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void UpdateInvalidPharmacy()
        {
            pharmacy.Name = "";
            var pharmacyReturned = _pharmacyManager.Update(pharmacy.Id, pharmacy);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void UpdateLargeNamePharmacy()
        {
            pharmacy.Name = InvalidPharmacyName;
            var pharmacyReturned = _pharmacyManager.Update(pharmacy.Id, pharmacy);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void UpdateNotExistentPharmacy()
        {
            _pharmacyRepository.Setup(x => x.GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns(nullPharmacy);
            var pharmacyReturned = _pharmacyManager.Update(pharmacy.Id, pharmacy);
        }

        [TestMethod]
        public void DeletePharmacyOk()
        {
            _pharmacyRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns(pharmacy);
            _pharmacyRepository.Setup(x => x.DeleteOne(pharmacy));
            _pharmacyRepository.Setup(x => x.Save());
            _pharmacyManager.Delete(pharmacy.Id);
            _pharmacyRepository.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void DeleteNotExistentPharmacy()
        {
            _pharmacyRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns(nullPharmacy);
            _pharmacyManager.Delete(pharmacy.Id);
            _pharmacyRepository.VerifyAll();
        }

        private IEnumerable<Pharmacy> GeneratePharmacyList()
        {
            var pharmacyList = new List<Pharmacy>();
            for (int i = 1; i < 11; i++)
            {
                pharmacyList.Add(new Pharmacy()
                {
                    Id = i,
                    Name = $"name{i}",
                    Address = $"address{i}",
                    Users = new List<User>(),
                    Drugs = new List<Drug>()
                });
            }
            return pharmacyList;
        }
    }
}
 