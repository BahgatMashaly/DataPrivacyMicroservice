using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Filters;

namespace DataPrivacyMicroservice.DataPrivacyManagementLayer.Authorization
{
    public class AddAuthorizationHeaderParameterOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var filterPipeline = apiDescription.ActionDescriptor.GetFilterPipeline();
            var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Instance)
                .Any(filter => filter is IAuthorizationFilter);

            var allowAnonymous =
                apiDescription.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();

            if (isAuthorized && !allowAnonymous)
            {
                if (operation.parameters == null)
                    operation.parameters = new List<Parameter>();

                operation.parameters?.Add(new Parameter
                {
                    name = "Authorization",
                    @in = "header",
                    description = "Basic HTTP Base64 encoded Header Authorization",
                    required = true,
                    type = "string"
                });
            }
        }

     
    }
}