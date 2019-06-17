using System;

namespace AliasModelBinder.Client
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class BindingAliasAttribute : Attribute
    {
        public string Alias { get; }

        public BindingAliasAttribute(string alias)
        {
            Alias = alias;
        }
    }
}