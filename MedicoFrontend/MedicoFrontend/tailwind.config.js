/** @type {import('tailwindcss').Config} */
const defaultTheme = require('tailwindcss/defaultTheme')
export default {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      fontFamily: { 
        lora: ['"Lora"', ...defaultTheme.fontFamily.sans],
        montserrat: ['"Montserrat"', ...defaultTheme.fontFamily.sans],
      },
      borderRadius: {
        '4xl' : '50px',
        '5xl' : '70px', 
        '6xl' : '90px'
      },
      colors: {
        secondaryColor: '#03122F',
        secondaryTextColor: '#DADFF7'
      },
      spacing: {
        texAreaSize: '30.00rem', 
      }
    },
  },
  plugins: [],
}

