using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;
using PharmaGo.Exceptions;
using PharmaGo.IBusinessLogic;
using PharmaGo.IDataAccess;

namespace PharmaGo.BusinessLogic
{
    public class StockRequestManager : IStockRequestManager
    {
        private readonly IRepository<StockRequest> _stockRequestRepository;
        private readonly IRepository<User> _employeeRepository;
        private readonly IRepository<Drug> _drugRepository;
        private readonly IRepository<Session> _sessionRepository;

        public StockRequestManager(IRepository<StockRequest> stockRequestRepository,
            IRepository<User> employeeRepository, IRepository<Drug> drugRepository, IRepository<Session> sessionRepository)
		{
            _stockRequestRepository = stockRequestRepository;
            _employeeRepository = employeeRepository;
            _drugRepository = drugRepository;
            _sessionRepository = sessionRepository;
        }

        public bool ApproveStockRequest(int id)
        {
            var stockRequest = _stockRequestRepository.GetOneByExpression(s => s.Id == id);
            if (stockRequest == null) throw new InvalidResourceException("Invalid stock request.");
            if (stockRequest != null)
            {
                if (stockRequest.Status == Domain.Enums.StockRequestStatus.Approved) throw new InvalidResourceException("Stock request already approved.");
                if (stockRequest.Status == Domain.Enums.StockRequestStatus.Rejected) throw new InvalidResourceException("Stock request already rejected.");
            }

            foreach (var stockRequestDetail in stockRequest.Details)
            {
                if (stockRequestDetail.Drug.Deleted == true) throw new InvalidResourceException("Stock request has deleted drugs included.");
                var drug = _drugRepository.GetOneByExpression(d => d.Id == stockRequestDetail.Drug.Id);
                drug.Stock += stockRequestDetail.Quantity;
                _drugRepository.UpdateOne(drug);
            }

            stockRequest.Status = Domain.Enums.StockRequestStatus.Approved;
            _stockRequestRepository.UpdateOne(stockRequest);

            _drugRepository.Save();
            _stockRequestRepository.Save();

            return true;
        }

        public bool RejectStockRequest(int id)
        {
            var stockRequest = _stockRequestRepository.GetOneByExpression(s => s.Id == id);
            if (stockRequest == null) throw new InvalidResourceException("Invalid stock request.");
            if (stockRequest != null)
            {
                if (stockRequest.Status == Domain.Enums.StockRequestStatus.Approved) throw new InvalidResourceException("Stock request already approved.");
                if (stockRequest.Status == Domain.Enums.StockRequestStatus.Rejected) throw new InvalidResourceException("Stock request already rejected.");
            }  

            stockRequest.Status = Domain.Enums.StockRequestStatus.Rejected;
            _stockRequestRepository.UpdateOne(stockRequest);
            _stockRequestRepository.Save();

            return true;
        }

        public StockRequest CreateStockRequest(StockRequest stockRequest, string token)
        {
            User existEmployee = null;
            if (stockRequest.Details == null) throw new InvalidResourceException("Invalid stock details.");
            if (stockRequest.Details.Count == 0) throw new InvalidResourceException("Invalid stock details.");

            var session = _sessionRepository.GetOneByExpression(session => session.Token == new Guid(token));
            if (session == null) throw new InvalidResourceException("Invalid session from employee.");
            var userId = session.UserId;

            existEmployee = _employeeRepository.GetOneDetailByExpression(u => u.Id == userId);
            if (existEmployee == null) throw new InvalidResourceException("Invalid employee.");
            stockRequest.Employee = existEmployee;

            foreach (StockRequestDetail item in stockRequest.Details)
            {
                var drug = _drugRepository.GetOneByExpression(d => d.Code == item.Drug.Code && d.Pharmacy.Id == existEmployee.Pharmacy.Id);
                if (drug == null) throw new InvalidResourceException("Stock request has invalid drug.");

                item.Drug = drug;
            }

            stockRequest.Status = Domain.Enums.StockRequestStatus.Pending;
            _stockRequestRepository.InsertOne(stockRequest);
            _stockRequestRepository.Save();

            return stockRequest;
        }

        public IEnumerable<StockRequest> GetStockRequestsByEmployee(string token, StockRequestSearchCriteria searchCriteria)
        {
            if (string.IsNullOrEmpty(token.ToString())) throw new InvalidResourceException("Invalid employee.");
            var session = _sessionRepository.GetOneByExpression(session => session.Token == new Guid(token));
            if (session == null) throw new InvalidResourceException("Invalid employee.");
            searchCriteria.EmployeeId = session.UserId;

            var stockRequests = _stockRequestRepository.GetAllBasicByExpression(searchCriteria.Criteria());

            return stockRequests;
        }

        public IEnumerable<StockRequest> GetStockRequestsByOwner(string token) {

            if (string.IsNullOrEmpty(token.ToString())) throw new InvalidResourceException("Invalid owner.");
            var session = _sessionRepository.GetOneByExpression(session => session.Token == new Guid(token));
            if (session == null) throw new ResourceNotFoundException("Invalid owner.");

            var userId = session.UserId;
            User user = _employeeRepository.GetOneDetailByExpression(u => u.Id == userId);
            if (user == null) throw new ResourceNotFoundException("Invalid user.");
            var pharmacyId = user.Pharmacy.Id;
            var stockRequests = _stockRequestRepository.GetAllBasicByExpression(s => s.Employee.Pharmacy.Id == pharmacyId);

            return stockRequests;

        }
    }
}

