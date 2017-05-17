using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DataAccess.Models;
using DataAccess.UnitOfWork;
using Services.DTOs;
using Services.Interface;

namespace Services
{
    /// <summary>
    /// Offers services for product specific CRUD operations
    /// </summary>
    public class ProductServices : IProductServices
    {
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public ProductServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Fetches product details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductDTO GetById(int id)
        {
            var product = _unitOfWork.ProductRepository.GetByID(id);
            if (product != null)
            {
                Mapper.CreateMap<Product, ProductDTO>();
                var productModel = Mapper.Map<Product, ProductDTO>(product);
                return productModel;
            }
            return null;
        }

        /// <summary>
        /// Fetches all the products.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductDTO> GetAll()
        {
            var products = _unitOfWork.ProductRepository.GetAllAsQueryable();
            if (products.Any())
            {
                Mapper.CreateMap<Product, ProductDTO>();
                var productsModel = Mapper.Map<List<Product>, List<ProductDTO>>(products.ToList());
                return productsModel;
            }
            return null;
        }

        /// <summary>
        /// Fetches all by CategoryID
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductDTO> GetByCategory(int id)
        {
            var products = _unitOfWork.ProductRepository.GetManyQueryable(x => x.CategoryID == id);
            if (products.Any())
            {
                Mapper.CreateMap<Product, ProductDTO>();
                var productsModel = Mapper.Map<List<Product>, List<ProductDTO>>(products.ToList());
                return productsModel;
            }
            return null;
        }

        /// <summary>
        /// Creates a product
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        public ProductDTO Create(ProductDTO productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                CategoryID = productDto.CategoryID
            };
            _unitOfWork.ProductRepository.Insert(product);
            _unitOfWork.Save();
            productDto.ID = product.ID;
            return productDto;
        }

        /// <summary>
        /// Updates a product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productDto"></param>
        /// <returns></returns>
        public bool Update(int id, ProductDTO productDto)
        {
            var success = false;
            if (productDto != null)
            {

                var product = _unitOfWork.ProductRepository.GetByID(id);
                if (product != null)
                {
                    product.Name = productDto.Name;
                    _unitOfWork.ProductRepository.Update(product);
                    _unitOfWork.Save();
                    success = true;
                }

            }
            return success;
        }

        /// <summary>
        /// Deletes a particular product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            var success = false;
            if (id > 0)
            {
                var product = _unitOfWork.ProductRepository.GetByID(id);
                if (product != null)
                {

                    _unitOfWork.ProductRepository.Delete(product);
                    _unitOfWork.Save();
                    success = true;
                }
            }
            return success;
        }
    }
}
