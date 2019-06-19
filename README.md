# alias-model-binder

[![Build status](https://ci.appveyor.com/api/projects/status/im76pxbt2kk793o0?svg=true)](https://ci.appveyor.com/project/NathanLBCooper/alias-model-binder)

An asp.net core model binder for allowing alternative (typically shortened) property names on request models.

To briefly describe how this works, it basically takes the place of the default `ComplexTypeModelBinder`, but expands the fields it's looking for to include names defined in a special property attribute.

## is this model binder for you?

**Are your query strings too long and do you wish that you could have shortened names just for these query strings without having to change the names of your actual properties?**

Let's take an example. Say you have a request object that looks like this

	public class SomeRequest
	{
   		public int[] SomeNumbers{ get; set; }
	}
    
Seems pretty normal, it even has a reasonably property name. But when passing collections in query strings even short names can add up fast. Here's an array of 10 integers:
    
    api/controller/action?SomeNumbers=1&SomeNumbers=2&SomeNumbers=3&SomeNumbers=4&SomeNumbers=5&SomeNumbers=6&SomeNumbers=7&SomeNumbers=8&SomeNumbers=9&SomeNumbers=10

**This query string is long. It's not even got that much data in it, and a lot of that length is the property name. The longer that name gets, or the more data we pass, the worse it gets.**

You might have a limit on query string sizes, or you may just find these long strings clumsy. You've chosen to stick with using HttpGet and a query string, **but you need these query strings to be shorter**.

We could rename `SomeNumbers` to something shorter like `n`. But the cost of that is less readable code. Actually it's worse than that: less readable code in the public contact to your service. If only there was a way to have the short name for the query string, and the long name for the people reading your code. 

Well, yeah. Of course there is a way. That's why you're reading this.


## using the model binder

Just add **todo publish client package** to where you define your request objects, and **todo publish web package** to your web project.

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
