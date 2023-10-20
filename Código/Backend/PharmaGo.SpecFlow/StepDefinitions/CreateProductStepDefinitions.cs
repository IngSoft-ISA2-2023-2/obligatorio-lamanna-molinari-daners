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
        public void GivenTheName(string name)
        {
            _product.Name = name;
        }

        [Given(@"la descripcion ""([^""]*)""")]
        public void GivenLaDescripcion(string descripcion)
        {
            _product.Description = descripcion;
        }


        [Given(@"el codigo ""([^""]*)""")]
        public void GivenElCodigo(string code)
        {
            _product.Code = code;
        }

        [Given(@"el precio ""([^""]*)""")]
        public void GivenElPrecio(int price)
        {
            _product.Price = price;
        }

        [When(@"Un product es creado mediante el ""([^""]*)"" con el endpoint")]
        public async Task WhenUnProductEsCreadoMedianteElConElEndpoint(string operation)
        {
            string requestBody = JsonConvert.SerializeObject(new { nombre = _product.Name, descripcion = _product.Description, codigo = _product.Code, precio = _product.Price });
            var request = new HttpRequestMessage(HttpMethod.Post, $"http://localhost:41123/api/product")
            {
                Content = new StringContent(requestBody)
                {
                    Headers =
                        {
                          ContentType = new MediaTypeHeaderValue("application/json")
                        }
                }
            };
            // if (false) { } //
            // create an http client
            var client = new HttpClient();
            // let's post
            var response = await client.SendAsync(request).ConfigureAwait(false);
            
       
            try
            {
                context.Set(response.StatusCode, "ResponseStatusCode");
                context.Set(response.Content, "ReponseMessage");
            } finally
            {
                //move along
            }
        }

        [Then(@"recibo una respuesta con el codigo ""([^""]*)""")]
        public void ThenReciboUnaRespuestaConElCodigoYElMensaje(int statusCode)
        {
            Assert.AreEqual(statusCode, (int)context.Get<HttpStatusCode>("ResponseStatusCode"));
        }
    }
}
