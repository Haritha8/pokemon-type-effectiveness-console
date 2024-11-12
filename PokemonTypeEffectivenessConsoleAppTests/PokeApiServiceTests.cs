using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using PokemonTypeEffectivenessConsoleApp.Models;
using Xunit;

namespace PokemonTypeEffectivenessConsoleApp.Tests
{
    public class PokeApiServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly PokeApiService _pokeApiService;

        public PokeApiServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _pokeApiService = new PokeApiService(_httpClient);
        }

        [Fact]
        public async Task GetPokemonTypeDataAsync_ReturnsPokemonTypeModel_WhenApiResponseIsValid()
        {
            // Arrange - Set up mock response for Pokémon data
            _httpMessageHandlerMock.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(@"{ ""types"": [{ ""type"": { ""url"": ""https://pokeapi.co/api/v2/type/grass"" } }] }")
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(@"{ ""name"": ""grass"", ""damage_relations"": { ""double_damage_to"": [{ ""name"": ""water"" }], ""double_damage_from"": [{ ""name"": ""fire"" }], ""half_damage_to"": [{ ""name"": ""electric"" }], ""half_damage_from"": [{ ""name"": ""ground"" }], ""no_damage_to"": [], ""no_damage_from"": [] } }")
                });

            // Act
            var result = await _pokeApiService.GetPokemonTypeDataAsync("bulbasaur");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("grass", result.Name);
            Assert.Single(result.DamageRelations.DoubleDamageTo);
            Assert.Equal("water", result.DamageRelations.DoubleDamageTo[0].Name);
        }

        [Fact]
        public async Task GetPokemonTypeDataAsync_ReturnsNull_WhenApiResponseIs404NotFound()
        {
            // Arrange - Set up mock response with a 404 error
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            // Act
            var result = await _pokeApiService.GetPokemonTypeDataAsync("invalidpokemon");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetPokemonTypeDataAsync_ReturnsNull_WhenApiResponseIsServerError()
        {
            // Arrange - Set up mock response with a 500 Internal Server Error
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            // Act
            var result = await _pokeApiService.GetPokemonTypeDataAsync("bulbasaur");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetPokemonTypeDataAsync_ReturnsNull_WhenNetworkErrorOccurs()
        {
            // Arrange - Simulate a network error
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act
            var result = await _pokeApiService.GetPokemonTypeDataAsync("bulbasaur");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetPokemonTypeDataAsync_ReturnsNull_WhenRequestTimesOut()
        {
            // Arrange - Simulate a timeout
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new TaskCanceledException("Request timed out"));

            // Act
            var result = await _pokeApiService.GetPokemonTypeDataAsync("bulbasaur");

            // Assert
            Assert.Null(result);
        }
    }
}
