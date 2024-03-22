using CatalogServices.DAL;
using CatalogServices.DAL.Interfaces;
using CatalogServices.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//meregister service menggunakan DI
builder.Services.AddScoped<ICategory, CatagoryDAL>();

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
    var categories = categoryDal.GetAll();
    return Results.Ok(categories);
});

app.MapGet("/api/categories/{id}", (ICategory categoryDal, int id) =>
{
    var category = categoryDal.GetById(id);
    if (category == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(category);
});

app.MapGet("/api/categories/search/{name}", (ICategory categoryDal, string name) =>
{
    var categories = categoryDal.GetByName(name);
    return Results.Ok(categories);
});

app.MapPost("/api/categories", (ICategory categoryDal, Category category) =>
{
    try
    {
        categoryDal.Insert(category);

        //return 201 Created
        return Results.Created($"/api/categories/{category.CategoryID}", category);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/api/categories", (ICategory categoryDal, Category category) =>
{
    try
    {
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
