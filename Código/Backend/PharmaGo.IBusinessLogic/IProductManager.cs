using ExportationModel.ExportDomain;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaGo.IBusinessLogic
{
    public interface IProductManager
    {
        Product GetById(int id);
        Product Create(Product product, string token);
        Product Update(int id, Product drug);
        void Delete(int id);
    }
}
