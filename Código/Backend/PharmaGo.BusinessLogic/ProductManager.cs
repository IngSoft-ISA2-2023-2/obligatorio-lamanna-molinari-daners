using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;
using PharmaGo.Exceptions;
using PharmaGo.IBusinessLogic;
using PharmaGo.IDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaGo.BusinessLogic
{
    public class ProductManager : IProductManager
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Pharmacy> _pharmacyRepository;
        private readonly IRepository<Session> _sessionRepository;
        private readonly IRepository<User> _userRepository;

        public ProductManager(IRepository<Product> productRepository,
                           IRepository<Pharmacy> pharmacyRepository,
                           IRepository<Session> sessionRespository,
                           IRepository<User> userRespository)
        {
            _productRepository = productRepository;
            _pharmacyRepository = pharmacyRepository;
            _sessionRepository = sessionRespository;
            _userRepository = userRespository;
        }

        public Product Create(Product product, string token)
        {
            if (product == null)
            {
                throw new ResourceNotFoundException("Please create a product before inserting it.");
            }
            product.ValidOrFail();

            var guidToken = new Guid(token);
            Session session = _sessionRepository.GetOneByExpression(s => s.Token == guidToken);
            var userId = session.UserId;
            User user = _userRepository.GetOneDetailByExpression(u => u.Id == userId);

            Pharmacy pharmacyofProduct = _pharmacyRepository.GetOneByExpression(p => p.Name == user.Pharmacy.Name);
            if (pharmacyofProduct == null)
            {
                throw new ResourceNotFoundException("The pharmacy of the product does not exist.");
            }

            if (_productRepository.Exists(d => d.Code == product.Code && d.Pharmacy.Name == pharmacyofProduct.Name))
            {
                throw new InvalidResourceException("The product already exists in that pharmacy.");
            }
            
            product.Pharmacy.Id = pharmacyofProduct.Id;
            _productRepository.InsertOne(product);
            _productRepository.Save();
            return product;

        }

        public void Delete(int id)
        {
            var productSaved = _productRepository.GetOneByExpression(d => d.Id == id);
            if (productSaved == null)
            {
                throw new ResourceNotFoundException("The product to delete does not exist.");
            }
            productSaved.Deleted = true;
            _productRepository.UpdateOne(productSaved);
            _productRepository.Save();
        }

        public IEnumerable<Product> GetAll(ProductSearchCriteria productSearchCriteria)
        {
            Product productToSearch = new Product();
            if (productSearchCriteria.PharmacyId == null)
            {
                productToSearch.Name = productSearchCriteria.Name;
            }
            else
            {
                Pharmacy pharmacySaved = _pharmacyRepository.GetOneByExpression(p => p.Id == productSearchCriteria.PharmacyId);
                if (pharmacySaved != null)
                {
                    productToSearch.Name = productSearchCriteria.Name;
                    productToSearch.Pharmacy = pharmacySaved;
                }
                else
                {
                    throw new ResourceNotFoundException("The pharmacy to get products of does not exist.");
                }
            }
            return _productRepository.GetAllByExpression(productSearchCriteria.Criteria(productToSearch));

        }

        public Product GetById(int id)
        {
            Product retrievedproduct = _productRepository.GetOneByExpression(d => d.Id == id);
            if (retrievedproduct == null)
            {
                throw new ResourceNotFoundException("The Product does not exist.");
            }

            return retrievedproduct;
        }

        public Product Update(int id, Product updateProduct, string token)
        {
            if (updateProduct == null)
            {
                throw new ResourceNotFoundException("The updated product is invalid.");
            }
          
            var productSaved = _productRepository.GetOneByExpression(d => d.Id == id);
            if (productSaved == null)
            {
                throw new ResourceNotFoundException("The product to update does not exist.");
            }

            var guidToken = new Guid(token);
            Session session = _sessionRepository.GetOneByExpression(s => s.Token == guidToken);
            var userId = session.UserId;
            User user = _userRepository.GetOneDetailByExpression(u => u.Id == userId);

            if (_productRepository.Exists(d => d.Code == updateProduct.Code && d.Pharmacy.Name == user.Pharmacy.Name))
            {
                throw new InvalidResourceException("A product with that code already exists in that pharmacy.");
            }
            if (updateProduct.Code != "")
            {
                productSaved.Code = updateProduct.Code;
            }
            if (updateProduct.Name != "")
            {
                productSaved.Name = updateProduct.Name;
            }
            if (updateProduct.Price != 0)
            {
                productSaved.Price = updateProduct.Price;
            }
            if (updateProduct.Description != "")
            {
                productSaved.Description = updateProduct.Description;
            }
            productSaved.ValidOrFail();
            _productRepository.UpdateOne(productSaved);
            _productRepository.Save();
            return productSaved;
        }
    }
}
