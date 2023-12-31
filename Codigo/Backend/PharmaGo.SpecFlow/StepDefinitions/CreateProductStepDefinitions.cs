using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PharmaGo.BusinessLogic;
using PharmaGo.DataAccess;
using PharmaGo.DataAccess.Repositories;
using PharmaGo.Domain.Entities;
using PharmaGo.IDataAccess;
using PharmaGo.WebApi.Controllers;
using PharmaGo.WebApi.Models.In;
using SpecFlow.Internal.Json;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using TechTalk.SpecFlow;

namespace PharmaGo.SpecFlow.StepDefinitions
{
    [Binding]
    public class CreateProductStepDefinitions
    {
        private PharmacyGoDbContext theDbContext;
        private IRepository<Product> theProductRepository;
        private IRepository<Pharmacy> thePharmacyRepository;
        private IRepository<Session> theSessionRepository;
        private IRepository<User> theUserRepository;

        private ProductManager productManager;
        private ProductController productController;
        private ProductModel productModel;

        private IActionResult result;

        [BeforeScenario]
        public void Setup()
        {
            var connectionString = "Server=.\\SQLEXPRESS;Database=PharmaGoDb;Trusted_Connection=True; MultipleActiveResultSets=True"; //agregar connection string a db
            var optionsBuilder = new DbContextOptionsBuilder<PharmacyGoDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            theDbContext = new PharmacyGoDbContext(optionsBuilder.Options);

            theProductRepository = new ProductRepository(theDbContext);
            thePharmacyRepository = new PharmacyRepository(theDbContext);
            theSessionRepository = new SessionRepository(theDbContext);
            theUserRepository = new UsersRepository(theDbContext);

            productManager = new ProductManager(theProductRepository, thePharmacyRepository, theSessionRepository, theUserRepository);
            productController = new ProductController(productManager);
            productModel = new ProductModel();

        }

        [Given(@"I am an authorized employee with ""([^""]*)""")]
        public void GivenIAmAnAuthorizedEmployee(string token)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = token;

            productController.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            productModel.PharmacyName = thePharmacyRepository.GetOneByExpression(p => p.Id == 1).Name;
        }


        [When(@"I add a new product with the name ""([^""]*)""")]
        public void WhenIAddANewProductWithTheName(string name)
        {
            productModel.Name = name;
        }

        [When(@"the description ""([^""]*)""")]
        public void WhenTheDescription(string description)
        {
            productModel.Description = description;
        }

        [When(@"the code ""([^""]*)""")]
        public void WhenTheCode(string code)
        {
            productModel.Code = code;
        }

        [When(@"the price ""([^""]*)""")]
        public void WhenThePrice(string price)
        {
            productModel.Price = decimal.Parse(price);
        }

        [Then(@"the response status should be ""([^""]*)""")]
        public void ThenTheResponseStatusShouldBeAnd(string statusCode)
        {
        
            result = productController.Create(productModel);
            var response = result as ObjectResult;
            int responseCode = response.StatusCode.Value;
            Assert.AreEqual(responseCode, int.Parse(statusCode));
        }

        [Then(@"the response message should be ""([^""]*)""")]
        public void ThenTheResponseMessageShouldBe(string message)
        {
            try
            {
                result = productController.Create(productModel);

                var response = result as ObjectResult;
                
            }catch(Exception e)
            {
                Assert.AreEqual(message, e.Message);
            }
        }

        [AfterScenario]
        public void Teardown()
        {
            Product p = theProductRepository.GetOneByExpression(p => p.Code == productModel.Code && p.Pharmacy.Name == productModel.PharmacyName);
            if (p != null) { productController.Delete(p.Id); }
        }
    }
}