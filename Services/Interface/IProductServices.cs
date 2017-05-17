using System.Collections.Generic;
using Services.DTOs;

namespace Services.Interface
{
    /// <summary>
    /// Product Service Contract
    /// </summary>
    public interface IProductServices
    {
        ProductDTO GetById(int id);
        IEnumerable<ProductDTO> GetAll();
        IEnumerable<ProductDTO> GetByCategory(int id);
        ProductDTO Create(ProductDTO p);
        bool Update(int id,ProductDTO p);
        bool Delete(int id);
    }
}
