using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PokemonTypeEffectivenessConsoleApp.Models;

namespace PokemonTypeEffectivenessConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Configure Dependency Injection
            var serviceProvider = new ServiceCollection()
                .AddSingleton<HttpClient>()
                .AddSingleton<PokeApiService>()
                .BuildServiceProvider();

            var pokeApiService = serviceProvider.GetRequiredService<PokeApiService>();

            Console.WriteLine("Welcome to the Pokémon Type Effectiveness Checker!");

            while (true)
            {
                Console.Write("Enter a Pokémon name (or type 'exit' to quit): ");
                var input = Console.ReadLine();

                // Check if the input is "exit" to allow quitting the application
                if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase))
                    break;

                // Validate user input
                if (!IsValidInput(input))
                    continue;

                try
                {
                    var pokemonTypeData = await pokeApiService.GetPokemonTypeDataAsync(input);

                    if (pokemonTypeData == null)
                    {
                        Console.WriteLine("Could not retrieve data. Please check the Pokémon name and try again.");
                    }
                    else
                    {
                        DisplayTypeEffectiveness(pokemonTypeData);
                    }
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("Network error. Please check your internet connection and try again.");
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("The request timed out. Please try again later.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                }
            }
        }

        static bool IsValidInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be empty. Please enter a valid Pokémon name.");
                return false;
            }

            if (!IsAlphabetic(input))
            {
                Console.WriteLine("Invalid input. Please enter only alphabetic characters (e.g., 'pikachu').");
                return false;
            }

            return true;
        }

        static bool IsAlphabetic(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsLetter(c))
                    return false;
            }
            return true;
        }

        static void DisplayTypeEffectiveness(PokemonTypeModel pokemonType)
        {
            Console.WriteLine($"\nEffectiveness for {pokemonType.Name.ToUpper()}");

            Console.WriteLine("\nStrong Against:");
            Console.WriteLine("Double Damage To: " + string.Join(", ", pokemonType.DamageRelations.DoubleDamageTo.Select(d => d.Name) ?? new[] { "None" }));
            Console.WriteLine("Half Damage From: " + string.Join(", ", pokemonType.DamageRelations.HalfDamageFrom.Select(d => d.Name) ?? new[] { "None" }));
            Console.WriteLine("No Damage From: " + string.Join(", ", pokemonType.DamageRelations.NoDamageFrom.Select(d => d.Name) ?? new[] { "None" }));

            Console.WriteLine("\nWeak Against:");
            Console.WriteLine("No Damage To: " + string.Join(", ", pokemonType.DamageRelations.NoDamageTo.Select(d => d.Name) ?? new[] { "None" }));
            Console.WriteLine("Half Damage To: " + string.Join(", ", pokemonType.DamageRelations.HalfDamageTo.Select(d => d.Name) ?? new[] { "None" }));
            Console.WriteLine("Double Damage From: " + string.Join(", ", pokemonType.DamageRelations.DoubleDamageFrom.Select(d => d.Name) ?? new[] { "None" }));

            Console.WriteLine();
        }
    }
}
