using Microsoft.AspNetCore.Mvc;
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
    public class DrugController : Controller
    {
        private readonly IDrugManager _drugManager;

        public DrugController(IDrugManager manager)
        {
            _drugManager = manager;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] DrugSearchCriteria drugSearchCriteria)
        {
            IEnumerable<Drug> drugs = _drugManager.GetAll(drugSearchCriteria);
            IEnumerable<DrugBasicModel> drugsToReturn = drugs.Select(d => new DrugBasicModel(d));
            return Ok(drugsToReturn);
        }

        [HttpGet]
        [Route("[action]")]
        [AuthorizationFilter(new string[] { nameof(RoleType.Employee) })]
        public IActionResult User()
        {
            string token = HttpContext.Request.Headers["Authorization"];
            IEnumerable<Drug> drugs = _drugManager.GetAllByUser(token);
            IEnumerable<DrugBasicModel> drugsToReturn = drugs.Select(d => new DrugBasicModel(d));
            return Ok(drugsToReturn);
        }

        [HttpGet("{id}")]

        public IActionResult GetById([FromRoute] int id)
        {
            Drug drug = _drugManager.GetById(id);
            return Ok(new DrugDetailModel(drug));
        }

        [HttpPost]
        [AuthorizationFilter(new string[] { nameof(RoleType.Employee) })]
        public IActionResult Create([FromBody] DrugModel drugModel)
        {
            string token = HttpContext.Request.Headers["Authorization"];
            Drug drugCreated = _drugManager.Create(drugModel.ToEntity(), token);
            DrugDetailModel drugResponse = new DrugDetailModel(drugCreated);
            return Ok(drugResponse);
        }

        [HttpPut("{id}")]
        [AuthorizationFilter(new string[] { nameof(RoleType.Administrator) })]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateDrugModel updatedDrug)
        {
            Drug drug = _drugManager.Update(id, updatedDrug.ToEntity());
            return Ok(new DrugDetailModel(drug));
        }

        [HttpDelete("{id}")]
        [AuthorizationFilter(new string[] { nameof(RoleType.Employee) })]
        public IActionResult Delete([FromRoute] int id)
        {
            _drugManager.Delete(id);
            return Ok(true);
        }
    }
}
