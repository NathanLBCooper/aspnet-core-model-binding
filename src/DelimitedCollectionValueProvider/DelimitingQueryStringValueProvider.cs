using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;

namespace DelimitedCollectionValueProvider
{
    public class DelimitingQueryStringValueProvider : QueryStringValueProvider
    {
        private readonly char[] _delimiters;
        private readonly CultureInfo _culture;
        private readonly IQueryCollection _queryCollection;

        public DelimitingQueryStringValueProvider(
            BindingSource bindingSource,
            IQueryCollection values,
            CultureInfo culture,
            char[] delimiters)
            : base(bindingSource, values, culture)
        {
            _delimiters = delimiters;
            _queryCollection = values;
            _culture = culture;
        }

        public override ValueProviderResult GetValue(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var values = _queryCollection[key];
            if (!values.Any())
            {
                return ValueProviderResult.None;
            }

            values = SplitOnDelimiters(values, _delimiters);

            return new ValueProviderResult(values, _culture);
        }

        private static StringValues SplitOnDelimiters(StringValues values, char[] delimiters)
        {
            if (values.Any(v => delimiters.Any(v.Contains)))
            {
                return new StringValues(values
                    .SelectMany(x => x.Split(delimiters, StringSplitOptions.RemoveEmptyEntries))
                    .ToArray());
            }

            return values;
        }
    }
}
