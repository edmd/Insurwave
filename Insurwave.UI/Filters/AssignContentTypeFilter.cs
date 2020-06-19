using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Insurwave.UI.Filters
{
    internal class AssignContentTypeFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Responses.ContainsKey("200"))
            {
                operation.Responses.Clear();
            }

            var data = new OpenApiResponse
            {
                Description = "Ok",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType()
                }
            };

            operation.Responses.Add("200", data);
        }
    }
}
