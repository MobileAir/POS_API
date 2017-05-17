using System.Collections.Generic;
using Services.DTOs;

namespace Services.Interface
{
    /// <summary>
    /// Error Service Contract
    /// </summary>
    public interface IErrorServices
    {
        ErrorDTO GetById(int id);
        IEnumerable<ErrorDTO> GetAll();
        ErrorDTO Create(ErrorDTO p);
        bool Delete(int id);
    }
}
