using Microsoft.AspNetCore.Mvc;
using PharmaGo.Domain.Entities;
using PharmaGo.Exceptions;
using PharmaGo.IBusinessLogic;
using PharmaGo.WebApi.Enums;
using PharmaGo.WebApi.Filters;
using PharmaGo.WebApi.Models.In;
using PharmaGo.WebApi.Models.Out;

namespace PharmaGo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockRequestController : ControllerBase
    {
        private readonly IStockRequestManager _stockRequestManager;

        public StockRequestController(IStockRequestManager manager)
        {
            _stockRequestManager = manager;
        }

        [HttpPost]
        [AuthorizationFilter(new string[] { nameof(RoleType.Employee) })]
        public IActionResult CreateStockRequest([FromBody] StockRequestModelRequest stockRequestModel)
        {
            string token = HttpContext.Request.Headers["Authorization"];
            var stockRequest = _stockRequestManager.CreateStockRequest(stockRequestModel.ToEntity(), token);
            return Ok(new StockRequestModelResponse(stockRequest));
        }

        [HttpPut]
        [AuthorizationFilter(new string[] { nameof(RoleType.Owner) })]
        [Route("[action]/{id}")]
        public IActionResult ApproveStockRequest(int id)
        {
            var approved = _stockRequestManager.ApproveStockRequest(id);
            return Ok(approved);
        }

        [HttpPut]
        [Route("[action]/{id}")]
        [AuthorizationFilter(new string[] { nameof(RoleType.Owner) })]
        public IActionResult RejectStockRequest(int id)
        {
            var approved = _stockRequestManager.RejectStockRequest(id);
            return Ok(approved);
        }

        [HttpGet]
        [Route("[action]")]
        [AuthorizationFilter(new string[] { nameof(RoleType.Employee) })]
        public IActionResult ByEmployee([FromQuery] StockRequestSearchCriteriaModelRequest searchCriteria)
        {
            string token = HttpContext.Request.Headers["Authorization"];
            var stockRequests = _stockRequestManager.GetStockRequestsByEmployee(token, searchCriteria.ToEntity());

            IEnumerable<StockRequestSearchCriteriaModelResponse> result = stockRequests.Select(d => new StockRequestSearchCriteriaModelResponse(d));
            return Ok(result);
        }

        [HttpGet]
        [AuthorizationFilter(new string[] { nameof(RoleType.Owner) })]
        public IActionResult GetAll()
        {
            string token = HttpContext.Request.Headers["Authorization"];
            var stockRequests = _stockRequestManager.GetStockRequestsByOwner(token);

            IEnumerable<StockRequestSearchCriteriaModelResponse> result = stockRequests.Select(d => new StockRequestSearchCriteriaModelResponse(d));
            return Ok(result);
        }
    }
}
