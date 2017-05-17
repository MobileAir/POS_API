using System.Collections.Generic;
using Services.DTOs;

namespace Services.Interface
{
    /// <summary>
    /// Category Service Contract
    /// </summary>
    public interface ICategoryServices
    {
        CategoryDTO GetById(int id);
        IEnumerable<CategoryDTO> GetAll();
        CategoryDTO Create(CategoryDTO p);
        bool Update(int id,CategoryDTO p);
        bool Delete(int id);
    }
}
