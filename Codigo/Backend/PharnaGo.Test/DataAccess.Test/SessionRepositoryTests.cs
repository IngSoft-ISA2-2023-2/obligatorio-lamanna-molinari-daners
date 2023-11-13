using Microsoft.EntityFrameworkCore;
using PharmaGo.DataAccess;
using PharmaGo.DataAccess.Repositories;
using PharmaGo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaGo.Test.DataAccess.Test
{
    [TestClass]
    public class SessionRepositoryTests
    {

        SessionRepository _sessionRepository;
        Session _session;
        Guid _token = new Guid("0bc35031-0bf8-43c9-9cca-5ef102903798");

        [TestInitialize]
        public void InitializeTest()
        {
            _session = new Session { Id = 4, Token = _token, UserId = 3};
        }

        [TestCleanup]
        public void CleanUp()
        {
            var options = new DbContextOptionsBuilder<PharmacyGoDbContext>()
                .UseInMemoryDatabase(databaseName: "PharmaGoDb")
                .Options;

            using (var context = new PharmacyGoDbContext(options))
            {
                _sessionRepository = new SessionRepository(context);
                if (_sessionRepository.Exists(_session))
                {
                    _sessionRepository.DeleteOne(_session);
                    _sessionRepository.Save();
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
                _sessionRepository = new SessionRepository(context);
                if (!_sessionRepository.Exists(_session))
                {
                    _sessionRepository.InsertOne(_session);
                    _sessionRepository.Save();
                }
                Assert.IsTrue(_sessionRepository.Exists(_session));
            }
        }



    }
}
