using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace DelimitedCollectionValueProvider.IntegrationTests
{
    public class DelimitedCollectionsTests : IntegrationClassFixture
    {
        private const char Delimiter = ',';

        private readonly HttpClient _client;

        public DelimitedCollectionsTests(IntegrationTestFixture fixture)
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
    }
}
