﻿using Microsoft.AspNetCore.Mvc;
using PharmaGo.BusinessLogic;
using PharmaGo.Domain.Entities;
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

        [HttpPost]
        [AuthorizationFilter(new string[] {nameof(RoleType.Employee)})]
        public IActionResult Create([FromBody] ProductModel productModel)
        {
            string token = HttpContext.Request.Headers["Authorization"];
           // string token = "E9E0E1E9-3812-4EB5-949E-AE92AC931401";
            Product productCreated = _productManager.Create(productModel.ToEntity(), token);
           ProductDetailModel drugResponse = new ProductDetailModel(productCreated);
           return Ok(drugResponse);
        }
    }
}
