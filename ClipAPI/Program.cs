using Rembg;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<Remover>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Clip",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

else
{
    // Optionally, enable Swagger in non-development environments
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}


app.UseHttpsRedirection();

app.UseCors("Clip");

// Set environment variable for pythonnet
var pythonHome = @"C:\Users\Kevin\AppData\Local\Programs\Python\Python38";
var pythonDll = Path.Combine(pythonHome, "python38.dll");
Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pythonDll);

// Endpoint for image processing
app.MapPost("/process-image", async (HttpContext context) =>
{
    try
    {
        // Ensure the request contains form data
        if (!context.Request.HasFormContentType)
        {
            return Results.BadRequest("Invalid form data");
        }

        var form = await context.Request.ReadFormAsync();
        var file = form.Files.GetFile("file");

        if (file == null)
        {
            return Results.BadRequest("No image file provided");
        }

        
        using (var inputImageStream = file.OpenReadStream())
        {
             var remover = new Remover();
             using (var resultStream = remover.RemoveBackground(inputImageStream))
             {
                 var ms = new MemoryStream();
                 await resultStream.CopyToAsync(ms);
                 ms.Position = 0;
                 return Results.File(ms, "image/png");
                 
             }
        }
        
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})

.WithName("ProcessImage");


app.Run();
