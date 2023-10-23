using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PharmaGo.BusinessLogic;
using PharmaGo.DataAccess.Repositories;
using PharmaGo.DataAccess;
using PharmaGo.Domain.Entities;
using PharmaGo.IDataAccess;
using PharmaGo.WebApi.Controllers;
using PharmaGo.WebApi.Models.In;
using System;
using System.Net;
using System.Net.Http.Headers;
using TechTalk.SpecFlow;


namespace PharmaGo.SpecFlow.StepDefinitions
{
    [Binding]
    public class DeleteProductStepDefinitions
    {
        private PharmacyGoDbContext theDbContext;
        private IRepository<Product> theProductRepository;
        private IRepository<Pharmacy> thePharmacyRepository;
        private IRepository<Session> theSessionRepository;
        private IRepository<User> theUserRepository;

        private ProductManager productManager;
        private ProductController productController;
        private int _productId;

        private IActionResult result;

        [BeforeScenario]
        public void Setup()
        {
            var connectionString = "Server=.\\SQLEXPRESS;Database=PharmaGoDb;Trusted_Connection=True; MultipleActiveResultSets=True";
            var optionsBuilder = new DbContextOptionsBuilder<PharmacyGoDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            theDbContext = new PharmacyGoDbContext(optionsBuilder.Options);

            theProductRepository = new ProductRepository(theDbContext);
            thePharmacyRepository = new PharmacyRepository(theDbContext);
            theSessionRepository = new SessionRepository(theDbContext);
            theUserRepository = new UsersRepository(theDbContext);

            productManager = new ProductManager(theProductRepository, thePharmacyRepository, theSessionRepository, theUserRepository);
            productController = new ProductController(productManager);

            

        }

        [Given(@"I am an authorized employee deleting a product")]
        public void GivenIAmAnAuthorizedEmployeeDeletingAProduct()
        {
         
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "E9E0E1E9-3812-4EB5-949E-AE92AC931401";

            productController.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
        }

        [When(@"I choose the product with code ""([^""]*)"" to be deleted")]
        public void WhenIChooseTheProductWithCodeToBeDeleted(string code)
        {
            ProductModel p = new ProductModel()
            {
                Name = "name",
                Code = code,
                Description = "description",
                Price = 100
            };
            try
            {
                productController.Create(p);
            }catch(Exception e)
            {

            }
            
            Product product = theProductRepository.GetOneByExpression(p => p.Code == code);
            _productId = product.Id;

        }

        [Then(@"the response status code should be ""([^""]*)""")]
        public void ThenTheResponseStatusCodeShouldBe(string codeResponse)
        {
            result = productController.Delete(_productId);
            var response = result as ObjectResult;
            int responseCode = response.StatusCode.Value;
            Assert.AreEqual(responseCode, int.Parse(codeResponse));
        }

       
    }
}
