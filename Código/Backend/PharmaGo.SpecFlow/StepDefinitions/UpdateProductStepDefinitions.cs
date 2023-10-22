using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaGo.BusinessLogic;
using PharmaGo.DataAccess;
using PharmaGo.DataAccess.Repositories;
using PharmaGo.Domain.Entities;
using PharmaGo.IDataAccess;
using PharmaGo.WebApi.Controllers;
using PharmaGo.WebApi.Models.In;
using System;
using System.Reflection.Emit;
using System.Xml.Linq;
using TechTalk.SpecFlow;

namespace PharmaGo.SpecFlow.StepDefinitions
{
    [Binding]
    public class UpdateProductStepDefinitions
    {
        private PharmacyGoDbContext theDbContext;
        private IRepository<Product> theProductRepository;
        private IRepository<Pharmacy> thePharmacyRepository;
        private IRepository<Session> theSessionRepository;
        private IRepository<User> theUserRepository;

        private ProductManager productManager;
        private ProductController productController;
        private UpdateProductModel productModel;
        private int _productToChangeId;
        private IActionResult result;

        [BeforeScenario]
        public void Setup()
        {
            var connectionString = "Server=DESKTOP-O360J65\\SQLEXPRESS;Database=PharmaGoDb;Trusted_Connection=True; MultipleActiveResultSets=True"; //agregar connection string a db
            var optionsBuilder = new DbContextOptionsBuilder<PharmacyGoDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            theDbContext = new PharmacyGoDbContext(optionsBuilder.Options);

            theProductRepository = new ProductRepository(theDbContext);
            thePharmacyRepository = new PharmacyRepository(theDbContext);
            theSessionRepository = new SessionRepository(theDbContext);
            theUserRepository = new UsersRepository(theDbContext);

            productManager = new ProductManager(theProductRepository, thePharmacyRepository, theSessionRepository, theUserRepository);
            productController = new ProductController(productManager);
            productModel = new UpdateProductModel();

        }

        [Given(@"I am an authorized employee who selected the product with code ""([^""]*)""")]
        public void GivenIAmAnAuthorizedEmployeeWhoSelectedTheProductWithCode(string oldCode)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "E9E0E1E9-3812-4EB5-949E-AE92AC931401";

            productController.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            ProductModel p = new ProductModel()
            {
                Name = "name",
                Code = oldCode,
                Description = "description",
                Price = 100
            };
            try
            {
                productController.Create(p);
            }
            catch (Exception e)
            {

            }
            
            Product product = theProductRepository.GetOneByExpression(p => p.Code == oldCode);
            _productToChangeId = product.Id;

        }

        [When(@"I update a product with the new name ""([^""]*)""")]
        public void WhenIUpdateAProductWithTheNewName(string name)
        {
            productModel.Name = name;
        }

        [When(@"the new description ""([^""]*)""")]
        public void WhenTheNewDescription(string description)
        {
            productModel.Description = description;
        }

        [When(@"the new code ""([^""]*)""")]
        public void WhenTheNewCode(string code)
        {
            productModel.Code = code;
        }

        [When(@"the new price ""([^""]*)""")]
        public void WhenTheNewPrice(string price)
        {
            productModel.Price = decimal.Parse(price);
        }

        [Then(@"the response code should be ""([^""]*)""")]
        public void ThenTheResponseCodeShouldBe(string statusCode)
        {
            result = productController.Update(_productToChangeId, productModel);
            var response = result as ObjectResult;
            int responseCode = response.StatusCode.Value;
            Assert.AreEqual(responseCode, int.Parse(statusCode));
        }


        [Then(@"the error response message should be ""([^""]*)""")]
        public void ThenTheErrorResponseMessageShouldBe(string message)
        {
            try
            {
                result = productController.Update(_productToChangeId, productModel);

                var response = result as ObjectResult;

            }
            catch (Exception e)
            {
                Assert.AreEqual(message, e.Message);
            }
        }
    }
}
