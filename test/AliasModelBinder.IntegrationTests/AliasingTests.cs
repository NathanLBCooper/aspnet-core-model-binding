using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace AliasModelBinder.IntegrationTests
{
    public class AliasingTests : IntegrationClassFixture
    {
        private readonly HttpClient _client;

        public AliasingTests(IntegrationTestFixture fixture)
        {
            this._client = fixture.Client;
        }

        [Theory]
        [InlineData("number", 1)]
        [InlineData("num", 2)]
        [InlineData("n", 3)]
        public async Task can_use_alias_in_complex_type_request(string parameterName, int value)
        {
            var response = await _client.GetAsync($"test/echo?{parameterName}={value}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsAsync<int>();

            content.Should().Be(value);
        }

        [Theory]
        [InlineData("number=1&number=2", 1)] // first is chosen, normal behaviour
        [InlineData("number=1&n=2", 1)] // ditto
        [InlineData("n=1&number=2", 2)] // property name has priority todo this may be a problem
        [InlineData("n=1&num=2", 2)] // based on the order of the attributes todo this may be a problem
        public async Task uses_priority_order_when_picking_between_duplicate_aliases(string queryString, int value)
        {
            var response = await _client.GetAsync($"test/echo?{queryString}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsAsync<int>();

            content.Should().Be(value);
        }

        [Theory]
        [InlineData("integers=1&integers=2", 1, 2)] // use property name
        [InlineData("int=1", 1)] // use an alias
        [InlineData("int=1&integers=2", 1, 2)] // use both
        [InlineData("int=5&i=10&integers=1", 5, 10, 1)] // use multiple aliases
        [InlineData("INT=1&I=2&intEGers=3", 1, 2, 3)] // check case still doesn't matter
        public async Task can_use_alias_in_collection_complex_type_request(string queryString, params int[] numbers)
        {
            var response = await _client.GetAsync($"test/echoCollection?{queryString}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsAsync<int[]>();

            content.Should().BeEquivalentTo(numbers);
        }

        [Theory]
        [InlineData("leftSummand=9&rightSummand=6", 15)] // use property names
        [InlineData("l=3&r=7", 10)] // use aliases
        public async Task can_use_alias_for_multiple_fields(string queryString, int result)
        {
            var response = await _client.GetAsync($"test/add?{queryString}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsAsync<int>();

            content.Should().Be(result);
        }

        [Theory]
        // todo. This is terrible behaviour, it does it's best using the (also problematic) priority picking, but we just need to throw or something
        [InlineData("term=3&r=7", 10)]
        [InlineData("l=3&term=7", 10)]
        [InlineData("term=3&term=7", 6)]
        public async Task deals_with_naming_collisions_in_a_confusing_way(string queryString, int result)
        {
            var response = await _client.GetAsync($"test/add?{queryString}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsAsync<int>();

            content.Should().Be(result);
        }
    }
}
