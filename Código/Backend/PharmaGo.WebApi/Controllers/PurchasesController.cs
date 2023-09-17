using Microsoft.AspNetCore.Mvc;
using PharmaGo.IBusinessLogic;
using PharmaGo.WebApi.Converters;
using PharmaGo.WebApi.Enums;
using PharmaGo.WebApi.Filters;
using PharmaGo.WebApi.Models.In;
using PharmaGo.WebApi.Models.Out;

namespace PharmaGo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchasesManager _purchasesManager;

        public PurchasesController(IPurchasesManager manager)
        {
            _purchasesManager = manager;
        }

        [HttpGet]
        [AuthorizationFilter(new string[] { nameof(RoleType.Employee) })]
        public IActionResult All()
        {
            string token = HttpContext.Request.Headers["Authorization"];
            var retrievedPuerchases = _purchasesManager.GetAllPurchases(token)
                .Select(p => new PurchaseModelResponse(p)).ToList();
            return Ok(retrievedPuerchases);

        }

        [HttpGet]
        [Route("[action]")]
        [AuthorizationFilter(new string[] { nameof(RoleType.Owner) })]
        public IActionResult ByDate([FromQuery] DateTime? start, [FromQuery] DateTime? end)
        {
            string token = HttpContext.Request.Headers["Authorization"];
            var retrievedPuerchases = _purchasesManager.GetAllPurchasesByDate(token, start, end)
                .Select(p => new PurchaseModelResponse(p)).ToList();
            return Ok(retrievedPuerchases);
        }

        [HttpPut]
        [Route("[action]/{id}")]
        [AuthorizationFilter(new string[] { nameof(RoleType.Employee) })]
        public IActionResult Approve(int id, [FromBody] PurchaseAuthorizationModel model)
        {
            var purchaseDetail = _purchasesManager.ApprobePurchaseDetail(id, model.pharmacyId, model.drugCode);
            var purchaseDetailModelResponse = new PurchaseDetailModelResponse(id, purchaseDetail);
            return Ok(purchaseDetailModelResponse);
        }

        [HttpPut]
        [Route("[action]/{id}")]
        [AuthorizationFilter(new string[] { nameof(RoleType.Employee) })]
        public IActionResult Reject(int id, [FromBody] PurchaseAuthorizationModel model)
        {
            var purchaseDetail = _purchasesManager.RejectPurchaseDetail(id, model.pharmacyId, model.drugCode);
            var purchaseDetailModelResponse = new PurchaseDetailModelResponse(id, purchaseDetail);
            return Ok(purchaseDetailModelResponse);
        }

        [HttpPost]
        public IActionResult CreatePurchase([FromBody] PurchaseModelRequest purchaseModel)
        {
            var converter = new PurchaseModelRequestToPurchaseConverter();
            var purchase = _purchasesManager.CreatePurchase(converter.Convert(purchaseModel));
            var purchaseModelResponse = new PurchaseModelResponse(purchase);
            return Ok(purchaseModelResponse);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Tracking([FromQuery] string? Code)
        {
            var purchase = _purchasesManager.GetPurchaseByTrackingCode(Code);
            var purchaseModelResponse = new PurchaseModelResponse(purchase);
            return Ok(purchaseModelResponse);
        }
    }
}
