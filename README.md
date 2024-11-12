# Pokemon Type Effectiveness Console App

Welcome to the **Pokemon Type Effectiveness Console App**! This is a console application that allows users to enter a Pokémon's name and retrieve type effectiveness information using data from the PokéAPI. The app shows which Pokémon types are strong or weak against the input Pokémon.

## Table of Contents

- [Features](#features)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Environment Variables](#environment-variables)
- [Usage](#usage)
- [Error Handling](#error-handling)
- [Technologies Used](#technologies-used)
- [Testing](#testing)
- [Project Structure](#project-structure)

## Features

- Retrieve and display type effectiveness information for a given Pokémon.
- Handles and displays error messages for invalid inputs or connectivity issues.
- User-friendly console prompts to ensure valid inputs.chips.
- **Copy URL to Clipboard**: Click a button to copy a GIF URL for easy sharing.
- **Responsive Design**: Suitable for both desktop and mobile use.

## Prerequisites

Ensure you have the following installed:

- **.NET SDK** (v6.0 or higher)
- An IDE or text editor like **Visual Studio** or **VS Code**

## Installation

1. To get started, clone the repository and install the required dependencies.

```sh
git clone https://github.com/Haritha8/pokemon-type-effectiveness-console.git
```

2. Navigate to the project directory:

```sh
cd PokemonTypeEffectivenessConsoleApp
```

3. Restore dependencies:

```sh
  dotnet restore
```

## Environment Variables

The application utilizes PokéAPI

You can get this at (https://pokeapi.co/).

## Usage

1. Start the application by running:

```sh
dotnet run --project PokemonTypeEffectivenessConsoleApp
```

2. Once started, Follow the on-screen instructions:

```sh
Enter a Pokémon name (e.g., pikachu) to see type effectiveness details.
Type exit to quit the application.
```

3. Output example:

```sh
Welcome to the Pokémon Type Effectiveness Checker!
Enter a Pokémon name (or type 'exit' to quit): pikachu

Effectiveness for PIKACHU

Strong Against:
- Double Damage To: Water, Flying
- Half Damage From: Electric, Flying, Steel
- No Damage From: None

Weak Against:
- No Damage To: Ground
- Half Damage To: Electric, Grass
- Double Damage From: Ground
```

## **Error Handling**:

The application handles the following scenarios: -**Empty Input**: Prompts the user to enter a valid name. -**Invalid Characters**: Prompts the user to enter only alphabetic characters (e.g., no numbers). -**Network Issues**: Displays a message if there’s an issue reaching the API. -**Timeout**: If the API request times out, an appropriate message is displayed. -**Invalid Pokémon Name**: If the Pokémon is not found, the user is notified.

- Other errors, such as connectivity issues, are also handled to ensure a smooth user experience.

## Technologies Used

- **.NET Core 6.0** - Framework for building the console application.
- **HttpClient** - For making HTTP requests to the PokéAPI.
- **Dependency Injection** - Manages dependencies for HttpClient and PokeApiService.
- **xUnit** - For unit testing.

## Testing

To run unit tests, navigate to the PokemonTypeEffectivenessConsoleAppTests directory and use the following command:

```sh
dotnet test
```

The tests cover various scenarios such as:

- Valid Pokémon names returning type data.
- Invalid Pokémon names (404 errors).
- Network issues, including timeouts and server errors.

## Folder Structure

```plaintext
PokemonTypeEffectivenessConsoleApp/
├── Models/
│   ├── DamageTypeModel.cs
│   ├── PokemonTypeModel.cs
│   └── TypeRelationsModel.cs
├── Services/
│   └── PokeApiService.cs
├── Program.cs
├── PokemonTypeEffectivenessConsoleApp.csproj
└── README.md

PokemonTypeEffectivenessConsoleAppTests/
├── PokeApiServiceTests.cs
└── PokemonTypeEffectivenessConsoleAppTests.csproj
├── README.md                  # You're here!
```

Happy exploring Pokémon type effectiveness! 🎉
