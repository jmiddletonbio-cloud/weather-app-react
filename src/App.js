import React, { useState } from 'react'
import axios from 'axios'
import './App.css'

const defaultData = {
  name: 'San Francisco',
  main: {
    temp: 68,
    feels_like: 65,
    humidity: 72
  },
  weather: [{ main: 'Partly Cloudy' }],
  wind: { speed: 12 }
}

function App() {
    const [data, setData] = useState(defaultData)
    const [location, setLocation] = useState('')
    const [error, setError] = useState(null)

  const url = `https://api.openweathermap.org/data/2.5/weather?q=${location}&units=imperial&appid=${process.env.REACT_APP_WEATHER_API_KEY}`

  const searchLocation = (event) => {
    if (event.key === 'Enter') {
      setError(null)
      axios.get(url)
        .then((response) => {
          setData(response.data)
        })
        .catch((error) => {
          console.error('API request failed:', error) // <------- ADDED: prints error to console
          const status = error.response?.status
          if (status === 401 || status === 403) {
            setError('API authentication failed. Please check your API key configuration.')
          } else if (status === 404) {
            setError('Location not found. Please try a different city name.')
          } else if (status >= 500) {
            setError('Weather service is temporarily unavailable. Please try again later.')
          } else {
            setError('Unable to fetch weather data. Please try again.')
          }
        })
      setLocation('')
    }
  }

  const getWeatherIcon = (condition) => {
    if (!condition) return '☀️'
    const main = condition.toLowerCase()
    if (main.includes('cloud')) return '☁️'
    if (main.includes('rain') || main.includes('drizzle')) return '🌧️'
    if (main.includes('thunder')) return '⛈️'
    if (main.includes('snow')) return '❄️'
    if (main.includes('mist') || main.includes('fog') || main.includes('haze')) return '🌫️'
    if (main.includes('clear')) return '☀️'
    return '🌤️'
  }

  return (
    <div className="app">
      <div className="glass-container">
        <div className="search-container">
          <input
            value={location}
            onChange={event => setLocation(event.target.value)}
            onKeyPress={searchLocation}
            placeholder='Search location...'
            type="text"
            className="search-input"
          />
        </div>

        {error && (
          <div className="error-message">
            {error}
          </div>
        )}

        <div className="circular-display">
          <div className="circular-outer">
            <div className="circular-inner">
              <div className="weather-icon">
                {data.weather ? getWeatherIcon(data.weather[0].main) : '🌤️'}
              </div>
              <div className="temperature">
                {data.main ? `${data.main.temp.toFixed()}°` : '--°'}
              </div>
              <div className="condition">
                {data.weather ? data.weather[0].main : 'Weather'}
              </div>
              <div className="location-name">
                {data.name || 'Enter a city'}
              </div>
            </div>
          </div>
        </div>

        {data.name && (
          <div className="details-container">
            <div className="detail-card">
              <div className="detail-value">
                {data.main ? `${data.main.feels_like.toFixed()}°` : '--'}
              </div>
              <div className="detail-label">Feels Like</div>
            </div>
            <div className="detail-card">
              <div className="detail-value">
                {data.main ? `${data.main.humidity}%` : '--'}
              </div>
              <div className="detail-label">Humidity</div>
            </div>
            <div className="detail-card">
              <div className="detail-value">
                {data.wind ? `${data.wind.speed.toFixed()}` : '--'}
              </div>
              <div className="detail-label">Wind MPH</div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}

export default App;
