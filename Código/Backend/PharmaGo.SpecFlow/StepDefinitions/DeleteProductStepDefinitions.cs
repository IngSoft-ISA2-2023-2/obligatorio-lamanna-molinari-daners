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
    public class DeleteProductStepDefinitions
    {

        private readonly Product _product = new Product();
        private readonly ScenarioContext context;

        public DeleteProductStepDefinitions(ScenarioContext context)
        {
            this.context = context;
        }
        
        [Given(@"La id de producto ""([^""]*)""")]
        public void GivenLaIdDeProducto(int id)
        {
            _product.Id = id;
        }

        [When(@"Un producto es eliminado mediante el ""([^""]*)"" con el endpoint")]
        public async Task WhenUnProductoEsEliminadoMedianteElConElEndpoint(string product)
        {
            string requestBody = JsonConvert.SerializeObject(new { id = _product.Id});
            var request = new HttpRequestMessage(HttpMethod.Delete, $"http://localhost:41123/api/{product}")
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
            }
            finally
            {
                //move along
            }
        }


        [Then(@"recibo una respuesta del delete con el codigo ""([^""]*)""")]
        public void ThenReciboUnaRespuestaDelDeleteConElCodigo(int statusCode)
        {
            Assert.AreEqual(statusCode, (int)context.Get<HttpStatusCode>("ResponseStatusCode"));
        }

    }
}
