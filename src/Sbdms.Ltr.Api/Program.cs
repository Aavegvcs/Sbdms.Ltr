using System.IO.Compression;
using Microsoft.AspNetCore.ResponseCompression;
using Asp.Versioning;
using Asp.Versioning.Conventions;
using Scalar.AspNetCore;
using Sbdms.Ltr.Core.Feature;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
        options.Providers.Add<BrotliCompressionProvider>();
        options.Providers.Add<GzipCompressionProvider>();
    });

    builder.Services.AddControllers();
    builder.Services.AddHttpClient();
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    }).AddMvc(options =>
    {
        options.Conventions.Add(new VersionByNamespaceConvention());
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

    builder.Services.AddOpenApi("v1");
    builder.Services.AddOpenApi("v2");

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy
                .SetIsOriginAllowed(_ => true)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

  }

var app = builder.Build();
{
    app.UseResponseCompression();

    if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
    {
        app.MapOpenApi();
        app.MapScalarApiReference("/api-docs", options =>
        {
            options.WithTitle("Commutr - Routing Services");
            options.AddDocument("v1", "Version 1", routePattern: "openapi/{documentName}.json")
                .AddDocument("v2", "Version 2", routePattern: "openapi/{documentName}.json");
        });
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.UseCors("AllowAll");
    app.Run();
}