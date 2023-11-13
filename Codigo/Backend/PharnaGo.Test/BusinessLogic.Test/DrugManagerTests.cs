using ExportationModel.ExportDomain;
using Moq;
using PharmaGo.BusinessLogic;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;
using PharmaGo.Exceptions;
using PharmaGo.IDataAccess;
using PharmaGo.WebApi.Models.In;
using PharmaGo.WebApi.Models.Out;
using System.Linq.Expressions;

namespace PharmaGo.Test.BusinessLogic.Test
{
    [TestClass]
    public class DrugManagerTests
    {
        private Mock<IRepository<Drug>> _drugRepository;
        private Mock<IRepository<Pharmacy>> _pharmacyRepository;
        private Mock<IRepository<UnitMeasure>> _unitMeasureRepository;
        private Mock<IRepository<Presentation>> _presentationRepository;
        private Mock<IRepository<User>> _userRepository;
        private Mock<IRepository<Session>> _sessionRepository;
        private DrugManager _drugManager;
        private DrugSearchCriteria drugSearch;
        private Drug drug;
        private const Drug nullDrug = null;
        private DrugModel drugModel;
        private DrugBasicModel drugBasicModel;
        private Pharmacy pharmacy;
        private const Pharmacy nullPharmacy = null;
        private UnitMeasure unitMeasure;
        private UnitMeasure nullUnitMeasure = null;
        private Presentation presentation;
        private Presentation nullPresentation = null;
        private string token = "c80da9ed-1b41-4768-8e34-b728cae25d2f";
        private Session session = null;
        private User user = null;

        [TestInitialize]
        public void InitTest()
        {
            _drugRepository = new Mock<IRepository<Drug>>();
            _pharmacyRepository = new Mock<IRepository<Pharmacy>>();
            _unitMeasureRepository = new Mock<IRepository<UnitMeasure>>();
            _presentationRepository = new Mock<IRepository<Presentation>>();
            _userRepository = new Mock<IRepository<User>>();
            _sessionRepository = new Mock<IRepository<Session>>();
            _drugManager = new DrugManager(_drugRepository.Object, 
                _pharmacyRepository.Object, 
                _unitMeasureRepository.Object, 
                _presentationRepository.Object, 
                _sessionRepository.Object, 
                _userRepository.Object);
            pharmacy = new Pharmacy() { Id = 1, Name = "pharmacy", Address = "address", Users = new List<User>() };
            drugSearch = new DrugSearchCriteria { PharmacyId = pharmacy.Id, Name = "drugName" };
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
            drugModel = new DrugModel()
            {
                Code = "code",
                Name = "name",
                Prescription = false,
                Price = 200,
                Quantity = 30,
                Symptom = "symptom",
                PharmacyName = "pharmacy"
            };
            drugBasicModel = new DrugBasicModel(drug);
            unitMeasure = new UnitMeasure()
            {
                Id = 1,
                Name = "ml",
                Deleted = false
            };
            presentation = new Presentation()
            {
                Id = 1,
                Name = "capsules",
                Deleted = false
            };
            session = new Session {Id = 1, Token = new Guid(token), UserId = 1 };
            user = new User() {Id = 1, UserName = "test", Email = "test@gmail.com", Address = "test" };
        }

        [TestCleanup]
        public void Cleanup()
        {
            _drugRepository.VerifyAll();
        }

