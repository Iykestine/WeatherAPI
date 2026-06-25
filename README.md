# Weather API

This is a weather API project that fetches and returns weather data from a 3rd party API. This project is based on the [Weather API Project Idea](https://roadmap.sh/projects/weather-api-wrapper-service) from the [roadmap.sh](https://roadmap.sh/) platform.

## Project Overview

Instead of relying on our own weather data, this project aims to build a weather API that fetches and returns weather data from a 3rd party API. This project will help you understand how to work with 3rd party APIs, caching, and environment variables.

The project uses [Visual Crossing's Weather API](https://www.visualcrossing.com/weather-api) as the 3rd party API to fetch weather data. This API is completely free and easy to use.

For caching the weather data, the project uses **IMemoryCache**, ASP.NET Core's built-in in-memory caching solution. The city name entered by the user is used as the cache key, and the result from calling the 3rd party API is cached with a configurable expiration time (e.g., 12 hours).

The project also includes **Swagger documentation** for the API, making it easier to understand and interact with the endpoints.

## Technologies Used

- **Programming Language:** [C#](https://learn.microsoft.com/en-us/dotnet/csharp/)
- **Framework:** [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/)
- **Caching:** [IMemoryCache](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/memory) (ASP.NET Core built-in)
- **API Documentation:** [Swagger / Swashbuckle](https://swagger.io/)

## Features

- Fetch weather data from a 3rd party API (Visual Crossing's Weather API)
- Cache the weather data using IMemoryCache to improve performance
- Use environment variables / configuration to store sensitive information like API keys
- Handle errors properly, such as when the 3rd party API is down or the city name is invalid
- Provide Swagger documentation for the API

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0 or later)
- A free API key from [Visual Crossing](https://www.visualcrossing.com/weather-api)

### Setup

1. Clone the repository:

```bash
git clone https://github.com/Iykestine/WeatherAPI.git
cd WeatherAPI
```

2. Configure your API key.

   Copy `appsettings.example.json` to `appsettings.json` and fill in your values:

```json
{
  "WeatherApi": {
    "ApiKey": "your-visual-crossing-api-key",
    "BaseUrl": "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline"
  }
}
```

3. Run the application:

```bash
dotnet run
```

4. The Weather API will be available at `http://localhost:7071` (or the port shown in your terminal).

5. Open the Swagger UI at:

```
http://localhost:7071/swagger/index.html
```

## Contribution

This project is part of the [roadmap.sh](https://roadmap.sh/) platform, which is designed to help developers learn and grow their skills. If you find any issues or have suggestions for improvement, feel free to contribute by submitting a pull request or opening an issue.

## License

This project is licensed under the [MIT License](./LICENSE).