using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliasModelBinder.Client;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders; // todo can we get rid of non-abstraction dependency in this library?
using Microsoft.Extensions.Logging;

namespace AliasModelBinder.Web
{
    public class AliasModelBinder : ComplexTypeModelBinder
    {
        public AliasModelBinder(IDictionary<ModelMetadata, IModelBinder> propertyBinders, ILoggerFactory loggerFactory,
            bool allowValidatingTopLevelNodes)
            : base(propertyBinders, loggerFactory, allowValidatingTopLevelNodes)
        {
        }

        protected override Task BindProperty(ModelBindingContext bindingContext)
        {
            var containerType = bindingContext.ModelMetadata.ContainerType;
            if (containerType != null)
            {
                var propertyType = containerType.GetProperty(bindingContext.ModelMetadata.PropertyName);
                var attributes = propertyType.GetCustomAttributes(true);

                var aliasAttributes = attributes.OfType<BindingAliasAttribute>().ToArray();
                if (aliasAttributes.Any())
                {
                    bindingContext.ValueProvider = new AliasValueProvider(bindingContext.ValueProvider,
                        bindingContext.ModelName, aliasAttributes.Select(attr => attr.Alias));
                }
            }

            return base.BindProperty(bindingContext);
        }
    }
}