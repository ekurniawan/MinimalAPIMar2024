using System.Data.SqlClient;
using CatalogServices.DAL.Interfaces;
using CatalogServices.Models;
using Dapper;

namespace CatalogServices;

public class CategoryDapper : ICategory
{
    private readonly IConfiguration _config;
    public CategoryDapper(IConfiguration config)
    {
        _config = config;
    }
    private string GetConnectionString()
    {
        return _config.GetConnectionString("DefaultConnection");
        //return @"Data Source=.\SQLEXPRESS;Initial Catalog=CatalogDb;Integrated Security=True";
        //return @"Server=localhost,1433;Initial Catalog=CatalogDb;User ID=sa;Password=Indonesia@2023;TrustServerCertificate=True;";
    }
    public void Delete(int id)
    {
        using (SqlConnection conn = new SqlConnection(GetConnectionString()))
        {
            var strSql = @"DELETE FROM Categories 
                               WHERE CategoryID = @CategoryID";
            var param = new { CategoryID = id };
            try
            {
                conn.Execute(strSql, param);
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error: {ex.Message}");
            }
        }
    }

    public IEnumerable<Category> GetAll()
    {
        using (SqlConnection conn = new SqlConnection(GetConnectionString()))
        {
            var strSql = @"SELECT * FROM Categories order by CategoryName";
            var categories = conn.Query<Category>(strSql);
            return categories;
        }
    }

    public Category GetById(int id)
    {
        using (SqlConnection conn = new SqlConnection(GetConnectionString()))
        {
            var strSql = @"SELECT * FROM Categories
                            WHERE CategoryID = @CategoryID";
            var param = new { CategoryID = id };
            var category = conn.QueryFirstOrDefault<Category>(strSql, param);
            if (category == null)
            {
                throw new ArgumentException("Data tidak ditemukan");
            }
            return category;
        }
    }

    public IEnumerable<Category> GetByName(string name)
    {
        using (SqlConnection conn = new SqlConnection(GetConnectionString()))
        {
            var strSql = @"SELECT * FROM Categories
                            WHERE CategoryName LIKE @CategoryName";
            var param = new { CategoryName = $"%{name}%" };
            var categories = conn.Query<Category>(strSql, param);
            return categories;
        }
    }

    public void Insert(Category obj)
    {
        using (SqlConnection conn = new SqlConnection(GetConnectionString()))
        {
            var strSql = @"INSERT INTO Categories (CategoryName) VALUES (@CategoryName)";
            var param = new { CategoryName = obj.CategoryName };
            try
            {
                conn.Execute(strSql, param);
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error: {ex.Message}");
            }
        }
    }

    public void Update(Category obj)
    {
        using (SqlConnection conn = new SqlConnection(GetConnectionString()))
        {
            var strSql = @"UPDATE Categories SET CategoryName = @CategoryName 
                            WHERE CategoryID = @CategoryID";
            var param = new { CategoryName = obj.CategoryName, CategoryID = obj.CategoryID };
            try
            {
                conn.Execute(strSql, param);
            }
            catch (SqlException sqlEx)
            {
                throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error: {ex.Message}");
            }
        }
    }
}
