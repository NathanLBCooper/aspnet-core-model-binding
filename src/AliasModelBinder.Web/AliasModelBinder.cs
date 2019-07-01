using System;
using System.Collections.Concurrent;
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
        private static readonly string[] UnAliased = new string[0];
        private static readonly ConcurrentDictionary<Property, string[]> Aliases = new ConcurrentDictionary<Property, string[]>();

        public AliasModelBinder(IDictionary<ModelMetadata, IModelBinder> propertyBinders, ILoggerFactory loggerFactory,
            bool allowValidatingTopLevelNodes)
            : base(propertyBinders, loggerFactory, allowValidatingTopLevelNodes)
        {
        }

        protected override Task BindProperty(ModelBindingContext bindingContext)
        {
            var containerType = bindingContext.ModelMetadata?.ContainerType;
            if (containerType != null)
            {
                var aliases = Aliases.GetOrAdd(new Property { ContainerType = containerType, Name = bindingContext.ModelMetadata.PropertyName }, GetAliases);

                if (aliases.Any())
                {
                    bindingContext.ValueProvider = new AliasValueProvider(bindingContext.ValueProvider,
                        bindingContext.ModelName, aliases);
                }
            }

            return base.BindProperty(bindingContext);
        }

        private static string[] GetAliases(Property property)
        {
            var attributes = property.ContainerType.GetProperty(property.Name).GetCustomAttributes(true).OfType<BindingAliasAttribute>();
            return attributes.Any() ? attributes.Select(attr => attr.Alias).ToArray() : UnAliased;
        }

        private struct Property
        {
            public Type ContainerType;
            public string Name;
        }
    }
}