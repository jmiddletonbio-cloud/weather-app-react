// DEVIN -> Import React for component rendering
import React from 'react';
// DEVIN -> Import testing utilities from React Testing Library
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
// DEVIN -> Import jest-dom for extended DOM assertions
import '@testing-library/jest-dom';
// DEVIN -> Import axios for mocking
import axios from 'axios';
// DEVIN -> Import the App component to test
import App from './App';

// DEVIN -> Mock axios module to prevent real API calls
jest.mock('axios');
// DEVIN -> Cast axios to mocked type for TypeScript compatibility
const mockedAxios = axios;

// DEVIN -> Define mock weather data matching OpenWeatherMap API structure
const mockWeatherData = {
  // DEVIN -> City name from API response
  name: 'New York',
  // DEVIN -> Main weather metrics
  main: {
    // DEVIN -> Current temperature in Fahrenheit
    temp: 75,
    // DEVIN -> Feels like temperature
    feels_like: 72,
    // DEVIN -> Humidity percentage
    humidity: 65
  },
  // DEVIN -> Weather conditions array
  weather: [
    {
      // DEVIN -> Primary weather condition
      main: 'Clear'
    }
  ],
  // DEVIN -> Wind information
  wind: {
    // DEVIN -> Wind speed in MPH
    speed: 10
  }
};

// DEVIN -> Test suite for App component
describe('App Component', () => {
  // DEVIN -> Reset all mocks before each test
  beforeEach(() => {
    // DEVIN -> Clear mock call history
    jest.clearAllMocks();
  });

  // DEVIN -> Test suite for searchLocation function
  describe('searchLocation function', () => {
    // DEVIN -> Test: Enter key triggers API call and displays weather data
    test('Enter key triggers API call and displays weather data', async () => {
      // DEVIN -> Setup axios mock to resolve with weather data
      mockedAxios.get.mockResolvedValueOnce({ data: mockWeatherData });

      // DEVIN -> Render the App component
      render(<App />);

      // DEVIN -> Find the input field by placeholder text
      const inputField = screen.getByPlaceholderText('Enter Location');

      // DEVIN -> Simulate user typing a location
      fireEvent.change(inputField, { target: { value: 'New York' } });

      // DEVIN -> Simulate Enter key press
      fireEvent.keyPress(inputField, {
        // DEVIN -> Key property for Enter
        key: 'Enter',
        // DEVIN -> Code property for Enter
        code: 'Enter',
        // DEVIN -> Character code for Enter key
        charCode: 13
      });

      // DEVIN -> Verify axios.get was called
      expect(mockedAxios.get).toHaveBeenCalledTimes(1);

      // DEVIN -> Wait for the weather data to appear in the UI
      await waitFor(() => {
        // DEVIN -> Verify location name is displayed
        expect(screen.getByText('New York')).toBeInTheDocument();
      });

      // DEVIN -> Verify temperature is displayed (75°F rounded)
      expect(screen.getByText('75°F')).toBeInTheDocument();

      // DEVIN -> Verify weather condition is displayed
      expect(screen.getByText('Clear')).toBeInTheDocument();

      // DEVIN -> Verify feels like temperature is displayed
      expect(screen.getByText('72°F')).toBeInTheDocument();

      // DEVIN -> Verify humidity is displayed
      expect(screen.getByText('65%')).toBeInTheDocument();

      // DEVIN -> Verify wind speed is displayed
      expect(screen.getByText('10 MPH')).toBeInTheDocument();
    });

    // DEVIN -> Test: Non-Enter keys don't trigger API call
    test('Non-Enter keys do not trigger API call', () => {
      // DEVIN -> Render the App component
      render(<App />);

      // DEVIN -> Find the input field by placeholder text
      const inputField = screen.getByPlaceholderText('Enter Location');

      // DEVIN -> Simulate user typing a location
      fireEvent.change(inputField, { target: { value: 'New York' } });

      // DEVIN -> Simulate Tab key press (not Enter)
      fireEvent.keyPress(inputField, {
        // DEVIN -> Key property for Tab
        key: 'Tab',
        // DEVIN -> Code property for Tab
        code: 'Tab',
        // DEVIN -> Character code for Tab key
        charCode: 9
      });

      // DEVIN -> Verify axios.get was NOT called
      expect(mockedAxios.get).not.toHaveBeenCalled();
    });

    // DEVIN -> Test: State updates after successful search
    test('Input field is cleared after successful search', async () => {
      // DEVIN -> Setup axios mock to resolve with weather data
      mockedAxios.get.mockResolvedValueOnce({ data: mockWeatherData });

      // DEVIN -> Render the App component
      render(<App />);

      // DEVIN -> Find the input field by placeholder text
      const inputField = screen.getByPlaceholderText('Enter Location');

      // DEVIN -> Simulate user typing a location
      fireEvent.change(inputField, { target: { value: 'New York' } });

      // DEVIN -> Verify input has the typed value before search
      expect(inputField).toHaveValue('New York');

      // DEVIN -> Simulate Enter key press
      fireEvent.keyPress(inputField, {
        // DEVIN -> Key property for Enter
        key: 'Enter',
        // DEVIN -> Code property for Enter
        code: 'Enter',
        // DEVIN -> Character code for Enter key
        charCode: 13
      });

      // DEVIN -> Wait for the input field to be cleared
      await waitFor(() => {
        // DEVIN -> Verify input field is cleared after search
        expect(inputField).toHaveValue('');
      });

      // DEVIN -> Wait for weather data to be displayed
      await waitFor(() => {
        // DEVIN -> Verify weather data is displayed correctly
        expect(screen.getByText('New York')).toBeInTheDocument();
      });

      // DEVIN -> Verify all weather metrics are displayed
      expect(screen.getByText('75°F')).toBeInTheDocument();
      // DEVIN -> Verify feels like temperature
      expect(screen.getByText('72°F')).toBeInTheDocument();
      // DEVIN -> Verify humidity percentage
      expect(screen.getByText('65%')).toBeInTheDocument();
      // DEVIN -> Verify wind speed
      expect(screen.getByText('10 MPH')).toBeInTheDocument();
    });
  });
});
