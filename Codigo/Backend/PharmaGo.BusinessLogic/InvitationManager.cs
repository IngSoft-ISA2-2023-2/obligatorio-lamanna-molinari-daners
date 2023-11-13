using System.Text.RegularExpressions;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;
using PharmaGo.Exceptions;
using PharmaGo.IBusinessLogic;
using PharmaGo.IDataAccess;

namespace PharmaGo.BusinessLogic
{
	public class InvitationManager : IInvitationManager
	{
		private readonly IRepository<Invitation> _invitationRepository;
        private readonly IRepository<Pharmacy> _pharmacyRepository;
		private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<Session> _sessionRepository;
        private readonly IRepository<User> _userRepository;

        public InvitationManager(IRepository<Invitation> invitarionRepository,
            IRepository<Pharmacy> pharmacyRepository, IRepository<Role> roleRepository,
            IRepository<Session> sessionRepository, IRepository<User> userRepository)
		{
			_invitationRepository = invitarionRepository;
			_pharmacyRepository = pharmacyRepository;
			_roleRepository = roleRepository;
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
		}

        public Invitation CreateInvitation(string token, Invitation invitation)
        {
            if (string.IsNullOrEmpty(token.ToString())) throw new InvalidResourceException("Invalid Administrator or Owner Role.");
            var userToken = _sessionRepository.GetOneByExpression(expression => expression.Token == new Guid(token));
            if (userToken == null) throw new ResourceNotFoundException("Invalid User session.");
            var user = _userRepository.GetOneDetailByExpression(expression => expression.Id == userToken.UserId);
            if (user == null) throw new ResourceNotFoundException("Invalid User.");

            if (user.Role.Name.Equals("Administrator"))
            {
                if (invitation.Role == null) throw new InvalidResourceException("Invalid Role.");

                if (invitation.Role != null)
                {
                    var role = _roleRepository.GetOneByExpression(r => r.Name == invitation.Role.Name);
                    if (role == null) throw new InvalidResourceException("Invalid Role.");

                    invitation.Role = role;
                }

                if (!invitation.Role.Name.Equals("Administrator"))
                {
                    if (invitation.Pharmacy is null)
                    {
                        throw new InvalidResourceException("A pharmacy is required.");
                    }
                }
                else
                {
                    if (invitation.Pharmacy != null)
                    {
                        throw new InvalidResourceException("A pharmacy is not required.");
                    }

                }

                if (invitation.Pharmacy != null)
                {
                    var pharmacy = _pharmacyRepository.GetOneByExpression(f => f.Name == invitation.Pharmacy.Name);
                    if (pharmacy == null) throw new InvalidResourceException("Invalid Pharmacy.");

                    invitation.Pharmacy = pharmacy;
                }
            }
            else
            {
                invitation.Pharmacy = user.Pharmacy;

                var role = _roleRepository.GetOneByExpression(expression => expression.Name == "Employee");
                if (role == null) throw new ResourceNotFoundException("Invalid Employee role.");
                invitation.Role = role;
            }

            if (String.IsNullOrEmpty(invitation.UserName)) throw new InvalidResourceException("Invalid UserName.");

            Invitation alreadyUserInvitation = _invitationRepository.GetOneByExpression(i => i.UserName == invitation.UserName);
            if (alreadyUserInvitation != null) throw new InvalidResourceException("Invitation already exist.");

            invitation.UserCode = this.CreateUserCode();

            _invitationRepository.InsertOne(invitation);
            _invitationRepository.Save();
            return invitation;
        }

        public string CreateUserCode()
		{
            string validUserCode = @"^[0-9]{6}$";
            Regex regexUserCode = new(validUserCode);

            Random r = new Random();
            int randNum = r.Next(1000000);
            string userCode = randNum.ToString("D6");

            if (String.IsNullOrEmpty(userCode)) throw new InvalidResourceException("UserCode not generated.");
            if (!regexUserCode.IsMatch(userCode)) throw new InvalidResourceException("UserCode must be numeric.");

			return userCode;
		}

        public IEnumerable<Invitation> GetAllInvitations(InvitationSearchCriteria searchCriteria)
        {
            return _invitationRepository.GetAllBasicByExpression(searchCriteria.Criteria());
        }

        public Invitation GetById(int id)
        {
            return _invitationRepository.GetOneDetailByExpression(expression => expression.Id == id);
        }

        public Invitation UpdateInvitation(int id, Invitation invitation)
        {
            var invitationEntity = _invitationRepository.GetOneDetailByExpression(expression => expression.Id == id);
            if (invitationEntity == null) throw new ResourceNotFoundException("Invalid Invitation.");
            if (invitationEntity.IsActive == false) throw new InvalidResourceException("Invitation is not active.");

            if (invitation.Role != null)
            {
                if (string.IsNullOrEmpty(invitation.Role.Name)) throw new InvalidResourceException("Invalid Role.");
                var role = _roleRepository.GetOneByExpression(expression => expression.Name == invitation.Role.Name);
                if (role == null) throw new ResourceNotFoundException("Invalid role.");

                if (!invitation.Role.Name.Equals("Administrator"))
                {
                    if (invitation.Pharmacy is null) throw new InvalidResourceException("A Pharmacy is required.");
                }
                else
                {
                    if (invitation.Pharmacy != null) throw new InvalidResourceException("A Pharmacy is not required.");
                }

                invitationEntity.Role = role;
            }

            if (!string.IsNullOrEmpty(invitation.UserName))
            {
                var alreadyExist = _invitationRepository.GetOneByExpression(userExpression => userExpression.UserName == invitation.UserName);
                if (alreadyExist != null)
                {
                    if (alreadyExist.Id != id) throw new InvalidResourceException("UserName already exist.");
                }

                invitationEntity.UserName = invitation.UserName;
            }

            if (invitation.Pharmacy != null)
            {
                if (string.IsNullOrEmpty(invitation.Pharmacy.Name)) throw new InvalidResourceException("Invalid Pharmacy.");
                var pharmacy = _pharmacyRepository.GetOneByExpression(expression => expression.Name == invitation.Pharmacy.Name);
                if (pharmacy == null) throw new ResourceNotFoundException("Invalid Pharmacy.");
                invitationEntity.Pharmacy = pharmacy;
            }
            else
            {
                invitationEntity.Pharmacy = null;
            }

            if (!string.IsNullOrEmpty(invitation.UserCode))
                invitationEntity.UserCode = invitation.UserCode;

            _invitationRepository.UpdateOne(invitationEntity);
            _invitationRepository.Save();

            return invitationEntity;
        }
    }
}

