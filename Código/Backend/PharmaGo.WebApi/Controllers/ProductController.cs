using Microsoft.AspNetCore.Mvc;
using PharmaGo.BusinessLogic;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;
using PharmaGo.IBusinessLogic;
using PharmaGo.WebApi.Enums;
using PharmaGo.WebApi.Filters;
using PharmaGo.WebApi.Models.In;
using PharmaGo.WebApi.Models.Out;

namespace PharmaGo.WebApi.Controllers
{
    [Route("api/product")]
    [ApiController]
    [TypeFilter(typeof(ExceptionFilter))]
    public class ProductController : Controller
    {
        [HttpPost]
        public IActionResult Create()
        {
            //string token = HttpContext.Request.Headers["Authorization"];
           // Product productCreated = _productManager.Create(drugModel.ToEntity(), token);
           // DrugDetailModel drugResponse = new DrugDetailModel(drugCreated);
            return Ok(200);
        }
    }
}
