using CatalogServices;
using CatalogServices.DAL;
using CatalogServices.DAL.Interfaces;
using CatalogServices.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//meregister service menggunakan DI
builder.Services.AddScoped<ICategory, CategoryDapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/categories", (ICategory categoryDal) =>
{
    List<CategoryDTO> categoriesDto = new List<CategoryDTO>();
    var categories = categoryDal.GetAll();
    foreach (var category in categories)
    {
        categoriesDto.Add(new CategoryDTO
        {
            CategoryName = category.CategoryName
        });
    }
    return Results.Ok(categoriesDto);
});

app.MapGet("/api/categories/{id}", (ICategory categoryDal, int id) =>
{
    CategoryDTO categoryDto = new CategoryDTO();
    var category = categoryDal.GetById(id);
    if (category == null)
    {
        return Results.NotFound();
    }
    categoryDto.CategoryName = category.CategoryName;
    return Results.Ok(categoryDto);
});

app.MapGet("/api/categories/search/{name}", (ICategory categoryDal, string name) =>
{
    List<CategoryDTO> categoriesDto = new List<CategoryDTO>();
    var categories = categoryDal.GetByName(name);
    foreach (var category in categories)
    {
        categoriesDto.Add(new CategoryDTO
        {
            CategoryName = category.CategoryName
        });
    }
    return Results.Ok(categoriesDto);
});

app.MapPost("/api/categories", (ICategory categoryDal, CategoryCreateDto categoryCreateDto) =>
{
    try
    {
        Category category = new Category
        {
            CategoryName = categoryCreateDto.CategoryName
        };
        categoryDal.Insert(category);

        //return 201 Created
        return Results.Created($"/api/categories/{category.CategoryID}", category);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/api/categories", (ICategory categoryDal, CategoryUpdateDto categoryUpdateDto) =>
{
    try
    {
        var category = new Category
        {
            CategoryID = categoryUpdateDto.CategoryID,
            CategoryName = categoryUpdateDto.CategoryName
        };
        categoryDal.Update(category);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapDelete("/api/categories/{id}", (ICategory categoryDal, int id) =>
{
    try
    {
        categoryDal.Delete(id);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();
