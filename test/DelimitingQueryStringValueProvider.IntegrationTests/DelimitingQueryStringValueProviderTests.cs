using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace DelimitingQueryStringValueProvider.IntegrationTests
{
    public class DelimitingQueryStringValueProviderTests : IntegrationClassFixture
    {
        private const char Delimiter = ',';

        private readonly HttpClient _client;

        public DelimitingQueryStringValueProviderTests(IntegrationTestFixture fixture)
        {
            this._client = fixture.Client;
        }

        [Fact]
        public async Task can_make_delimited_collection_calls_to_endpoint_accepting_raw_collection()
        {
            var numbers = new[] {1, 2, 3};
            var response = await _client.GetAsync($"test/echoCollectionRaw?integers={string.Join(Delimiter, numbers)}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsAsync<int[]>();

            content.Should().BeEquivalentTo(numbers);
        }

        [Fact]
        public async Task can_make_delimited_collection_calls_to_endpoint_accepting_model_with_collection()
        {
            var numbers = new[] { 1, 2, 3};
            var response = await _client.GetAsync($"test/echoCollectionModel?integers={string.Join(Delimiter, numbers)}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsAsync<int[]>();

            content.Should().BeEquivalentTo(numbers);
        }

        [Fact]
        public async Task can_make_request_with_multiple_arrays()
        {
            var queryParams = $"one={string.Join(Delimiter, new[] {1, 2, 3})}"
                           + $"&two={string.Join(Delimiter, new[] {4, 5, 6})}"
                           + "&three=7&three=8&three=9";

            var response = await _client.GetAsync($"test/echoMultipleCollections?{queryParams}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsAsync<int[]>();

            content.Should().BeEquivalentTo(new[] {1, 2, 3, 4, 5, 6, 7, 8, 9});
        }
    }
}