        [TestMethod]
        public void GetDrugsOk()
        {
            IEnumerable<Drug> drugList = GenerateDrugList();
            _drugRepository.Setup(r => r.GetAllByExpression(It.IsAny<Expression<Func<Drug, bool>>>())).Returns(drugList);
            _pharmacyRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns(pharmacy);
            var drugsReturned = _drugManager.GetAll(drugSearch);
            _drugRepository.VerifyAll();
            Assert.AreEqual(drugsReturned, drugList);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void GetDrugsOfInexistentPharmacy()
        {
            IEnumerable<Drug> drugList = GenerateDrugList();
            _pharmacyRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns(nullPharmacy);
            var drugsReturned = _drugManager.GetAll(drugSearch);
            _drugRepository.VerifyAll();
            Assert.AreEqual(drugsReturned, drugList);
        }

        [TestMethod]
        public void GetDrugByIdOk()
        {
            _drugRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Drug, bool>>>())).Returns(drug);
            var drugReturned = _drugManager.GetById(drug.Id);
            _drugRepository.VerifyAll();
            Assert.AreEqual(drug, drugReturned);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void GetDrugByIdNotExists()
        {
            _drugRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Drug, bool>>>())).Returns(nullDrug);
            var drugReturned = _drugManager.GetById(drug.Id);
            _drugRepository.VerifyAll();
        }

        [TestMethod]
        public void CreateDrugOk()
        {
            _drugRepository.Setup(r => r.Exists(It.IsAny<Expression<Func<Drug, bool>>>())).Returns(false);
            _pharmacyRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns(pharmacy);
            _unitMeasureRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<UnitMeasure, bool>>>())).Returns(unitMeasure);
            _presentationRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Presentation, bool>>>())).Returns(presentation);
            _sessionRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>())).Returns(session);
            _userRepository.Setup(r => r.GetOneDetailByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);

            _drugRepository.Setup(x => x.InsertOne(It.IsAny<Drug>()));
            _drugRepository.Setup(x => x.Save());
            
            var drugReturned = _drugManager.Create(drugModel.ToEntity(), token);

            // Assert
            _drugRepository.VerifyAll();
            Assert.AreNotEqual(drugReturned.Id, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void CreateDrugWithExistentCode()
        {
            _sessionRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>())).Returns(session);
            _userRepository.Setup(r => r.GetOneDetailByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);
            var drugReturned = _drugManager.Create(drug, token);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void CreateNullDrug()
        {
            var drugReturned = _drugManager.Create(null, token);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void CreateDrugNullPharmacy()
        {
            _sessionRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>())).Returns(session);
            _userRepository.Setup(r => r.GetOneDetailByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);
            _pharmacyRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns(nullPharmacy);
            var drugReturned = _drugManager.Create(drug, token);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void CreateDrugInvalidUnitMeasure()
        {
            _sessionRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>())).Returns(session);
            _userRepository.Setup(r => r.GetOneDetailByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);
            _pharmacyRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns(pharmacy);
            _unitMeasureRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<UnitMeasure, bool>>>())).Returns(nullUnitMeasure);
           
            var drugReturned = _drugManager.Create(drugModel.ToEntity(), token);
            _drugRepository.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void CreateDrugInvalidPresentation()
        {
            _sessionRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>())).Returns(session);
            _userRepository.Setup(r => r.GetOneDetailByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);
            _pharmacyRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns(pharmacy);
            _unitMeasureRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<UnitMeasure, bool>>>())).Returns(unitMeasure);
            _presentationRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Presentation, bool>>>())).Returns(nullPresentation);
            var drugReturned = _drugManager.Create(drugModel.ToEntity(), token);
            _drugRepository.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateDrugEmptyCode()
        {
            drug.Code = "";
            var drugReturned = _drugManager.Create(drug, token);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateDrugNullCode()
        {
            drug.Code = null;
            var drugReturned = _drugManager.Create(drug, token);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateDrugEmptyName()
        {
            drug.Name = "";
            var drugReturned = _drugManager.Create(drug, token);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateDrugNullName()
        {
            drug.Name = null;
            var drugReturned = _drugManager.Create(drug, token);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateDrugEmptySymptom()
        {
            drug.Symptom = "";
            var drugReturned = _drugManager.Create(drug, token);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateDrugNullSymptom()
        {
            drug.Symptom = null;
            var drugReturned = _drugManager.Create(drug, token);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateDrugInvalidQuantity()
        {
            drug.Quantity = 0;
            var drugReturned = _drugManager.Create(drug, token);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateDrugInvalidPrice()
        {
            drug.Price = 0;
            var drugReturned = _drugManager.Create(drug, token);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateDrugInvalidStock()
        {
            drug.Stock = -1;
            var drugReturned = _drugManager.Create(drug, token);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateDrugNullPresentation()
        {
            drug.Presentation = null;
            var drugReturned = _drugManager.Create(drug, token);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidResourceException))]
        public void CreateDrugNullUnitMeasure()
        {
            drug.UnitMeasure = null;
            var drugReturned = _drugManager.Create(drug, token);
        }

        [TestMethod]
        public void UpdateDrugOk()
        {
            Drug updatedDrug = drugModel.ToEntity();
            _pharmacyRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Pharmacy, bool>>>())).Returns(pharmacy);
            _drugRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Drug, bool>>>())).Returns(drug);
            _drugRepository.Setup(x => x.UpdateOne(drug));
            _drugRepository.Setup(x => x.Save());
            var drugReturned = _drugManager.Update(drug.Id, updatedDrug);
            _drugRepository.VerifyAll();
            Assert.AreEqual(drugReturned, drug);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void UpdateNullDrug()
        {
            var drugReturned = _drugManager.Update(drug.Id,nullDrug);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void UpdateInexistentDrug()
        {
            _drugRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Drug, bool>>>())).Returns(nullDrug);
            var drugReturned = _drugManager.Update(drug.Id, drug);
        }

        [TestMethod]
        public void DeleteDrugOk()
        {
            _drugRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Drug, bool>>>())).Returns(drug);
            _drugRepository.Setup(x => x.UpdateOne(drug));
            _drugRepository.Setup(x => x.Save());
            _drugManager.Delete(drug.Id);
            _drugRepository.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void DeleteNullDrug()
        {
            _drugRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Drug, bool>>>())).Returns(nullDrug);
            _drugManager.Delete(drug.Id);
            _drugRepository.VerifyAll();
        }

        [TestMethod]
        public void GetDrugsToExportOk()
        {
            _sessionRepository.Setup(r => r.GetOneByExpression(It.IsAny<Expression<Func<Session, bool>>>())).Returns(session);
            _userRepository.Setup(r => r.GetOneDetailByExpression(It.IsAny<Expression<Func<User, bool>>>())).Returns(user);
            List<Drug> drugList = GenerateDrugList() as List<Drug>;
            
            _drugRepository.Setup(r => r.GetAllByExpression(It.IsAny<Expression<Func<Drug, bool>>>())).Returns(drugList);
            var drugsToExport = (List<DrugExportationModel>)_drugManager.GetDrugsToExport(token);
            
            // Assert
            _drugRepository.VerifyAll();
            for (int i = 0; i < drugsToExport.Count(); i++)
            {
                Assert.AreEqual(drugsToExport[i].Code, drugList[i].Code);
                Assert.AreEqual(drugsToExport[i].Name, drugList[i].Name);
                Assert.AreEqual(drugsToExport[i].Symptom, drugList[i].Symptom);
            }
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
    }
}
