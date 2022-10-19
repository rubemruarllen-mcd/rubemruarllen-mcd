using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;

namespace PollingPoc.Swagger
{
    /// <summary>
    ///     Default swagger headers
    /// </summary>
    internal class AddHeaderParameters : IOperationFilter
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AddHeaderParameters" /> class.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        public AddHeaderParameters(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        ///     Applies the default headers
        /// </summary>
        /// <param name="operation">A instance of <see cref="OpenApiOperation" />.</param>
        /// <param name="context">A instance of <see cref="OperationFilterContext" />.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var headers = _configuration.GetSection("Headers:Include").GetChildren();
            operation.Parameters ??= new List<OpenApiParameter>();

            foreach (var header in headers)
                operation.Parameters.Add(
                    new OpenApiParameter
                    {
                        Name = header.Value,
                        In = ParameterLocation.Header,
                        Schema = new OpenApiSchema
                        {
                            Type = "string",
                            Default = new OpenApiString($"some-{header.Value}")
                        }
                    });
        }
    }
}
