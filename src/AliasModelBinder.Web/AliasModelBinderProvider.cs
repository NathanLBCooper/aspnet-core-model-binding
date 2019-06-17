using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders; // todo can we get rid of non-abstraction dependency in this library?
using Microsoft.Extensions.Logging;

namespace AliasModelBinder.Web
{
    public class AliasModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.IsComplexType && !context.Metadata.IsCollectionType)
            {
                var propertyBinders = new Dictionary<ModelMetadata, IModelBinder>();

                foreach (var property in context.Metadata.Properties)
                {
                    propertyBinders.Add(property, context.CreateBinder(property));
                }

                return new AliasModelBinder(propertyBinders,
                    (ILoggerFactory)context.Services.GetService(typeof(ILoggerFactory)), true);
            }

            return null;
        }

        /// <summary>
        /// Setup the AliasModelBinderProvider Mvc project to use BindingAlias attribute, to allow for aliasing property names in query strings
        /// </summary>
        public static void Configure(IList<IModelBinderProvider> providers)
        {
            // Place in front of ComplexTypeModelBinderProvider to replace this binder type in practice
            for (int i = 0; i < providers.Count; i++)
            {
                if (providers[i] is ComplexTypeModelBinderProvider)
                {
                    providers.Insert(i, new AliasModelBinderProvider());
                    return;
                }
            }

            providers.Add(new AliasModelBinderProvider());
        }
    }
}