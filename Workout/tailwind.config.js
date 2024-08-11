/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./**/*.{razor,html}"],
  theme: {
    extend: {
      colors: {
        primary: '#1690e8',
      },
      fontFamily: {
        sans: ["Poppins", "sans-serif"],
        rajdhani: ["Rajdhani", "sans-serif"],
      },
    },
  },
  plugins: [],
};
