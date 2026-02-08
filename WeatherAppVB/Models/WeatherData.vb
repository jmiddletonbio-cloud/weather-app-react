Imports Newtonsoft.Json

Namespace WeatherAppVB.Models
    ''' <summary>
    ''' Main weather data response from OpenWeatherMap API
    ''' </summary>
    Public Class WeatherData
        <JsonProperty("name")>
        Public Property Name As String

        <JsonProperty("main")>
        Public Property Main As MainData

        <JsonProperty("weather")>
        Public Property Weather As List(Of WeatherCondition)

        <JsonProperty("wind")>
        Public Property Wind As WindData
    End Class

    ''' <summary>
    ''' Main weather metrics (temperature, humidity, etc.)
    ''' </summary>
    Public Class MainData
        <JsonProperty("temp")>
        Public Property Temp As Double

        <JsonProperty("feels_like")>
        Public Property FeelsLike As Double

        <JsonProperty("humidity")>
        Public Property Humidity As Integer
    End Class

    ''' <summary>
    ''' Weather condition information
    ''' </summary>
    Public Class WeatherCondition
        <JsonProperty("main")>
        Public Property Main As String

        <JsonProperty("description")>
        Public Property Description As String
    End Class

    ''' <summary>
    ''' Wind information
    ''' </summary>
    Public Class WindData
        <JsonProperty("speed")>
        Public Property Speed As Double
    End Class
End Namespace
