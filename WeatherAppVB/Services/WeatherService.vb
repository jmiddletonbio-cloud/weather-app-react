Imports System.Net.Http
Imports Newtonsoft.Json
Imports WeatherAppVB.Models

Namespace WeatherAppVB.Services
    ''' <summary>
    ''' Service for fetching weather data from OpenWeatherMap API
    ''' </summary>
    Public Class WeatherService
        Private ReadOnly _httpClient As HttpClient
        Private _apiKey As String

        Public Sub New()
            _httpClient = New HttpClient()
            _apiKey = String.Empty
        End Sub

        ''' <summary>
        ''' Sets the API key for OpenWeatherMap
        ''' </summary>
        Public Sub SetApiKey(apiKey As String)
            _apiKey = apiKey
        End Sub

        ''' <summary>
        ''' Gets the current API key
        ''' </summary>
        Public Function GetApiKey() As String
            Return _apiKey
        End Function

        ''' <summary>
        ''' Checks if API key is configured
        ''' </summary>
        Public Function HasApiKey() As Boolean
            Return Not String.IsNullOrWhiteSpace(_apiKey)
        End Function

        ''' <summary>
        ''' Fetches weather data for a given location
        ''' </summary>
        Public Async Function GetWeatherAsync(location As String) As Task(Of WeatherData)
            If Not HasApiKey() Then
                Throw New InvalidOperationException("API key is not configured. Please set your OpenWeatherMap API key in Settings.")
            End If

            If String.IsNullOrWhiteSpace(location) Then
                Throw New ArgumentException("Location cannot be empty.", NameOf(location))
            End If

            Dim url As String = $"https://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(location)}&units=imperial&appid={_apiKey}"

            Try
                Dim response As HttpResponseMessage = Await _httpClient.GetAsync(url)

                If response.StatusCode = Net.HttpStatusCode.Unauthorized OrElse response.StatusCode = Net.HttpStatusCode.Forbidden Then
                    Throw New HttpRequestException("API authentication failed. Please check your API key configuration.")
                ElseIf response.StatusCode = Net.HttpStatusCode.NotFound Then
                    Throw New HttpRequestException("Location not found. Please try a different city name.")
                ElseIf CInt(response.StatusCode) >= 500 Then
                    Throw New HttpRequestException("Weather service is temporarily unavailable. Please try again later.")
                End If

                response.EnsureSuccessStatusCode()

                Dim json As String = Await response.Content.ReadAsStringAsync()
                Dim weatherData As WeatherData = JsonConvert.DeserializeObject(Of WeatherData)(json)

                Return weatherData
            Catch ex As HttpRequestException
                Throw
            Catch ex As Exception
                Throw New HttpRequestException("Unable to fetch weather data. Please try again.", ex)
            End Try
        End Function

        ''' <summary>
        ''' Maps weather condition to an icon/emoji
        ''' </summary>
        Public Shared Function GetWeatherIcon(condition As String) As String
            If String.IsNullOrEmpty(condition) Then Return "☀️"

            Dim main As String = condition.ToLower()

            If main.Contains("cloud") Then Return "☁️"
            If main.Contains("rain") OrElse main.Contains("drizzle") Then Return "🌧️"
            If main.Contains("thunder") Then Return "⛈️"
            If main.Contains("snow") Then Return "❄️"
            If main.Contains("mist") OrElse main.Contains("fog") OrElse main.Contains("haze") Then Return "🌫️"
            If main.Contains("clear") Then Return "☀️"

            Return "🌤️"
        End Function
    End Class
End Namespace
