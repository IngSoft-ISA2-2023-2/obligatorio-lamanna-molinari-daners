using Microsoft.AspNetCore.Http;
using Microsoft.Build.Evaluation;
using Newtonsoft.Json;
using PharmaGo.Domain.Entities;
using System;
using System.Net;
using System.Net.Http.Headers;
using TechTalk.SpecFlow;

namespace PharmaGo.SpecFlow.StepDefinitions
{
    [Binding]
    public class CreateProductStepDefinitions
    {
        private readonly Product _product = new Product();
        private readonly ScenarioContext context;

        public CreateProductStepDefinitions(ScenarioContext context)
        {
            this.context = context;
        }

        [Given(@"The name ""([^""]*)""")]
        public void GivenTheName(string nombre)
        {
            _product.Name = nombre;
        }

        [Given(@"the description ""([^""]*)""")]
        public void GivenTheDescription(string descripcion)
        {
            _product.Description = descripcion;
        }

        [Given(@"the code ""([^""]*)""")]
        public void GivenTheCode(int code)
        {
            _product.Code = code;
        }

        [Given(@"the price ""([^""]*)""")]
        public void GivenThePrice(decimal price)
        {
            _product.Price = price;
        }

        [When(@"A ""([^""]*)"" is created with the values")]
        public async Task WhenAIsCreatedWithTheValues(string operation)
        {
            string requestBody = JsonConvert.SerializeObject(new { nombre = _product.Name, descripcion = _product.Description, codigo = _product.Code, precio = _product.Price });
            var request = new HttpRequestMessage(HttpMethod.Post, $"http://localhost:7186/api/product")
            {
                Content = new StringContent(requestBody)
                {
                    Headers =
                        {
                          ContentType = new MediaTypeHeaderValue("application/json")
                        }
                }
            };

            // create an http client
            var client = new HttpClient();
            // let's post
            var response = await client.SendAsync(request).ConfigureAwait(false);
            
       
            try
            {
                context.Set(response.StatusCode, "ResponseStatusCode");
            } finally
            {
                //move along
            }
        }

        [Then(@"i receive the response with the ""([^""]*)"" and ""([^""]*)""")]
        public void ThenIReceiveTheResponseWithTheAnd(int statusCode, string message)
        {
            Console.Write(context);


            Assert.AreEqual(statusCode, (int)context.Get<HttpStatusCode>("ResponseStatusCode"));
            // Assert.AreEqual(message, context.Get<HttpResponse>("ResponseStatusCode"));
        }
    }
}
