using Microsoft.AspNetCore.Http;
using Microsoft.Build.Evaluation;
using Newtonsoft.Json;
using PharmaGo.Domain.Entities;
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

        [When(@"Un product es creado por el empleado con el token ""([^""]*)"" mediante el ""([^""]*)"" con el endpoint")]
        public async Task WhenUnProductEsCreadoPorElEmpleadoConElTokenMedianteElConElEndpoint(string token, string product)
        {

        string requestBody = JsonConvert.SerializeObject(new { nombre = _product.Name, descripcion = _product.Description, codigo = _product.Code, precio = _product.Price, pharmacyName = "Farmacia 1234" });
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"http://localhost:41123/api/product")
            {
                Content = new StringContent(requestBody)
                {
                    Headers =
                        {
                          ContentType = new MediaTypeHeaderValue("application/json"),
                          
                        }
                },
            };

            request.Headers.Authorization = new AuthenticationHeaderValue(token);


            var client = new HttpClient();
          
            //Employee1
            var response = await client.SendAsync(request).ConfigureAwait(false);

           
            try
            {
                context.Set(response.StatusCode, "ResponseStatusCode");
                
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
