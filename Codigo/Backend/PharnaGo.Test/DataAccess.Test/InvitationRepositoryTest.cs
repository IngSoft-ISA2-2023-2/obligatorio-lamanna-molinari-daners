using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PharmaGo.DataAccess;
using PharmaGo.DataAccess.Repositories;
using PharmaGo.Domain.Entities;

namespace PharmaGo.Test.DataAccess.Test
{
	[TestClass]
	public class InvitationRepositoryTest
	{
        private Invitation _invitation;
        private PharmacyGoDbContext _context;
        private DbContextOptions<PharmacyGoDbContext> _options;
        private InvitationRepository _invitationRepository;

        [TestInitialize]
        public void Setup()
        {
            this._options = new DbContextOptionsBuilder<PharmacyGoDbContext>()
                .UseInMemoryDatabase(databaseName: "PharmaGo").Options;
            this._context = new PharmacyGoDbContext(this._options);

            _invitation = new Invitation()
            {
                Id = 1,
                UserName = "jcastro@test.com.uy",
                UserCode = Guid.NewGuid().ToString(),
                Pharmacy = new Pharmacy()
                {
                    Id = 1,
                    Name = "PharmaGo",
                    Address = "Montevideo"
                }
                ,
                Role = new Role()
                {
                    Id = 1,
                    Name = "Administrator"
                },
                Created = DateTime.Now
            };
        }

        [TestCleanup]
        public void CleanUp()
        {
            using (var context = new PharmacyGoDbContext(this._options))
            {
                _invitationRepository = new InvitationRepository(context);
                if (_invitationRepository.Exists(_invitation))
                {
                    _invitationRepository.DeleteOne(_invitation);
                    _invitationRepository.Save();
                }
            }

            this._context.Database.EnsureDeleted();
        }

        [TestMethod]
        public void AddInvitation_WithNullPharmacy_ShouldAddNewEntryInDatabase()
        {
            var invitation = new Invitation()
            {
                Id = 1,
                UserName = "jcastro@test.com.uy",
                UserCode = Guid.NewGuid().ToString(),
                Role = new Role()
                {
                    Id = 1,
                    Name = "Administrator"
                },
                Created = DateTime.Now
            };
            //Arrange
            InvitationRepository invitationRepository = new InvitationRepository(this._context);

            //Act
            invitationRepository.InsertOne(invitation);
            invitationRepository.Save();

            //Assert
            var invitationsQuantity = invitationRepository.CountAsync();
            Assert.AreEqual(1, invitationsQuantity);
        }

        [TestMethod]
        public void AddInvitation_ShouldAddNewEntryInDatabase()
        {
            //Arrange
            InvitationRepository invitationRepository = new InvitationRepository(this._context);

            //Act
            invitationRepository.InsertOne(_invitation);
            invitationRepository.Save();

            //Assert
            var invitationsQuantity = invitationRepository.CountAsync();
            Assert.AreEqual(1, invitationsQuantity);
        }

        [TestMethod]
        public void UpdateInvitation_ShouldUpdateInvitation()
        {
            //Arrange
            InvitationRepository invitationRepository = new InvitationRepository(this._context);

            //Act
            var notExpectedValue = _invitation.UserCode;
            invitationRepository.InsertOne(_invitation);
            invitationRepository.Save();
            _invitation.UserCode = Guid.NewGuid().ToString();
            invitationRepository.UpdateOne(_invitation);

            Invitation updatedInvitation = invitationRepository
                .GetOneByExpression(expre => expre.Id == _invitation.Id);

            //Assert
            Assert.AreNotEqual(notExpectedValue, updatedInvitation.UserCode);
        }

        [TestMethod]
        public void GetOneDetailByExpression_ShouldAddNewEntryInDatabase()
        {
            using (var context = new PharmacyGoDbContext(this._options))
            {
                _invitationRepository = new InvitationRepository(context);

                var invitationId = _invitation.Id;
                _invitationRepository.InsertOne(_invitation);
                _invitationRepository.Save();
                Invitation invitation = _invitationRepository.GetOneDetailByExpression(u => u.Id == invitationId);
                
                // Assert
                Assert.AreEqual(invitation.Id, _invitation.Id);
            }
        }

        [TestMethod]
        public void GetAllBasicInvitations_ShouldReturnAllInvitations()
        {
            //Arrange
            InvitationRepository invitationRepository = new InvitationRepository(this._context);

            //Act
            invitationRepository.InsertOne(_invitation);
            invitationRepository.Save();

            var invitations = invitationRepository
                .GetAllBasicByExpression(expre => expre.Id == _invitation.Id);

            //Assert
            Assert.AreEqual(1, invitations.Count());
        }
    }
}

