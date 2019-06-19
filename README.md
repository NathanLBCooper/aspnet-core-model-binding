# alias-model-binder
[![Build status](https://ci.appveyor.com/api/projects/status/im76pxbt2kk793o0?svg=true)](https://ci.appveyor.com/project/NathanLBCooper/alias-model-binder)
![GitHub](https://img.shields.io/github/license/NathanLBCooper/alias-model-binder.svg)

| Package | Version |
| --- | --- |
| **Client** | [![nuget](https://img.shields.io/nuget/v/AliasModelBinder.Web.svg)](https://www.nuget.org/packages/AliasModelBinder.Web/) |
| **Web** | [![nuget](https://img.shields.io/nuget/v/AliasModelBinder.Client.svg)](https://www.nuget.org/packages/AliasModelBinder.Client/) |

![alias-model-binder](https://i.imgur.com/yrErlSX.png)

**A model binder for ASP.NET Core. Use alternative, shorter property names in your query strings.**

**Turn**

`api/controller/action?SomePrettyLongPropertyName=1&SomePrettyLongPropertyName=2&SomePrettyLongPropertyName=3&SomePrettyLongPropertyName=4&SomePrettyLongPropertyName=5&SomePrettyLongPropertyName=6&SomePrettyLongPropertyName=7&SomePrettyLongPropertyName=8&SomePrettyLongPropertyName=9&SomePrettyLongPropertyName=10&SomePrettyLongPropertyName=11&SomePrettyLongPropertyName=12&SomePrettyLongPropertyName=13&SomePrettyLongPropertyName=14&SomePrettyLongPropertyName=15`

**into**

`api/controller/action?n=1&n=2&n=3&n=4&n=5&n=6&n=7&n=8&n=9&n=10&n=11&n=12&n=13&n=14&n=15`

**without changing anything in your code or sacrificing readability**

## is this the problem you're having?

**Are your query strings too long and do you wish that you could have shortened names just for these query strings without having to change the names of your actual properties?**

Let's take an example. Say you have a request object that looks like this

	public class SomeRequest
	{
   		public int[] SomeNumbers{ get; set; }
	}
    
Seems pretty normal, it even has a reasonably property name. But when passing collections in query strings even short names can add up fast. Here's an array of 10 integers:
    
    api/controller/action?SomeNumbers=1&SomeNumbers=2&SomeNumbers=3&SomeNumbers=4&SomeNumbers=5&SomeNumbers=6&SomeNumbers=7&SomeNumbers=8&SomeNumbers=9&SomeNumbers=10

That's pretty long.

There are many reasons that's a bad thing. It may be that you have a query string limit, or maybe it's just because it's clumsy and repetitive.

We could rename `SomeNumbers` to something shorter like `n`. But the cost of that is less readable code. Actually it's worse than that: less readable code in the public contact to your service.

If only there was some way to get shorten these query strings without changing my model. Well, there is. That's what Alias Model Binder is for.

### or maybe you have another problem Alias Model Binder solves:

It's not just for query string shortening. It's just the most obvious, and the problem I was dealing with when I created it. You can use it for anything where it's useful to have multiple names for the properties of a request object. Here are some examples that spring to mind:

- making backwards compatible name changes
- api translation
- ...

## using the model binder

Just add [the client package](https://www.nuget.org/packages/AliasModelBinder.Client/) to where you define your request objects, and [the web package](https://www.nuget.org/packages/AliasModelBinder.Web/) to your web project.

(*These two packages are seperate, because I don't want to force your client libraries to depend on ASP.NET*)

Then simply add an attribute to your request class

    public class SomeRequest
	{
    	[BindingAlias("n")]
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
    
    
Now you can use both your original query and this much shorter one:

    api/controller/action?n=1&n=2&n=3&n=4&n=5&n=6&n=7&n=8&n=9&n=10
    
Look at the ExampleApp in the `test/` folder to see a full example of an application using alias model binding.
    
## compatiblity

The web package has a dependancy on *Microsoft.AspNetCore.Mvc.Core* version 2.2.5 (not the abstractions package *Microsoft.AspNetCore.Mvc.Abstractions*), and so is only compatible with web projects using a version of *Microsoft.AspNetCore.Mvc.Core* that's binary compatible with 2.2.5.
