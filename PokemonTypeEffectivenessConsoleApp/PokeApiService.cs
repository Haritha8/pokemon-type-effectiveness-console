using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using PokemonTypeEffectivenessConsoleApp.Models;

namespace PokemonTypeEffectivenessConsoleApp
{
    public class PokeApiService
    {
        private readonly HttpClient _httpClient;

        public PokeApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PokemonTypeModel> GetPokemonTypeDataAsync(string pokemonName)
        {
            try
            {
                // Attempt to retrieve Pokémon data
                var pokemonResponseMessage = await _httpClient.GetAsync("https://pokeapi.co/api/v2/pokemon/" + pokemonName.ToLower());

                // Check if the response indicates a 404 Not Found error
                if (pokemonResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("No Pokémon found with the given name. Please try again with a correct name.");
                    return null;
                }

                // Check for other HTTP errors
                pokemonResponseMessage.EnsureSuccessStatusCode();

                // Deserialize the JSON response if successful
                var pokemonResponse = await pokemonResponseMessage.Content.ReadFromJsonAsync<PokemonResponse>();

                if (pokemonResponse?.Types == null || pokemonResponse.Types.Count == 0)
                {
                    Console.WriteLine("No types found for this Pokémon.");
                    return null;
                }

                // Get type details URL
                var typeUrl = pokemonResponse.Types[0].Type.Url;

                // Fetch type details
                var typeResponseMessage = await _httpClient.GetAsync(typeUrl);

                // Check for 404 in the type response as well
                if (typeResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("Type data not found for this Pokémon.");
                    return null;
                }

                typeResponseMessage.EnsureSuccessStatusCode();
                var typeData = await typeResponseMessage.Content.ReadFromJsonAsync<TypeApiResponse>();

                if (typeData == null)
                {
                    Console.WriteLine("No type data found for this Pokémon.");
                    return null;
                }

                // Map to PokemonTypeModel
                return new PokemonTypeModel
                {
                    Name = typeData.Name,
                    DamageRelations = typeData.DamageRelations
                };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Network error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return null;
        }

        private class PokemonResponse
        {
            public List<PokemonType> Types { get; set; }
        }

        private class PokemonType
        {
            public TypeData Type { get; set; }
        }

        private class TypeData
        {
            public string Url { get; set; }
        }

        private class TypeApiResponse
        {
            public string Name { get; set; }
            [JsonPropertyName("damage_relations")]
            public TypeRelationsModel DamageRelations { get; set; }
        }
    }
}
