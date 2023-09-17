using Microsoft.EntityFrameworkCore;
using PharmaGo.DataAccess;
using PharmaGo.DataAccess.Repositories;
using PharmaGo.Domain.Entities;

namespace PharmaGo.Test.DataAccess.Test
{
    [TestClass]
    public class UsersRepositoryTests
    {

        private UsersRepository _userRepository;
        private User _user;

        [TestInitialize]
        public void InitializeTest()
        {
            _user = new User { Id = 1, 
                               UserName = "pedro001", 
                               Email = "pedro@gmail.com", 
                               Address = "Av. Italia 002", 
                               Password = "Abcdef12345678.", 
                               RegistrationDate = new DateTime(2022, 10, 4, 14, 0, 0)};
        }

        [TestCleanup]
        public void CleanUp()
        {
            var options = new DbContextOptionsBuilder<PharmacyGoDbContext>()
                .UseInMemoryDatabase(databaseName: "PharmaGoDb")
                .Options;

            using (var context = new PharmacyGoDbContext(options))
            {
                _userRepository = new UsersRepository(context);
                if (_userRepository.Exists(_user))
                {
                    _userRepository.DeleteOne(_user);
                    _userRepository.Save();
                }
            }
        }

        [TestMethod]
        public void TestInsertOne()
        {
            var options = new DbContextOptionsBuilder<PharmacyGoDbContext>()
            .UseInMemoryDatabase(databaseName: "PharmaGoDb")
            .Options;

            using (var context = new PharmacyGoDbContext(options))
            {
                _userRepository = new UsersRepository(context);
                if (!_userRepository.Exists(_user))
                {
                    _userRepository.InsertOne(_user);
                    _userRepository.Save();
                }
                Assert.IsTrue(_userRepository.Exists(_user));
            }
        }

        [TestMethod]
        public void TestDeleteOne()
        {
            var options = new DbContextOptionsBuilder<PharmacyGoDbContext>()
              .UseInMemoryDatabase(databaseName: "PharmaGoDb")
              .Options;

            bool exist = true;
            using (var context = new PharmacyGoDbContext(options))
            {
                _userRepository = new UsersRepository(context);
                _userRepository.InsertOne(_user);
                _userRepository.Save();
                _userRepository.DeleteOne(_user);
                _userRepository.Save();

                exist = _userRepository.Exists(_user);
            }

            Assert.IsFalse(exist);
        }

        [TestMethod]
        public void Test_Get_One_Detail_By_Expression()
        {
            var options = new DbContextOptionsBuilder<PharmacyGoDbContext>()
              .UseInMemoryDatabase(databaseName: "PharmaGoDb")
              .Options;

            bool exist = true;
            using (var context = new PharmacyGoDbContext(options))
            {
                _userRepository = new UsersRepository(context);
                _userRepository.InsertOne(_user);
                _userRepository.Save();
                User user = _userRepository.GetOneDetailByExpression(x => x.UserName == "pedro001");
                exist = _userRepository.Exists(user);
            }

            Assert.IsTrue(exist);
        }


        [TestMethod]
        public void Test_Get_Users()
        {
            var options = new DbContextOptionsBuilder<PharmacyGoDbContext>()
            .UseInMemoryDatabase(databaseName: "PharmaGoDb")
            .Options;

            using (var context = new PharmacyGoDbContext(options))
            {
                _userRepository = new UsersRepository(context);
                if (!_userRepository.Exists(_user))
                {
                    _userRepository.InsertOne(_user);
                    _userRepository.Save();
                }
                var users = _userRepository.GetAllByExpression(p => p.Id != 0);
                Assert.IsNotNull(users);
            }
        }



    }
}
