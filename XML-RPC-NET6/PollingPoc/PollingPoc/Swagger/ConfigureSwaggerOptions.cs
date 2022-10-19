using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;

namespace PollingPoc.Swagger
{
    /// <summary>
    ///     Creates a new swagger documentation for each version present in the API.
    /// </summary>
    internal class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IWebHostEnvironment _env;
        private readonly IApiVersionDescriptionProvider _provider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConfigureSwaggerOptions" /> class.
        /// </summary>
        /// <param name="provider">Instance of <see cref="IApiVersionDescriptionProvider" />.</param>
        /// <param name="env">A instance of <see cref="IWebHostEnvironment" />.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IWebHostEnvironment env)
        {
            _provider = provider;
            _env = env;
        }

        /// <summary>
        ///     Configures swagger
        /// </summary>
        /// <param name="options">A instance of <see cref="SwaggerGenOptions" />.</param>
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
                options.SwaggerDoc(
                    description.GroupName,
                    CreateVersionInfo(description));
        }

        /// <summary>
        ///     Creates the version information
        /// </summary>
        /// <param name="description">API description</param>
        /// <returns>API Info</returns>
        private OpenApiInfo CreateVersionInfo(
            ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = _env.ApplicationName,
                Version = description.ApiVersion.ToString()
            };

            if (description.IsDeprecated)
                info.Description += " This API version has been deprecated.";

            return info;
        }
    }
}
