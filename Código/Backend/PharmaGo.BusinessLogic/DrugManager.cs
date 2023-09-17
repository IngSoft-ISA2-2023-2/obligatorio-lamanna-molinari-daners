using ExportationModel.ExportDomain;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;
using PharmaGo.Exceptions;
using PharmaGo.IBusinessLogic;
using PharmaGo.IDataAccess;

namespace PharmaGo.BusinessLogic
{
    public class DrugManager : IDrugManager
    {
        private readonly IRepository<Drug> _drugRepository;
        private readonly IRepository<Pharmacy> _pharmacyRepository;
        private readonly IRepository<UnitMeasure> _unitMeasureRepository;
        private readonly IRepository<Presentation> _presentationRepository;
        private readonly IRepository<Session> _sessionRepository;
        private readonly IRepository<User> _userRepository;

        public DrugManager(IRepository<Drug> drugRepo, 
                           IRepository<Pharmacy> pharmacyRepository, 
                           IRepository<UnitMeasure> unitMeasureRepository, 
                           IRepository<Presentation> presentationRepository,
                           IRepository<Session> sessionRespository,
                           IRepository<User> userRespository)
        {
            _drugRepository = drugRepo;
            _pharmacyRepository = pharmacyRepository;
            _unitMeasureRepository = unitMeasureRepository;
            _presentationRepository = presentationRepository;
            _sessionRepository = sessionRespository;
            _userRepository = userRespository;
        }

        public IEnumerable<Drug> GetAll(DrugSearchCriteria drugSearchCriteria)
        {
            Drug drugToSearch = new Drug();
            if (drugSearchCriteria.PharmacyId == null)
            {
                drugToSearch.Name = drugSearchCriteria.Name;
            }
            else
            {
                Pharmacy pharmacySaved = _pharmacyRepository.GetOneByExpression(p => p.Id == drugSearchCriteria.PharmacyId);
                if(pharmacySaved != null)
                {
                    drugToSearch.Name = drugSearchCriteria.Name;
                    drugToSearch.Pharmacy = pharmacySaved;
                }
                else
                {
                    throw new ResourceNotFoundException("The pharmacy to get drugs of does not exist.");
                }
            }
            return _drugRepository.GetAllByExpression(drugSearchCriteria.Criteria(drugToSearch));
        }

        public Drug GetById(int id)
        {
            Drug retrievedDrug = _drugRepository.GetOneByExpression(d => d.Id == id);
            if (retrievedDrug == null)
            {
                throw new ResourceNotFoundException("The drug does not exist.");
            }

            return retrievedDrug;
        }

        public Drug Create(Drug drug, string token)
        {
            if (drug == null)
            {
                throw new ResourceNotFoundException("Please create a drug before inserting it.");
            }
            drug.ValidOrFail();

            var guidToken = new Guid(token);
            Session session = _sessionRepository.GetOneByExpression(s => s.Token == guidToken);
            var userId = session.UserId;
            User user = _userRepository.GetOneDetailByExpression(u => u.Id == userId);

            Pharmacy pharmacyOfDrug = _pharmacyRepository.GetOneByExpression(p => p.Name == user.Pharmacy.Name);
            if (pharmacyOfDrug == null)
            {
                throw new ResourceNotFoundException("The pharmacy of the drug does not exist.");
            }

            if (_drugRepository.Exists(d => d.Code == drug.Code && d.Pharmacy.Name == pharmacyOfDrug.Name))
            {
                throw new InvalidResourceException("The drug already exists in that pharmacy.");
            }
            UnitMeasure unitMeasureOfDrug = _unitMeasureRepository.GetOneByExpression(u => u.Id == drug.UnitMeasure.Id);
            if (unitMeasureOfDrug == null)
            {
                throw new ResourceNotFoundException("The unit measure of the drug does not exist.");
            }
            Presentation presentationOfDrug = _presentationRepository.GetOneByExpression(p => p.Id == drug.Presentation.Id);
            if (presentationOfDrug == null)
            {
                throw new ResourceNotFoundException("The presentation of the drug does not exist.");
            }

            drug.UnitMeasure.Id = unitMeasureOfDrug.Id;
            drug.UnitMeasure.Name = unitMeasureOfDrug.Name;
            drug.Presentation.Id = presentationOfDrug.Id;
            drug.Presentation.Name = presentationOfDrug.Name;
            drug.Pharmacy.Id = pharmacyOfDrug.Id;
            _drugRepository.InsertOne(drug);
            _drugRepository.Save();
            return drug;
        }

        public Drug Update(int id, Drug updatedDrug) 
        {
            if(updatedDrug == null)
            {
                throw new ResourceNotFoundException("The updated drug is invalid.");
            }
            updatedDrug.ValidOrFail();
            var drugSaved = _drugRepository.GetOneByExpression(d => d.Id == id);
            if(drugSaved == null)
            {
                throw new ResourceNotFoundException("The drug to update does not exist.");
            }
            drugSaved.Code = updatedDrug.Code;
            drugSaved.Name = updatedDrug.Name;
            drugSaved.Symptom = updatedDrug.Symptom;
            drugSaved.Quantity = updatedDrug.Quantity;
            drugSaved.Price = updatedDrug.Price;
            drugSaved.Prescription = updatedDrug.Prescription;
            _drugRepository.UpdateOne(drugSaved);
            _drugRepository.Save();
            return drugSaved;
        }

        public void Delete(int id)
        {
            var drugSaved = _drugRepository.GetOneByExpression(d => d.Id == id);
            if(drugSaved == null)
            {
                throw new ResourceNotFoundException("The drug to delete does not exist.");
            }
            drugSaved.Deleted = true;
            _drugRepository.UpdateOne(drugSaved);
            _drugRepository.Save();
        }

        public IEnumerable<DrugExportationModel> GetDrugsToExport(string token)
        {
            var guidToken = new Guid(token);
            Session session = _sessionRepository.GetOneByExpression(s => s.Token == guidToken);
            var userId = session.UserId;
            User user = _userRepository.GetOneDetailByExpression(u => u.Id == userId);
            Pharmacy pharmacyOfDrug = _pharmacyRepository.GetOneByExpression(p => p.Name == user.Pharmacy.Name);
            IEnumerable<Drug> drugsSaved = _drugRepository.GetAllByExpression(d => d.Name == d.Name && d.Pharmacy.Id == pharmacyOfDrug.Id);
            IEnumerable<DrugExportationModel> drugsToExport = drugsSaved.Select(d =>
                new DrugExportationModel
                {
                    Code = d.Code,
                    Name = d.Name,
                    Symptom = d.Symptom,
                    Deleted = d.Deleted
                }).ToList();
            return drugsToExport;
        }

        public IEnumerable<Drug> GetAllByUser(string token)
        {
            var guidToken = new Guid(token);
            Session session = _sessionRepository.GetOneByExpression(s => s.Token == guidToken);
            var userId = session.UserId;
            User user = _userRepository.GetOneDetailByExpression(u => u.Id == userId);
            Pharmacy pharmacy = user.Pharmacy;
            return _drugRepository.GetAllByExpression(d => d.Deleted == false && d.Pharmacy.Id == pharmacy.Id);
        }
    }
}
