using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;

namespace PollingPoc.Swagger
{
    /// <summary>
    ///     Creates Swagger documentation filters that can modify Swagger documents after they're initially generated  
    /// </summary>
    public class SwaggerDocumentFilter : IDocumentFilter
    {
        /// <summary>
        ///     Applies the filters to the Swagger documents
        /// </summary>
        /// <param name="swaggerDoc">The Swagger document to be changed</param>
        /// <param name="context">Swagger document context</param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            ApplyInfo(swaggerDoc);
            ApplyTags(swaggerDoc);
        }

        /// <summary>
        ///     Applies info filters
        /// </summary>
        /// <param name="swaggerDoc">The Swagger document to be changed</param>
        private static void ApplyInfo(OpenApiDocument swaggerDoc)
        {
            swaggerDoc.Info = new OpenApiInfo
            {
                Title = "PollingPoc",
                Description = File.ReadAllText("./Resources/Swagger/SwaggerDocumentFilterInfoDescription.txt"),
                Version = "1"
            };
        }

        /// <summary>
        ///     Applies tags filters
        /// </summary>
        /// <param name="swaggerDoc">The Swagger document to be changed</param>
        private static void ApplyTags(OpenApiDocument swaggerDoc)
        {
            swaggerDoc.Tags = new List<OpenApiTag> {
                new() { Name = "XmlRpc", Description = "XmlRpc Reports" }
            };
        }
    }
}
