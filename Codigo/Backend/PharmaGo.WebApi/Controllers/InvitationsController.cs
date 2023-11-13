using Microsoft.AspNetCore.Mvc;
using PharmaGo.BusinessLogic;
using PharmaGo.Domain.Entities;
using PharmaGo.Exceptions;
using PharmaGo.IBusinessLogic;
using PharmaGo.WebApi.Enums;
using PharmaGo.WebApi.Filters;
using PharmaGo.WebApi.Models.In;
using PharmaGo.WebApi.Models.Out;

namespace PharmaGo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationsController : ControllerBase
    {
        private readonly IInvitationManager _invitationManager;

        public InvitationsController(IInvitationManager manager)
        {
            _invitationManager = manager;
        }

        [HttpPost]
        [AuthorizationFilter(new string[] { nameof(RoleType.Administrator), nameof(RoleType.Owner) })]
        public IActionResult CreateInvitation([FromBody] InvitationModelRequest invitationModel)
        {
            string token = HttpContext.Request.Headers["Authorization"];
            var invitation = _invitationManager.CreateInvitation(token, invitationModel.ToEntity());
            return Ok(new InvitationModelResponse(invitation));
        }

        [HttpGet]
        [AuthorizationFilter(new string[] { nameof(RoleType.Administrator) })]
        public IActionResult GetAll([FromQuery] InvitationSearchCriteriaModelRequest searchCriteria)
        {
            var invitations = _invitationManager.GetAllInvitations(searchCriteria.ToEntity());

            IEnumerable<InvitationSearchCriteriaModelResponse> result =
                invitations.Select(invitaion => new InvitationSearchCriteriaModelResponse(invitaion));

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            Invitation invitation = _invitationManager.GetById(id);
            return Ok(new InvitationDetailModelResponse(invitation));
        }

        [HttpPut("{id}")]
        [AuthorizationFilter(new string[] { nameof(RoleType.Administrator) })]
        public IActionResult UpdateInvitation([FromRoute] int id, [FromBody] InvitationModelRequest invitationModel)
        {
            var invitation = _invitationManager.UpdateInvitation(id, invitationModel.ToEntity());
            InvitationDetailModelResponse result = new InvitationDetailModelResponse(invitation);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        [AuthorizationFilter(new string[] { nameof(RoleType.Administrator) })]
        public IActionResult UserCode()
        {
            var userCode = _invitationManager.CreateUserCode();
            return Ok(new InvitationUserCodeModelResponse(userCode));
        }

    }
}
