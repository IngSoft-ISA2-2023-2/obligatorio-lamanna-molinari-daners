using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PharmaGo.BusinessLogic;
using PharmaGo.DataAccess;
using PharmaGo.DataAccess.Repositories;
using PharmaGo.Domain.Entities;
using PharmaGo.IBusinessLogic;
using PharmaGo.IDataAccess;

namespace PharmaGo.Factory
{
    public static class ServiceFactory
    {

        public static void RegisterBusinessLogicServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddScoped<ILoginManager, LoginManager>();
            serviceCollection.AddScoped<IUsersManager, UsersManager>();
            serviceCollection.AddScoped<IInvitationManager, InvitationManager>();
            serviceCollection.AddScoped<IStockRequestManager, StockRequestManager>();
            serviceCollection.AddScoped<IPurchasesManager, PurchasesManager>();
            serviceCollection.AddScoped<IPharmacyManager, PharmacyManager>();
            serviceCollection.AddScoped<IDrugManager, DrugManager>();
            serviceCollection.AddScoped<IPresentationManager, PresentationManager>();
            serviceCollection.AddScoped<IUnitMeasureManager, UnitMeasureManager>();
            serviceCollection.AddScoped<IExportManager, ExportManager>();
            serviceCollection.AddScoped<IRoleManager, RoleManager>();
        }

        public static void RegisterDataAccessServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddScoped<IRepository<User>, UsersRepository>();
            serviceCollection.AddScoped<IRepository<Session>, SessionRepository>();
            serviceCollection.AddScoped<IRepository<Invitation>, InvitationRepository>();
            serviceCollection.AddScoped<IRepository<Role>, RoleRepository>();
            serviceCollection.AddScoped<IRepository<StockRequest>, StockRequestRepository>();
            serviceCollection.AddScoped<IRepository<Pharmacy>, PharmacyRepository>();
            serviceCollection.AddScoped<IRepository<UnitMeasure>, UnitMeasureRepository>();
            serviceCollection.AddScoped<IRepository<Purchase>, PurchasesRepository>();
            serviceCollection.AddScoped<IRepository<Presentation>, PresentationRepository>();
            serviceCollection.AddScoped<IRepository<Drug>, DrugRepository>();
            serviceCollection.AddScoped<IRepository<PurchaseDetail>, PurchasesDetailRepository>();
            serviceCollection.AddScoped<IRepository<Role>, RoleRepository>();

            serviceCollection.AddDbContext<DbContext, PharmacyGoDbContext>(o => o.UseSqlServer(configuration.GetConnectionString("PharmaGo")));
        }

    }
}