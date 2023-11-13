using Microsoft.AspNetCore.Mvc;
using PharmaGo.IBusinessLogic;
using PharmaGo.WebApi.Models.In;
using PharmaGo.WebApi.Models.Out;

namespace PharmaGo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginManager _loginManager;

       public LoginController(ILoginManager manager)
       {
         _loginManager = manager;
       }

        [HttpPost]
        public IActionResult Login([FromBody] LoginModelRequest userModel)
        {
            var authorization = _loginManager.Login(userModel.UserName, userModel.Password);
            return Ok(new LoginModelResponse() { token = authorization.Token, role = authorization.Role, userName = authorization.UserName });
        }

    }
}
