using System.Collections.Generic;
using Services.DTOs;

namespace Services
{
    /// <summary>
    /// Product Service Contract
    /// </summary>
    public interface IProductServices
    {
        ProductDTO GetById(int id);
        IEnumerable<ProductDTO> GetAll();
        ProductDTO Create(ProductDTO p);
        bool Update(int id,ProductDTO p);
        bool Delete(int id);
    }
}
