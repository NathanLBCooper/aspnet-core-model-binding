using System;
using System.Net.Http;
using DelimitingQueryStringValueProvider.ExampleApp.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace DelimitingQueryStringValueProvider.IntegrationTests
{
    public class IntegrationTestFixture : IDisposable
    {
        private readonly TestServer _server;

        public IntegrationTestFixture()
        {
            var builder = new WebHostBuilder().UseStartup<Startup>();
            _server = new TestServer(builder);

            Client = _server.CreateClient();
            Client.BaseAddress = new Uri("http://localhost:5000");
        }

        public HttpClient Client { get; }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }
}