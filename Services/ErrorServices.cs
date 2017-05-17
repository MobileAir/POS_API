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
    /// Offers services for error specific CRUD operations
    /// </summary>
    public class ErrorServices : IErrorServices
    {
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public ErrorServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Fetches error details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ErrorDTO GetById(int id)
        {
            var error = _unitOfWork.ErrorRepository.GetByID(id);
            if (error != null)
            {
                Mapper.CreateMap<Error, ErrorDTO>();
                var errorModel = Mapper.Map<Error, ErrorDTO>(error);
                return errorModel;
            }
            return null;
        }

        /// <summary>
        /// Fetches all the errors.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ErrorDTO> GetAll()
        {
            var errors = _unitOfWork.ErrorRepository.GetAll().ToList();
            if (errors.Any())
            {
                Mapper.CreateMap<Error, ErrorDTO>();
                var productsModel = Mapper.Map<List<Error>, List<ErrorDTO>>(errors);
                return productsModel;
            }
            return null;
        }

        /// <summary>
        /// Creates a error
        /// </summary>
        /// <param name="errorDto"></param>
        /// <returns></returns>
        public ErrorDTO Create(ErrorDTO errorDto)
        {
            Mapper.CreateMap<ErrorDTO, Error>();
            var error = Mapper.Map<ErrorDTO, Error>(errorDto);
            _unitOfWork.ErrorRepository.Insert(error);
            _unitOfWork.Save();
            errorDto.ID = error.ID;
            return errorDto;
        }

        /// <summary>
        /// Deletes a particular error
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            var success = false;
            if (id > 0)
            {
                var error = _unitOfWork.ErrorRepository.GetByID(id);
                if (error != null)
                {

                    _unitOfWork.ErrorRepository.Delete(error);
                    _unitOfWork.Save();
                    success = true;
                }
            }
            return success;
        }
    }
}
