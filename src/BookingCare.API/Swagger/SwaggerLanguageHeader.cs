using BookingCare.Domain.Common;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BookingCare.API.Swagger;

public class SwaggerLanguageHeader : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "Accept-Language",
            In = ParameterLocation.Header,
            Required = false,
            Examples = new Dictionary<string, OpenApiExample>
                {
                    { "English", new OpenApiExample{ Value = new OpenApiString(LanguageCode.English) } },
                    { "Vietnamese", new OpenApiExample{ Value = new OpenApiString(LanguageCode.VietNamese) } },
                },
            Schema = new OpenApiSchema
            {
                Type = "string"
            }
        });
    }
}