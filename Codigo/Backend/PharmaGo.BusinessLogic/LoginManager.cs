using PharmaGo.Domain.Entities;
using PharmaGo.Exceptions;
using PharmaGo.IBusinessLogic;
using PharmaGo.IDataAccess;

namespace PharmaGo.BusinessLogic
{
    public class LoginManager : ILoginManager
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Session> _sessionRepository;

        public LoginManager(IRepository<User> userRepository, IRepository<Session> sessionRepository)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
        }

        public Authorization Login(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName))
            {
                throw new InvalidResourceException("Invalid Username");
            }
            User user = _userRepository.GetOneDetailByExpression(u => u.UserName.ToLower().Equals(userName.ToLower()));
            if (user == null) {
                throw new ResourceNotFoundException("The user does not exist");
            }
            if (String.IsNullOrEmpty(password) || !user.Password.Equals(password)) {
                throw new InvalidResourceException("Invalid Password");
            }
            var _userId = user.Id;
            Session session = _sessionRepository.GetOneByExpression(s => s.UserId == _userId);
            if (session == null) {
                var token = Guid.NewGuid();
                Session newSession = new Session { Token = token, UserId = _userId };
                _sessionRepository.InsertOne(newSession);
                _sessionRepository.Save();
                return new Authorization { Token = token, Role = user.Role.Name, UserName = user.UserName };
            }
            return new Authorization { Token = session.Token, Role = user.Role.Name, UserName = user.UserName };
        }

        public bool IsTokenValid(string token)
        {
            var guidToken = new Guid(token);
            Session session = _sessionRepository.GetOneByExpression(x => x.Token == guidToken);
            if (session == null) return false;
            return true;
        }

        public bool IsRoleValid(string[] roles, string token)
        {
            var guidToken = new Guid(token);
            Session session = _sessionRepository.GetOneByExpression(x => x.Token == guidToken);
            var userId = session.UserId;
            User user = _userRepository.GetOneDetailByExpression(x => x.Id == userId);
            if (user == null) return false;
            foreach(string role in roles)
            {
                if (user.Role.Name.ToLower() == role.ToLower()) return true;
            }
            return false;
        }
    }
}