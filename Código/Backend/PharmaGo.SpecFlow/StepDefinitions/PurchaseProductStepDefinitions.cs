using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaGo.BusinessLogic;
using PharmaGo.DataAccess.Repositories;
using PharmaGo.DataAccess;
using PharmaGo.Domain.Entities;
using PharmaGo.IDataAccess;
using PharmaGo.WebApi.Controllers;
using PharmaGo.WebApi.Models.In;
using System;
using TechTalk.SpecFlow;
using static PharmaGo.WebApi.Models.In.PurchaseModelRequest;

namespace PharmaGo.SpecFlow.StepDefinitions
{
    [Binding]
    public class PurchaseProductStepDefinitions
    {
        private PharmacyGoDbContext theDbContext;
        private IRepository<Product> theProductRepository;
        private IRepository<Pharmacy> thePharmacyRepository;
        private IRepository<Session> theSessionRepository;
        private IRepository<User> userRepository;
        private IRepository<Purchase> purchaseRepository;
        private IRepository<Drug> drugRepository;
        private IRepository<PurchaseDetail> purchaseDetailRepository;
        private IRepository<PurchaseDetailProduct> purchaseDetailProductRepository;

        private ProductManager productManager;
        private ProductController productController;
        private ProductModel productModel;

        private PurchasesManager purchasesManager;
        private PurchasesController purchasesController;
        private PurchaseModelRequest purchaseModelRequest;

        private IActionResult result;
        private PurchaseDetailProductModelRequest productReq;
        private PurchaseDetailModelRequest detailsReq;
        private PurchaseModelRequest request;

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
            drugRepository = new DrugRepository(theDbContext);
            purchaseDetailRepository = new PurchasesDetailRepository(theDbContext);
            purchaseDetailProductRepository = new PurchasesDetailProductRepository(theDbContext);
            userRepository = new UsersRepository(theDbContext);

            productController = new ProductController(productManager);
            productModel = new ProductModel();

            purchaseRepository = new PurchasesRepository(theDbContext);
            purchasesManager = new PurchasesManager(purchaseRepository, thePharmacyRepository, drugRepository, purchaseDetailRepository, purchaseDetailProductRepository, theSessionRepository, userRepository);
            purchasesController = new PurchasesController(purchasesManager);
        }

        [Given(@"I'm a client")]
        public void GivenImAClient()
        {
            //nothing to do
        }

        [When(@"I enter a purhcase request")]
        public void WhenIEnterAPurhcaseRequest()
        {
            productReq = new PurchaseDetailProductModelRequest()
            {
                PharmacyId = 1,
                Code = "12345",
                Quantity = 1
            };

            detailsReq = new PurchaseDetailModelRequest()
            {
                PharmacyId = 1,
                Code = "TEST", //name of test drug in the database
                Quantity = 1
            };

            request = new PurchaseModelRequest()
            {
                BuyerEmail = "abcde@gmail.com",
                PurchaseDate = DateTime.Now,
                Details = new List<PurchaseDetailModelRequest>() { detailsReq },
                DetailsProducts = new List<PurchaseDetailProductModelRequest>(){ productReq }
            };
        }

        [Then(@"the response code should be  ""([^""]*)""")]
        public void ThenTheResponseCodeShouldBe(string statusCode)
        {
            result = purchasesController.CreatePurchase(request);
            var response = result as ObjectResult;
            int responseCode = response.StatusCode.Value;
            Assert.AreEqual(responseCode, int.Parse(statusCode));
        }

    }
}
