# Weather App VB.NET

A Windows Forms desktop application for displaying weather information, migrated from the React web application.

## Features

- Search weather by city name
- Display current temperature, weather condition, and location
- Show additional details: feels like temperature, humidity, wind speed
- Weather icon mapping based on conditions
- API key configuration via Settings menu

## Requirements

- .NET 6.0 SDK or later
- Windows OS (Windows Forms)
- OpenWeatherMap API key (free tier available at https://openweathermap.org/api)

## Setup

1. **Build the project:**
   ```bash
   cd WeatherAppVB
   dotnet build
   ```

2. **Run the application:**
   ```bash
   dotnet run
   ```

3. **Configure API Key:**
   - Go to `Settings` > `API Key...` in the menu
   - Enter your OpenWeatherMap API key
   - Click Save

## Usage

1. Enter a city name in the search box
2. Press Enter to search
3. Weather data will be displayed including:
   - Weather icon (emoji)
   - Temperature (°F)
   - Weather condition
   - City name
   - Feels like temperature
   - Humidity percentage
   - Wind speed (MPH)

## Project Structure

```
WeatherAppVB/
├── Program.vb              # Application entry point
├── WeatherAppVB.vbproj     # Project file
├── Models/
│   └── WeatherData.vb      # Data models for API response
├── Services/
│   └── WeatherService.vb   # HTTP client for OpenWeatherMap API
└── Forms/
    ├── MainForm.vb         # Main application window
    └── ApiKeyDialog.vb     # API key configuration dialog
```

## API Reference

This application uses the [OpenWeatherMap Current Weather API](https://openweathermap.org/current):
- Endpoint: `https://api.openweathermap.org/data/2.5/weather`
- Units: Imperial (Fahrenheit, MPH)

## Migration Notes

This VB.NET application provides the same core functionality as the React web app:
- Same API integration (OpenWeatherMap)
- Same data display (temperature, condition, humidity, wind, feels like)
- Same weather icon mapping logic
- Same error handling patterns
