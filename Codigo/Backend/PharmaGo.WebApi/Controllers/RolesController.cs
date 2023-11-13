using Microsoft.AspNetCore.Mvc;
using PharmaGo.IBusinessLogic;
using PharmaGo.WebApi.Models.Out;

namespace PharmaGo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleManager _roleManager;

        public RolesController(IRoleManager manager)
        {
            _roleManager = manager;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var roles = _roleManager.GetAll();
            IEnumerable<RoleModelResponse> result = roles.Select(role => new RoleModelResponse(role));
            return Ok(result);
        }

    }
}
