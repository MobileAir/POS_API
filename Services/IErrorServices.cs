using System.Collections.Generic;
using Services.DTOs;

namespace Services
{
    /// <summary>
    /// Product Service Contract
    /// </summary>
    public interface IErrorServices
    {
        ErrorDTO GetById(int id);
        IEnumerable<ErrorDTO> GetAll();
        ErrorDTO Create(ErrorDTO p);
        bool Delete(int id);
    }
}
