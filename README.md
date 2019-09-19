# aspnet-core-model-binding

Containing the **Alias model binder** and **Delimiting Query String Value Provider**

[![Build status](https://ci.appveyor.com/api/projects/status/im76pxbt2kk793o0?svg=true)](https://ci.appveyor.com/project/NathanLBCooper/alias-model-binder)
![GitHub](https://img.shields.io/github/license/NathanLBCooper/alias-model-binder.svg)

| Package | Version |
| --- | --- |
| **AliasModelBinder Client** | [![nuget](https://img.shields.io/nuget/v/AliasModelBinder.Client.svg)](https://www.nuget.org/packages/AliasModelBinder.Client/) |
| **AliasModelBinder Web** | [![nuget](https://img.shields.io/nuget/v/AliasModelBinder.Web.svg)](https://www.nuget.org/packages/AliasModelBinder.Web/) |
| **DelimitingQueryStringValueProvider Web** | [![nuget](https://img.shields.io/nuget/v/DelimitingQueryStringValueProvider.Web.svg)](https://www.nuget.org/packages/DelimitingQueryStringValueProvider.Web/) [![Join the chat at https://gitter.im/aspnet-core-model-binding/community](https://badges.gitter.im/aspnet-core-model-binding/community.svg)](https://gitter.im/aspnet-core-model-binding/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge) |

# Alias Model Binder

**A model binder for ASP.NET Core. Use multiple alternative property names in your query strings.**

For example, you can use this:

`api/controller/action?SomePropertyName=1&SomePropertyName=2` ,

`api/controller/action?SomeOtherThing=1&SomeOtherThing=2` and

`api/controller/action?s=1&s=2`

at the same time.

Why might you want to do this?

- Perhaps you've changed your request model property names and want to keep backward compatibility? 
- Perhaps you're trying to reduce query string length by using shortened property names.

**How do you add this to your project?**

Add [the web package](https://www.nuget.org/packages/AliasModelBinder.Web/) to your web project.

Then add an attribute to your request class

    public class SomeRequest
	{
    	[BindingAlias("n")]   // btw, this would be an awful property name. Thank goodness for the modelbinder
   		public int[] SomeNumbers{ get; set; }
	}


and configure your aspnetcore project to use the model binder in *Startup.cs*

    public void ConfigureServices(IServiceCollection services)
    {
            services
                .AddMvc()
                .AddMvcOptions(options =>
                {
                    AliasModelBinderProvider.Configure(options.ModelBinderProviders);
                });
    }
    
    
Now you can use both your original query `api/controller/action?someNumbers=1&someNumbers=2&someNumbers=3&someNumbers=4&someNumbers=5`as well as `api/controller/action?n=1&n=2&n=3&n=4&n=5`.
    
Optionally, if you want to define attributes in your client project, you can take [the client package](https://www.nuget.org/packages/AliasModelBinder.Client/), which contains just the attribute definitions and has no dependencies. If you're happy to (re)define your objects with the attributes in your web project, you don't need this package.
    
Look at the *AliasModelBinder.ExampleApp* in the `test/` folder to see a full example of an application using alias model binding.


# Delimiting Query String Value Provider

**A Value Provider for ASP.NET Core. Pass less verbose arrays in your query strings.**

We've seen queries that look like this: `api/controller/action?number=1&number=2&number=3&number=4&number=5`. But wouldn't it be simpler if we used the Delimiting Query String Provider to write something like this instead:

	api/controller/action?number=1,2,3,5

Why would you want to do this?

- Shorter query strings
- You find this syntax clearer

**How do you add this to your project?**

Add the [web package](https://www.nuget.org/packages/DelimitingQueryStringValueProvider.Web/) to your web project.

Then just configure your aspnet core to use it in *Startup.cs*

    public void ConfigureServices(IServiceCollection services)
    {
            services
                .AddMvc()
                .AddMvcOptions(options =>
                {
                    DelimitingQueryStringValueProviderFactory.Configure(options.ValueProviderFactories);
                });
    }

There's an extra argument to pass a custom choice of delimiter. *","* is the default.

I could have passed *"_"* and had query strings like `api/controller/action?number=1_2_3` for example.

Now you can use this alternative syntax for all the collections in your query strings.

Look at the *DelimitingQueryStringValueProvider.ExampleApp* in the test/ folder to see a full example of an application using alias model binding.



## compatiblity

All web packages have a dependancy on *Microsoft.AspNetCore.Mvc.Core* version 2.2.5 (not the abstractions package *Microsoft.AspNetCore.Mvc.Abstractions*), and so are only compatible with web projects using a version of *Microsoft.AspNetCore.Mvc.Core* that's binary compatible with 2.2.5.

Client packages have no dependancies.
