using CatalogServices.Models;

namespace CatalogServices.DAL.Interfaces
{
    public interface ICategory : ICrud<Category>
    {
        IEnumerable<Category> GetByName(string name);
    }
}