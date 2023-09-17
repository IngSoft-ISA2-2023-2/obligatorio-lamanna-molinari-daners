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
    public class PharmacyController : Controller
    {
        private readonly IPharmacyManager _pharmacyManager;

        public PharmacyController(IPharmacyManager pharmacyManager)
        {
            _pharmacyManager = pharmacyManager;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] PharmacySearchCriteria pharmacySearchCriteria)
        {
            IEnumerable<Pharmacy> pharmacies = _pharmacyManager.GetAll(pharmacySearchCriteria);
            IEnumerable<PharmacyBasicModel> pharmaciesToReturn = pharmacies.Select(p => new PharmacyBasicModel(p));
            return Ok(pharmaciesToReturn);
        }

        [HttpGet("{id}")]
        [AuthorizationFilter(new string[] { nameof(RoleType.Administrator) })]
        public IActionResult GetById([FromRoute] int id)
        {
            Pharmacy pharmacy = _pharmacyManager.GetById(id);
            return Ok(new PharmacyDetailModel(pharmacy));
        }

        [HttpPost]
        [AuthorizationFilter(new string[] { nameof(RoleType.Administrator) })]
        public IActionResult Create([FromBody] PharmacyModel pharmacyModel)
        {
            Pharmacy pharmacyCreated = _pharmacyManager.Create(pharmacyModel.ToEntity());
            PharmacyDetailModel pharmacyResponse = new PharmacyDetailModel(pharmacyCreated);
            return Ok(pharmacyResponse);
        }

        [HttpPut("{id}")]
        [AuthorizationFilter(new string[] { nameof(RoleType.Administrator) })]
        public IActionResult Update([FromRoute] int id, [FromBody] PharmacyModel updatedPharmacy)
        {
            Pharmacy pharmacy = _pharmacyManager.Update(id, updatedPharmacy.ToEntity());
            return Ok(new PharmacyDetailModel(pharmacy));
        }

        [HttpDelete("{id}")]
        [AuthorizationFilter(new string[] { nameof(RoleType.Administrator) })]
        public IActionResult Delete([FromRoute] int id)
        {
            _pharmacyManager.Delete(id);
            return Ok(true);
        }
    }
}
