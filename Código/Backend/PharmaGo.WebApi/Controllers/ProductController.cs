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
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ExceptionFilter))]
    public class ProductController : Controller
    {
        private readonly IProductManager _productManager;

        public ProductController(IProductManager productManager)
        {
            _productManager = productManager;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] ProductSearchCriteria productSearchCriteria)
        {
            IEnumerable<Product> products = _productManager.GetAll(productSearchCriteria);
            IEnumerable<ProductBasicModel> productsToReturn = products.Select(d => new ProductBasicModel(d));
            return Ok(productsToReturn);
        }
        [HttpPost]
        [AuthorizationFilter(new string[] {nameof(RoleType.Employee)})]
        public IActionResult Create([FromBody] ProductModel productModel)
        {
            string token = HttpContext.Request.Headers["Authorization"];
            Product productCreated = _productManager.Create(productModel.ToEntity(), token);
           ProductDetailModel drugResponse = new ProductDetailModel(productCreated);
           return Ok(drugResponse);
        }

        [HttpDelete("{id}")]
        [AuthorizationFilter(new string[] { nameof(RoleType.Employee) })]
        public IActionResult Delete([FromRoute] int id)
        {
            _productManager.Delete(id);
            return Ok(true);
        }

        [HttpPut("{id}")]
        [AuthorizationFilter(new string[] { nameof(RoleType.Employee) })]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateProductModel updatedProduct)
        {
            string token = HttpContext.Request.Headers["Authorization"];
            Product product = _productManager.Update(id, updatedProduct.ToEntity(),token);
            return Ok(new ProductDetailModel(product));
        }
    }
}
