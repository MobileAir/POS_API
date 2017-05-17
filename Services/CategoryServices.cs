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
    /// Offers services for category specific CRUD operations
    /// </summary>
    public class CategoryServices : ICategoryServices
    {
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public CategoryServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Fetches category details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CategoryDTO GetById(int id)
        {
            var category = _unitOfWork.CategoryRepository.GetByID(id);
            if (category != null)
            {
                Mapper.CreateMap<Category, CategoryDTO>();
                var productModel = Mapper.Map<Category, CategoryDTO>(category);
                return productModel;
            }
            return null;
        }

        /// <summary>
        /// Fetches all the categories.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CategoryDTO> GetAll()
        {
            var categories = _unitOfWork.CategoryRepository.GetAllAsQueryable();
            if (categories.Any())
            {
                Mapper.CreateMap<Category, CategoryDTO>();
                var productsModel = Mapper.Map<List<Category>, List<CategoryDTO>>(categories.ToList());
                return productsModel;
            }
            return null;
        }

        /// <summary>
        /// Creates a category
        /// </summary>
        /// <param name="categoryDto"></param>
        /// <returns></returns>
        public CategoryDTO Create(CategoryDTO categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name
            };
            _unitOfWork.CategoryRepository.Insert(category);
            _unitOfWork.Save();
            categoryDto.ID = category.ID;
            return categoryDto;
        }

        /// <summary>
        /// Updates a category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoryDto"></param>
        /// <returns></returns>
        public bool Update(int id, CategoryDTO categoryDto)
        {
            var success = false;
            if (categoryDto != null)
            {

                var category = _unitOfWork.CategoryRepository.GetByID(id);
                if (category != null)
                {
                    category.Name = categoryDto.Name;
                    _unitOfWork.CategoryRepository.Update(category);
                    _unitOfWork.Save();
                    success = true;
                }

            }
            return success;
        }

        /// <summary>
        /// Deletes a particular category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            var success = false;
            if (id > 0)
            {
                var category = _unitOfWork.CategoryRepository.GetByID(id);
                if (category != null)
                {

                    _unitOfWork.CategoryRepository.Delete(category);
                    _unitOfWork.Save();
                    success = true;
                }
            }
            return success;
        }
    }
}
