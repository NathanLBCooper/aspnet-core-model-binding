using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DelimitingQueryStringValueProvider.Web
{
    public class DelimitingQueryStringValueProviderFactory : IValueProviderFactory
    {
        private readonly char[] _delimiters;

        public DelimitingQueryStringValueProviderFactory()
            : this(',')
        {
        }

        public DelimitingQueryStringValueProviderFactory(params char[] delimiters)
        {
            if (delimiters == null) throw new ArgumentNullException(nameof(delimiters));
            if (!delimiters.Any()) throw new ArgumentException($"Array cannot be empty", nameof(delimiters));

            _delimiters = delimiters;
        }

        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var valueProvider = new Web.DelimitingQueryStringValueProvider(
                BindingSource.Query,
                context.ActionContext.HttpContext.Request.Query,
                CultureInfo.InvariantCulture,
                _delimiters);

            context.ValueProviders.Add(valueProvider);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Setup mvc project to start accepting arrays in the query strings in format arr=1,2,3.
        /// Where ',' is your custom choice of delimiters
        /// </summary>
        public static void Configure(IList<IValueProviderFactory> valueProviderFactories, params char[] delimiters)
        {
            InsertIntoFactories(valueProviderFactories, new DelimitingQueryStringValueProviderFactory(delimiters));
        }

        /// <summary>
        /// Setup mvc project to start accepting arrays in the query strings in format arr=1,2,3
        /// </summary>
        public static void Configure(IList<IValueProviderFactory> valueProviderFactories)
        {
            InsertIntoFactories(valueProviderFactories, new DelimitingQueryStringValueProviderFactory());
        }

        private static void InsertIntoFactories(IList<IValueProviderFactory> factories, DelimitingQueryStringValueProviderFactory factory)
        {
            // Place in front of QueryStringValueProviderFactory to replace this factory type in practice
            for (int i = 0; i < factories.Count; i++)
            {
                if (factories[i] is QueryStringValueProviderFactory)
                {
                    factories.Insert(i, factory);
                    return;
                }
            }

            factories.Add(factory);
        }
    }
}
