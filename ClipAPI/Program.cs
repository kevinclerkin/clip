using Rembg;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Set environment variable for pythonnet
var pythonHome = @"C:\Users\Kevin\AppData\Local\Programs\Python\Python38";
var pythonDll = Path.Combine(pythonHome, "python38.dll");
Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pythonDll);

// Define the endpoint for image processing
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
        var file = form.Files.GetFile("image");

        if (file == null)
        {
            return Results.BadRequest("No image file provided");
        }

        // Process the image using Rembg library
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
